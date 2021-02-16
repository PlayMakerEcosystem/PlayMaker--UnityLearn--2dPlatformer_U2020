// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace HutongGames.PlayMakerEditor.Ecosystem.Utils
{

	public static class EnumFileFinder
	{

		public static Dictionary<string,EnumFileDetails> FindEnumFiles()
		{
			
			//Lookup enums in file names
			Dictionary<string,EnumFileDetails> enumDetailsList = new Dictionary<string,EnumFileDetails>();
			
			classFiles = new List<string>();
			FindAllCSharpScriptFiles(Application.dataPath);
			
			//Lookup class name in the class file text 
			for (int i = 0; i < classFiles.Count; i++)
			{
				string filePath = classFiles[i];
				string codeFile = File.ReadAllText(filePath);

				if (codeFile.Contains("["+"PLAYMAKER_ENUM]")) // compose the tag to avoid this file to be found...
				{


					// read all lines, we are going to parse data
					string[] lines = File.ReadAllLines(filePath);

					// safety precaution
					if (lines.Length<10)
					{
						continue;
					}

					string nameSpace = lines[5].Substring(10);
					string enumName	= lines[7].Substring(13);

					EnumFileDetails _details = new EnumFileDetails(
						enumName, 
						nameSpace,
						filePath, 
						File.GetLastWriteTimeUtc(filePath)
						);

					enumDetailsList.Add (filePath,_details);
				}

			}

			return enumDetailsList;
		}

		static List<string> classFiles;
		static void FindAllCSharpScriptFiles(string startDir)
		{

			try
			{
				foreach (string file in Directory.GetFiles(startDir))
				{
					if (file.Contains(".cs"))
						classFiles.Add(file);
				}
				foreach (string dir in Directory.GetDirectories(startDir))
				{
					FindAllCSharpScriptFiles(dir);
				}
			}
			catch (System.Exception ex)
			{
				Debug.Log(ex.Message);
			}
		}
			
	}

	public class EnumFileDetails
	{
		string _enumName;

		/// <summary>
		/// Gets the name of the enum.
		/// </summary>
		/// <value>The name of the enum.</value>
		public string enumName 
		{ 
			get
			{
				return _enumName;
			}
		}

		string _nameSpace;
		/// <summary>
		/// Gets or sets the namespace.
		/// </summary>
		/// <value>The name space.</value>
		public string nameSpace
		{
			get
			{
			return _nameSpace;
			}
		}

		string _filePath;
		/// <summary>
		/// Gets the filePath.
		/// </summary>
		/// <value>The filepath of the enum.</value>
		public string filePath
		{
			get
			{
				return _filePath;
			}
		}

		string _projectPath;
		/// <summary>
		/// Gets the relative path from the project's assets folder.
		/// </summary>
		/// <value>The relative path of the enum.</value>
		public string projectPath
		{
			get
			{
				return _projectPath;
			}
		}

		System.DateTime _updateTime;
		/// <summary>
		/// Gets the update time.
		/// </summary>
		/// <value>The update time.</value>
		public System.DateTime updateTime
		{
			get
			{
				return _updateTime;
			}
		}
		
		/*
		string _fileName;
		/// <summary>
		/// Gets the fileName.
		/// This is a deduced property
		/// </summary>
		/// <value>The filename of the enum's file.</value>
		public string fileName
		{ 
			get
			{
				return _fileName;
			}
		}
	*/


		public override string ToString ()
		{
			return string.Format ("EnumFileDetails:\n" +
			                      "<b>enumName</b> {0}\n" +
			                      "<b>filePath</b> {1}\n" +
			                      "<b>projectPath</b> {2}\n" +
			                      "<b>updateTime</b> {3}", enumName, filePath,projectPath, updateTime);
		}
		
		internal EnumFileDetails() {}
		internal EnumFileDetails(string setEnumName,string setNameSpace, string setFilePath, System.DateTime setUpdateTime)
		{
			_enumName = setEnumName;
			_nameSpace = setNameSpace;
			_filePath = setFilePath;
			_updateTime = setUpdateTime;

			_projectPath =  filePath.Substring(Application.dataPath.Length+1);

		}


	}
}
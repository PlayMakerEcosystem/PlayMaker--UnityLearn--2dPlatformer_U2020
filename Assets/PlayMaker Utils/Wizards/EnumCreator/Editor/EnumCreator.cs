// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using UnityEditor;
using UnityEngine;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using System.CodeDom.Compiler;
using System.Text.RegularExpressions;

using System.Linq;

namespace HutongGames.PlayMakerEditor.Ecosystem.Utils
{
	
	// TODO: fix folder path validation
	// TODO: fix enum indexing to allow for proper editing

	public class EnumCreator {
	

		public class ValidationResult
		{
			public bool success = true;
			public string message = "";

			public ValidationResult(bool success,string message){
				this.success = success;
				this.message = message;
			}

			public static ValidationResult ValidResult()
			{
				return new ValidationResult(true,"");
			}
		}

		[Serializable]
		public class EnumDefinition
		{

			#region Data
			/// <summary>
			/// The name space.
			/// </summary>
			public string NameSpace = "Com.MyGame";

			/// <summary>
			/// The name.
			/// </summary>
			public string Name = "MyEnum";

			/// <summary>
			/// The folder path.
			/// </summary>
			public string FolderPath = "PlayMaker Custom Scripts/My Game/";

			/// <summary>
			/// The enum entries.
			/// </summary>
			public List<string> entries = new List<string>();

			/// <summary>
			/// The full filePath of this definition.
			/// deduced property or injected property 
			/// </summary>
			public string filePath = "";

			public string directoryPath ="";

			/// <summary>
			/// The Enum literal. it's only use for preview
			/// </summary>
			public string EnumLiteralPreview = "";

			/// <summary>
			/// The script literal.
			/// </summary>
			public string ScriptLiteral = "";

			#endregion


			public void UpdateFilePath()
			{
				
				string fileName = Name+".cs";
				directoryPath = Path.Combine(Application.dataPath, FolderPath);

				filePath = Path.Combine(directoryPath,fileName);
			}
			#region Validation

			public ValidationResult DefinitionValidation	= ValidationResult.ValidResult();
			public ValidationResult NameSpaceValidation		= ValidationResult.ValidResult();
			public ValidationResult NameValidation			= ValidationResult.ValidResult();
			public ValidationResult FolderPathValidation	= ValidationResult.ValidResult();
			public ValidationResult EntriesValidation		= ValidationResult.ValidResult();
			public Dictionary<string,ValidationResult> EntryValidations = new Dictionary<string, ValidationResult>();

			public ValidationResult ValidateDefinition()
			{
				NameSpaceValidation		= ValidateNameSpace();
				NameValidation			= ValidateName();
				FolderPathValidation	= ValidateFolderPath();
				EntriesValidation		= ValidateEntries();

				if (
					NameSpaceValidation.success
					&&
					NameValidation.success
					&& 
					FolderPathValidation.success
					&&
					EntriesValidation.success
					)
				{
					DefinitionValidation =  ValidationResult.ValidResult();
				}else{
					DefinitionValidation = new ValidationResult(false,"Invalid Definition: Please correct fields with errors");
				}

				return DefinitionValidation;
			}

			private readonly Regex doubleDot = new Regex("\\.\\.");
			//private readonly Regex FolderPathRegex =  new Regex("^([a-zA-Z0-9][^*/><?\"|:]*)$");
			//private readonly Regex FolderNameRegex = new Regex("[" + Regex.Escape(Path.GetInvalidPathChars) + "]");
			private readonly CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");

			public ValidationResult ValidateNameSpace()
			{
				if ( String.IsNullOrEmpty(NameSpace.Trim()) )
				{
					return new ValidationResult(false, "A Namespace must be provided.");
				}


				if (doubleDot.IsMatch(NameSpace))
				{
					return new ValidationResult(false, "NameSpace Structure is not valid.");
				}

				var inputs = (NameSpace as string).Split('.');
				foreach (var item in inputs)
				{
					if (!provider.IsValidIdentifier(item))
					{
						return new ValidationResult(false, string.Format("NameSpace sub element '{0}' is invalid.", item));
					}
				}
				return ValidationResult.ValidResult();
			}


			public ValidationResult ValidateName()
			{
				if ( String.IsNullOrEmpty(Name.Trim()))
				{
					return new ValidationResult(false, "An Enum name must be provided.");
				}
				if (!provider.IsValidIdentifier(Name))
				{
					return new ValidationResult(false,"Enum Name is invalid");
				}

				return ValidationResult.ValidResult();
			}



			public ValidationResult ValidateFolderPath()
			{
				// we accept not folder path, meaning it will be at the root of the assets.
				if (String.IsNullOrEmpty(FolderPath))
				{
					return ValidationResult.ValidResult();
				}

				//string outputPath = Path.Combine(Application.dataPath, FolderPath);
				/*
				if ( string.IsNullOrEmpty( Path.GetDirectoryName(FolderPath)) )
				{
					return new ValidationResult(false,"invalid path");
				}
			
				if ( Path.GetInvalidFileNameChars().Where(x => outputPath.Contains(x)).Count() > 0)
				{
					return new ValidationResult(false,"Contains invalid char");
				}
			
				try
				{
					outputPath = Path.GetFullPath(outputPath);
					//Debug.Log(outputPath);
				}catch(Exception e)
				{
				return new ValidationResult(false,e.Message);
				}


				var inputs = (FolderPath as string).Split('/');
				foreach (var item in inputs)
				{

					if (provider.IsValidIdentifier(item)) // (FolderNameRegex.IsMatch(item))
					{
						return new ValidationResult(false, string.Format("Folder '{0}' is invalid.", item));
					}

				}
			
*/
				return ValidationResult.ValidResult();
			}

			public ValidationResult ValidateEntries()
			{
				if (entries==null || entries.Count==0)
				{
					return new ValidationResult(false, "Enum must have at least one entry.");
				}

				EntryValidations = new Dictionary<string, ValidationResult>();

				int invalidEntries = 0;
				foreach(string item in entries)
				{

					ValidationResult _validation = ValidateEnumEntry(item);
					if (!_validation.success)
					{
						invalidEntries++;
					}
					if (item!=null)
					{
						EntryValidations[item] = _validation;
					}

				}

				if (invalidEntries>0)
				{
					if (invalidEntries==1)
					{
						return new ValidationResult(false, invalidEntries+" entry is invalid.");
					}else{
						return new ValidationResult(false, invalidEntries+" entries are invalid.");
					}

				}
				return ValidationResult.ValidResult();
			}

			public ValidationResult ValidateEnumEntry(string entry)
			{

				if (entry==null || String.IsNullOrEmpty(entry.Trim()))
				{
					return new ValidationResult(false, "Enum entry can not be null or empty");
				}

				if (!provider.IsValidIdentifier(entry))
				{
					return new ValidationResult(false,"Enum entry is invalid");
				}

				

				return ValidationResult.ValidResult();
			}


		}

		#endregion Validation
		static string Template_MainStructure =@"[HEADER]

[ENUM_STRUCTURE]";

		static string Template_Header = @"// (c) Copyright HutongGames, LLC 2010-[YEAR]. All rights reserved.
// THIS CONTENT IS AUTOMATICALLY GENERATED.
// [TAG]
// It is recommended to use PlayMaker Enum Creator Wizard to edit this enum definition. But you can perfectly edit this script directly";

		static string Template_EnumStructure = @"namespace [NAMESPACE]
{
	public enum [ENUM_NAME]
	{
[ENUM_ENTRIES]		
	}
}";
		
		static string Template_EnumEntry = "\t\t{0}{1}";
		

		/// <summary>
		/// Create a new script featuring the new enum.
		/// </summary>
		public void CreateEnum(EnumDefinition enumDefinition)
		{
			BuildScriptLiteral(enumDefinition);

			enumDefinition.UpdateFilePath();

			// Ensure that this path actually exists.
			if (!Directory.Exists(enumDefinition.directoryPath))
				Directory.CreateDirectory(enumDefinition.directoryPath);

			File.WriteAllText(enumDefinition.filePath, enumDefinition.ScriptLiteral);

			AssetDatabase.Refresh();
		}
		
		
		public void BuildScriptLiteral(EnumDefinition enumDefinition)
		{


			// build the header
			string headerLiteral = Template_Header;
			headerLiteral = headerLiteral.Replace("[YEAR]",DateTime.Today.Year.ToString());
			headerLiteral = headerLiteral.Replace("[TAG]","["+"PLAYMAKER_ENUM]"); // we recompose the tag to avoid detection of this very script

			// build the enum structure
			string EnumStructureLiteral = Template_EnumStructure;
			EnumStructureLiteral = EnumStructureLiteral.Replace("[NAMESPACE]",enumDefinition.NameSpace);
			EnumStructureLiteral = EnumStructureLiteral.Replace("[ENUM_NAME]",enumDefinition.Name);
			string _entriesLiteral = "";
			for(int i=0;i<enumDefinition.entries.Count;i++)
			{
				_entriesLiteral += string.Format(Template_EnumEntry,enumDefinition.entries[i],"");
				if (i+1<enumDefinition.entries.Count)
				{
					_entriesLiteral += ",\n";
				}
				
			}
			EnumStructureLiteral = EnumStructureLiteral.Replace("[ENUM_ENTRIES]",_entriesLiteral);

			// build script literal
			string scriptLiteral = Template_MainStructure;
			scriptLiteral = scriptLiteral.Replace("[HEADER]",headerLiteral);
			scriptLiteral = scriptLiteral.Replace("[ENUM_STRUCTURE]",EnumStructureLiteral);
			enumDefinition.ScriptLiteral = scriptLiteral;

			enumDefinition.EnumLiteralPreview = EnumStructureLiteral;
		}

	}
}

#if UNITY_EDITOR

#if (UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 ) 
#define UNITY_PRE_5_4
#endif


using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


using UnityEditor;


using PlayMaker.ConditionalExpression;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	
	//#pragma warning disable 618 
	
	public class LinkerData : ScriptableObject
	{
		
		static Type[] _unlinkedTyped = new Type[]{
			typeof(UnityEngine.Collider2D)
			#if UNITY_PRE_5_4
			,typeof(UnityEngine.ParticleEmitter)
			#endif
		};
		
		
		public bool debug = false;
		public bool LinkContentUpdateDone = false;
		public TextAsset Asset;
		public string AssetPath;
		
		public LinkerData self;
		
		public bool PlayModeFlag;
		
		// TODO: make it serializable...
		public Dictionary<string,List<string>> linkerEntries = new Dictionary<string, List<string>>();
		
		
		static public bool DebugAll
		{
			get{
				
				// don't spam if published
				if (!Application.isEditor && Application.isPlaying) return false;
				
				if (instance==null)
				{
					return false;
				}
				
				return instance.debug;
			}
		}
		
		static private LinkerData _instance_;
		
		
		static public LinkerData instance
		{
			get{
				if (_instance_==null)
				{
					return null;
				}
				return _instance_.self;
			}
			
			set{
				_instance_ = value;
			}
			
		}
		
		public static void RegisterClassDependancy(Type type,string typeName)
		{
			if (!Application.isEditor) return;
			
			if (type == null)
			{
				Debug.LogWarning("LinkerData RegisterClassDependancy with no type");
				return;
			}
			
			if (instance ==null)
			{
				Debug.LogWarning("LinkerData is missing an instance, please create one first in your assets from the create menu: Assets/Create/PlayMaker/Create Linker Wizard");
				return;
			}
			
			//Check for unlinked dependancy...
			foreach(Type _type in _unlinkedTyped)
			{
				
				if (type.IsSubclassOf(_type))
				{
					Debug.Log("Found subclass of "+_type);
					instance.RegisterLinkerEntry(_type.Namespace,_type.AssemblyQualifiedName);
				}
			}
			
			instance.RegisterLinkerEntry(type.Assembly.FullName,typeName);
		}
		
		/* removed because we need to check for subclass types
		public static void RegisterClassDependancy(string assemblyName,string typeName)
		{
			if (instance ==null)
			{
				Debug.LogWarning("LinkerData is missing an instance, please create one first in your assets from the create menu: Assets/Create/PlayMaker/Create Linker Wizard");
				return;
			}

			instance.RegisterLinkerEntry(assemblyName,typeName);
		}
*/
		
		public void RegisterLinkerEntry(string assemblyName,string typeName)
		{
			
			if (!Application.isEditor) return;
			
			
			if (instance ==null)
			{
				Debug.LogWarning("LinkerData is missing an instance, please create one first in your assets from the create menu: Assets/Create/PlayMaker/Create Linker Wizard");
				return;
			}
			
			if (string.IsNullOrEmpty(assemblyName))
			{
				Debug.LogError("LinkerEntry missing <color=blue>assemblyName</color>");
				return;
			}
			if (string.IsNullOrEmpty(typeName))
			{
				Debug.LogError("LinkerEntry missing <color=blue>typeName</color>");
				return;
			}
			
			// clean up assembly
			if (assemblyName.Contains(","))
			{
				assemblyName = assemblyName.Split(","[0])[0];
			}
			
			// clean up typeName
			if (typeName.Contains(","))
			{
				typeName = typeName.Split(","[0])[0];
			}
			
			if (!linkerEntries.ContainsKey(assemblyName))
			{
				linkerEntries.Add(assemblyName,new List<string>());
			}
			
			if (!linkerEntries[assemblyName].Contains(typeName))
			{
				linkerEntries[assemblyName].Add(typeName);
			}
		}
		
		
		void OnEnable() {
			LinkerData._instance_ = this;
			self = this;
			
			//EditorApplication.playmodeStateChanged = HandleOnPlayModeChanged;
		}
		
		/*
		void HandleOnPlayModeChanged()
		{
			// This method is run whenever the playmode state is changed.
			if (!EditorApplication.isPlaying)
			{
				PlayModeFlag = true;
				Debug.Log ("HandleOnPlayModeChanged");
			}
		}
*/
		
		public static void IncludeConditionalExpression()
		{
			LinkerData.RegisterClassDependancy(typeof(ConditionalExpression),typeof(ConditionalExpression).FullName);
		}
		
		
	}
}

#endif

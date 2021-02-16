// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using UnityEditor;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;


public class PlayMakerEditorUtils : Editor {
	
	#if UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
	[MenuItem ("PlayMaker/Addons/Tools/Export Current Scene",false,100)]
	public static void ExportCurrentScene()
	{

		if (!EditorApplication.SaveCurrentSceneIfUserWantsTo())
		{
			return;
		}
		
		EditorUtility.DisplayDialog("PlayMaker dll export","Just a reminder that PlayMaker.dll file is not redistributable,\n\nMake sure you uncheck: \n'Assets/PlayMaker/PlayMaker.dll'\n\nwhen exporting a package to sharing with others.","Ok");
		
		
		var _sel =  Selection.objects;
		
		
		if (EditorUtility.DisplayDialog("Export Globals?","If your scene if using global variables, it will need to be included in the package as well.","Export Globals","Ignore Globals"))
		{
			EditorApplication.ExecuteMenuItem("PlayMaker/Tools/Export Globals");
			var _globalAsset = AssetDatabase.LoadAssetAtPath("Assets/PlaymakerGlobals_EXPORTED.asset",typeof(UnityEngine.Object));
			ArrayUtility.Add<UnityEngine.Object>(ref _sel ,_globalAsset);
			Selection.objects = _sel;
		}
		
	
		SelectSceneCustomAction();
		
		
	
		var _scene = AssetDatabase.LoadAssetAtPath(EditorApplication.currentScene,typeof(UnityEngine.Object));
		_sel =  Selection.objects;
		ArrayUtility.Add<UnityEngine.Object>(ref _sel ,_scene);
		Selection.objects = _sel;
		EditorApplication.ExecuteMenuItem("Assets/Export Package...");
	}
	
	
	[MenuItem ("PlayMaker/Addons/Tools/Export Current Scene", true)]
	public static bool CheckExportCurrentScene() {
	    return !String.IsNullOrEmpty(EditorApplication.currentScene);
	}
	
	
	[MenuItem ("PlayMaker/Addons/Tools/Select Current Scene Used Custom Actions")]
	public static void SelectSceneCustomAction()
	{
		UnityEngine.Object[] _list = GetSceneCustomActionDependencies();
		
		var _sel =  Selection.objects;
		ArrayUtility.AddRange<UnityEngine.Object>(ref _sel ,_list);
		Selection.objects = _sel;	
	}

	#endif

	public static UnityEngine.Object[] GetSceneCustomActionDependencies()
	{
		
		UnityEngine.Object[] list = new UnityEngine.Object[0];
		
		FsmEditor.RebuildFsmList();

		List<PlayMakerFSM> fsms = FsmEditor.FsmComponentList;
		
//		List<System.Type> PlayMakerActions = FsmEditorUtility.Actionslist;

		foreach(PlayMakerFSM fsm in fsms)
		{
			
			//Debug.Log(FsmEditorUtility.GetFullFsmLabel(fsm));
			
			//if (fsm.FsmStates != null) fsm.FsmStates.Initialize();
			
			for (int s = 0; s<fsm.FsmStates.Length; ++s)
			{
				
					fsm.FsmStates[s].LoadActions();
				
					Debug.Log(fsm.FsmStates[s].Name+" is loaded:"+fsm.FsmStates[s].ActionsLoaded);
				
					// Show SendEvent and SendMessage as we find them
					foreach(FsmStateAction action in fsm.FsmStates[s].Actions)
					{
						#if PLAYMAKER_1_8_OR_NEWER
							UnityEngine.Object _asset = ActionScripts.GetAsset(action);
						#else
							UnityEngine.Object _asset = FsmEditorUtility.GetActionScriptAsset(action);
						#endif
						
						string _name = action.Name;
						if (String.IsNullOrEmpty(_name))
						{
							if (_asset!=null)
							{
								_name = _asset.name;
							}else
							{

								#if PLAYMAKER_1_8_OR_NEWER
									_name = Labels.GetActionLabel(action) + "[WARNING: FILE NOT FOUND]";
								#else
									_name = FsmEditorUtility.GetActionLabel(action) + "[WARNING: FILE NOT FOUND]";
								#endif

							}
							
						}
					
						if (Enum.IsDefined(typeof(WikiPages),_name))
						{
						//	Debug.Log(_name+" : official action");
						}else{
						//	Debug.Log(_name+" : custom action");
						
							if (_asset!=null)
							{
								ArrayUtility.Add<UnityEngine.Object>(ref list ,_asset);
							}
						}
							
					}
			}
			
			
		}
		
		return list;
		
	}// GetSceneCustomActionDependencies
	
	/// <summary>
	//	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static void CreateAsset<T> (string name="") where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
		
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") 
		{
			path = "Assets";
		} 
		else if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}

		string _name = string.IsNullOrEmpty(name)? "New " + typeof(T).ToString() : name ;

		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + _name + ".asset");
		
		AssetDatabase.CreateAsset (asset, assetPathAndName);
		
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
	}


	
	/// <summary>
	/// Used to get assets of a certain type and file extension from entire project
	/// </summary>
	/// <param name="type">The type to retrieve. eg typeof(GameObject).</param>
	/// <param name="fileExtension">The file extention the type uses eg ".prefab".</param>
	/// <returns>An Object array of assets.</returns>
	public static UnityEngine.Object[] GetAssetsOfType(System.Type type, string fileExtension)
	{
		List<UnityEngine.Object> tempObjects = new List<UnityEngine.Object>();
		DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
		FileInfo[] goFileInfo = directory.GetFiles("*" + fileExtension, SearchOption.AllDirectories);
		
		int i = 0; int goFileInfoLength = goFileInfo.Length;
		FileInfo tempGoFileInfo; string tempFilePath;
		UnityEngine.Object tempGO;
		for (; i < goFileInfoLength; i++)
		{
			tempGoFileInfo = goFileInfo[i];
			if (tempGoFileInfo == null)
				continue;
			
			tempFilePath = tempGoFileInfo.FullName;
			tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
			
			//Debug.Log(tempFilePath + "\n" + Application.dataPath);
			
			tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(UnityEngine.Object)) as UnityEngine.Object;
			if (tempGO == null)
			{
				//Debug.LogWarning("Skipping Null");
				continue;
			}
			else if (tempGO.GetType() != type)
			{
				//Debug.LogWarning("Skipping " + tempGO.GetType().ToString());
				continue;
			}
			
			tempObjects.Add(tempGO);
		}
		
		
		
		
		return tempObjects.ToArray();
	}
	
	public static UnityEngine.Object GetAssetByName(string fileName)
	{
		DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
		FileInfo[] goFileInfo = directory.GetFiles("*" + fileName, SearchOption.AllDirectories);
		
		int i = 0; int goFileInfoLength = goFileInfo.Length;
		FileInfo tempGoFileInfo; string tempFilePath;
		UnityEngine.Object tempGO;
		for (; i < goFileInfoLength; i++)
		{
			tempGoFileInfo = goFileInfo[i];
			if (tempGoFileInfo == null)
				continue;
			
			tempFilePath = tempGoFileInfo.FullName;
			tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
			
			//	Debug.Log(tempFilePath + "\n" + Application.dataPath);
			
			tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(UnityEngine.Object)) as UnityEngine.Object;
			if (tempGO == null)
			{
				Debug.LogWarning("Skipping Null");
				continue;
			}
			
			return tempGO;
		}
		
		return null;
	}


	/// <summary>
	/// Mounts the scripting define symbol to all targets. Taken from Hutonggames PlayMakerDefines.cs
	/// </summary>
	/// <param name="defineSymbol">Define symbol.</param>
	public static void MountScriptingDefineSymbolToAllTargets(string defineSymbol)
	{
		foreach (BuildTargetGroup group in Enum.GetValues(typeof(BuildTargetGroup)))
		{
			if (!IsValidBuildTargetGroup(group)) continue;
			
			var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';').Select(d => d.Trim()).ToList();
			if (!defineSymbols.Contains(defineSymbol))
			{
				defineSymbols.Add(defineSymbol);
				try
				{
					PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defineSymbols.ToArray()));
				}
				catch (Exception)
				{
					Debug.Log("Could not set PLAYMAKER defines for build target group: " + group);
					throw;
				}
				
			}
		}
	}

	/// <summary>
	/// Unmount scripting define symbol to all targets. Taken from Hutonggames PlayMakerDefines.cs
	/// </summary>
	/// <param name="defineSymbol">Define symbol.</param>
	public static void UnMountScriptingDefineSymbolToAllTargets(string defineSymbol)
	{
		foreach (BuildTargetGroup group in Enum.GetValues(typeof(BuildTargetGroup)))
		{
			if (!IsValidBuildTargetGroup(group)) continue;
			
			var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';').Select(d => d.Trim()).ToList();
			if (defineSymbols.Contains(defineSymbol))
			{
				defineSymbols.Remove(defineSymbol);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defineSymbols.ToArray()));
			}
		}
	}

	private static bool IsValidBuildTargetGroup(BuildTargetGroup group)
	{
		if (group == BuildTargetGroup.Unknown || IsObsolete(group)) return false;
		
		// Checking Obsolete attribute should be enough, 
		// but sometimes Unity versions are missing attributes
		// so keeping these checks around just in case:
		
		#if UNITY_5_3_0 // Unity 5.3.0 had tvOS in enum but throws error if used
		if ((int)(object)group == 25) return false;
		#endif
		
		#if UNITY_5_4 || UNITY_5_5 // Unity 5.4+ doesn't like Wp8 and Blackberry any more
		if ((int)(object)group == 15) return false;
		if ((int)(object)group == 16) return false;
		#endif
		
		#if UNITY_5_6 // Unity 5.6+ removed build target 27?
		if ((int)(object)group == 27) return false;
		#endif
		
		return true;
	}
	
	private static bool IsObsolete(Enum value)
	{
		var enumInt = (int)(object)value;
		if (enumInt == 4 || enumInt == 14) return false;
		
		var field = value.GetType().GetField(value.ToString());
		var attributes = (ObsoleteAttribute[])field.GetCustomAttributes(typeof(ObsoleteAttribute), false);
		return attributes.Length > 0;
	}
	
	/* NOTE: IsObsolete is complicated by the definition of BuildTargetGroup enum. 
         * E.g., in Unity 5.4:
         * 
          public enum BuildTargetGroup
          {
            Unknown = 0,
            Standalone = 1,
            [Obsolete("WebPlayer was removed in 5.4, consider using WebGL")] WebPlayer = 2,
            iOS = 4,
            [Obsolete("Use iOS instead (UnityUpgradable) -> iOS", true)] iPhone = 4,
            PS3 = 5,
            XBOX360 = 6,
            Android = 7,
            WebGL = 13,
            [Obsolete("Use WSA instead")] Metro = 14,
            WSA = 14,
            [Obsolete("Use WSA instead")] WP8 = 15,
            [Obsolete("BlackBerry has been removed as of 5.4")] BlackBerry = 16,
            Tizen = 17,
            PSP2 = 18,
            PS4 = 19,
            PSM = 20,
            XboxOne = 21,
            SamsungTV = 22,
            Nintendo3DS = 23,
            WiiU = 24,
            tvOS = 25,
          }
  
         */


	public static void CopyTextToClipboard(string content)
	{
		TextEditor te = new TextEditor();

		#if UNITY_5_3_OR_NEWER || UNITY_5_3
			te.text = content;
		#else
			te.content = new GUIContent(content);
		#endif

		te.SelectAll();
		te.Copy();
	}

	/// <summary>
	/// Add if necessary a list of Global events to the PlayMaker Globals resources to be available in the global events list
	/// </summary>
	/// <param name="events"></param>
	public static void CreateGlobalEventsIfNeeded(string[] events)
	{
		bool _isDirty = false;
		
		
		foreach (string _eventName in events)
		{
			if (!FsmEvent.IsEventGlobal(_eventName))
			{
				PlayMakerGlobals.AddGlobalEvent(_eventName);
				//	Debug.Log("Adding global event: "+_eventName);
				_isDirty = true;
			}
		}

		if (_isDirty)
		{
			FsmEditor.SaveGlobals();
			for (int index = 0; index < FsmEvent.globalEvents.Count; ++index)
				new FsmEvent(FsmEvent.globalEvents[index]).IsGlobal = true;


			FsmEventsWindow.ResetView();
		}
	}

	/// <summary>
	/// Add if necessary a Global event to the PlayMaker Globals resources to be available in the global events list
	/// </summary>
	/// <param name="eventName"></param>
	public static void CreateGlobalEventIfNeeded(string eventName)
	{
		CreateGlobalEventsIfNeeded(new[] {eventName});
	}
	
}

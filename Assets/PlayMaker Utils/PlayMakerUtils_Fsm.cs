// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

using HutongGames.PlayMaker;

public partial class PlayMakerUtils {


	public static Fsm GetFsmOnGameObject(GameObject go,string fsmName)
	{
		if (go==null || string.IsNullOrEmpty(fsmName))
		{
			return null;
		}
		
		foreach(PlayMakerFSM _fsm in go.GetComponents<PlayMakerFSM>())
		{
			if (string.Equals(_fsm.FsmName,fsmName))
			{
				return _fsm.Fsm;
			}
		}
		
		return null;
	}

	public static PlayMakerFSM FindFsmOnGameObject(GameObject go,string fsmName)
	{
		if (go==null || string.IsNullOrEmpty(fsmName))
		{
			return null;
		}

		foreach(PlayMakerFSM _fsm in go.GetComponents<PlayMakerFSM>())
		{
			if (string.Equals(_fsm.FsmName,fsmName))
			{
				return _fsm;
			}
		}

		return null;
	}


	public static string LogFullPathToAction(FsmStateAction action)
	{
		return GetGameObjectPath (action.Fsm.GameObject) + ":Fsm(" +
			action.Fsm.Name + "):State(" + action.State.Name + "):Action(" + action.GetType ().Name+")";
	}


	public static string GetGameObjectPath(GameObject obj)
	{
		string path = "/" + obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			path = "/" + obj.name + path;
		}
		return path;
	}
}

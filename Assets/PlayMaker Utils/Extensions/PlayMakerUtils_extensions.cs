// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.


using System;
using System.Collections;
using HutongGames.PlayMaker;

public static class PlayMakerUtils_Extensions
{
	
	#region ArrayList
	public static int IndexOf(ArrayList target,Object value) {
		return PlayMakerUtils_Extensions.IndexOf(target, value, 0, target.Count);
	}

    public static int IndexOf(ArrayList target,Object value, int startIndex) {
        if (startIndex > target.Count)
            throw new ArgumentOutOfRangeException("startIndex", "ArgumentOutOfRange_Index");

        return PlayMakerUtils_Extensions.IndexOf(target, value, startIndex, target.Count - startIndex);
    }

    public static int IndexOf(ArrayList target,Object value, int startIndex, int count) {

        if (startIndex<0 || startIndex >= target.Count)
            throw new ArgumentOutOfRangeException("startIndex", "ArgumentOutOfRange_Index");
        if (count <0 || startIndex > target.Count - count) throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_Count");
		
		
         if (target.Count == 0)
                    return -1;
		
		int endIndex = startIndex+count;
		
        if (value == null) {
            for(int i=startIndex; i < endIndex; i++)
                if (target[i] == null)
                    return i;
            return -1;
        } else {
            for(int i=startIndex; i < endIndex; i++)
                if (target[i] != null && target[i].Equals(value))
                    return i;
            return -1;
        }
		
    }

	public static int LastIndexOf(ArrayList target,Object value) {
         return PlayMakerUtils_Extensions.LastIndexOf(target,value,target.Count - 1, target.Count);
    }

    public static int LastIndexOf(ArrayList target,Object value, int startIndex) {
        return PlayMakerUtils_Extensions.LastIndexOf(target,value, startIndex, startIndex + 1);
    }
	
	public static int LastIndexOf(ArrayList target,Object value,int startIndex, int count) {
		
		ArrayList _list = target;

        if (_list.Count == 0)
                    return -1;
		
		if (startIndex<0 || startIndex >= target.Count)
            throw new ArgumentOutOfRangeException("startIndex", "ArgumentOutOfRange_Index");
		if (count < 0 || startIndex > target.Count - count) throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_Count");
		
		
        int endIndex = startIndex + count -1;
        if (value == null) {
            for(int i=endIndex; i >= startIndex; i--)
                if (_list[i] == null)
                    return i;
            return -1;
        } else {
              for(int i=endIndex; i >= startIndex; i--)
                if (_list[i] != null && _list[i].Equals(value))
                    return i;
            return -1;
        }
	}
	
	#endregion

	#region Transform

	public static string GetPath(this UnityEngine.Transform current) {
		if (current.parent == null)
			return "/" + current.name;
		return current.parent.GetPath() + "/" + current.name;
	}
	#endregion

	#region Component

	public static string GetPath(this UnityEngine.Component component) {
		return component.transform.GetPath();
	}

	#endregion
	
	#region FsmStateAction
	
	public static string GetActionPath(this FsmStateAction action) {
		
		return action.Fsm.GameObject.transform.GetPath()+ "/"+action.Fsm.Name+":"+action.State.Name+":"+action.Name;
	}

	#endregion

}


// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

using HutongGames.PlayMaker;

public partial class PlayMakerUtils {

	/// <summary>
	/// Self generated Fsm in case developer passes null as a source Fsm to fire an event.
	/// </summary>
	public static PlayMakerFSM FsmEventSender;

	public static PlayMakerFSM GetFsmEventSender()
	{
		if (FsmEventSender==null)
		{
			FsmEventSender = new GameObject("PlayMaker Send Event Proxy").AddComponent<PlayMakerFSM>();
			//FsmEventSender.hideFlags = HideFlags.HideAndDontSave; // not too sure if I should hide it or not.. cause we can't define the event sender yet
			FsmEventSender.FsmName = "Send Event Proxy";
			FsmEventSender.FsmDescription = "This Fsm was created at runtime, because a script or component is willing to send a PlayMaker event";
		}
		return FsmEventSender;
	}


	public static void SendEventToTarget(PlayMakerFSM fromFsm,FsmEventTarget target,string fsmEvent,FsmEventData eventData)
	{
		if (fromFsm==null)
		{
			fromFsm = GetFsmEventSender();
		}
		
		if (eventData!=null)
		{
		    Fsm.EventData = eventData;
		}
		
		if (fromFsm == null)
		{
			return;
		}

		fromFsm.Fsm.Event(target,fsmEvent);
	}

	public static void SendEventToGameObject(PlayMakerFSM fromFsm,GameObject target,string fsmEvent,bool includeChildren)
	{
		SendEventToGameObject(fromFsm,target,fsmEvent,includeChildren,null);
	}
	
	public static void SendEventToGameObject(PlayMakerFSM fromFsm,GameObject target,string fsmEvent)
	{
		SendEventToGameObject(fromFsm,target,fsmEvent,false,null);
	}
	
	public static void SendEventToGameObject(PlayMakerFSM fromFsm,GameObject target,string fsmEvent,FsmEventData eventData)
	{
		SendEventToGameObject(fromFsm,target,fsmEvent,false,eventData);
	}
	
	public static void SendEventToGameObject(PlayMakerFSM fromFsm,GameObject target,string fsmEvent,bool includeChildren,FsmEventData eventData)
	{
		if (fromFsm==null)
		{
			fromFsm = GetFsmEventSender();
		}

		if (eventData!=null)
		{
			HutongGames.PlayMaker.Fsm.EventData = eventData;
		}
		
		if (fromFsm == null)
		{
			return;
		}
		
		FsmEventTarget _eventTarget = new FsmEventTarget();
		_eventTarget.excludeSelf = false;
		_eventTarget.sendToChildren = includeChildren;

		_eventTarget.target = FsmEventTarget.EventTarget.GameObject;	

		FsmOwnerDefault owner = new FsmOwnerDefault();
		owner.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
		owner.GameObject = new FsmGameObject();
		owner.GameObject.Value = target;

		_eventTarget.gameObject = owner;

		fromFsm.Fsm.Event(_eventTarget,fsmEvent);

	}

	
	public static bool DoesTargetImplementsEvent(FsmEventTarget target,string eventName)
	{
		
		if (target.target == FsmEventTarget.EventTarget.BroadcastAll)
		{
			return FsmEvent.IsEventGlobal(eventName);
		}
		
		if (target.target == FsmEventTarget.EventTarget.FSMComponent)
		{
			return DoesFsmImplementsEvent(target.fsmComponent,eventName);
		}
		
		if (target.target == FsmEventTarget.EventTarget.GameObject)
		{
			return DoesGameObjectImplementsEvent(target.gameObject.GameObject.Value,eventName);
		}
		
		if (target.target == FsmEventTarget.EventTarget.GameObjectFSM)
		{
			return DoesGameObjectImplementsEvent(target.gameObject.GameObject.Value,target.fsmName.Value, eventName);
		}
		
		if (target.target == FsmEventTarget.EventTarget.Self)
		{
			Debug.LogError("Self target not supported yet");
		}
		
		if (target.target == FsmEventTarget.EventTarget.SubFSMs)
		{
			Debug.LogError("subFsms target not supported yet");
		}
		
		if (target.target == FsmEventTarget.EventTarget.HostFSM)
		{
			Debug.LogError("HostFSM target not supported yet");
		}
		
		return false;
	}
	
	public static bool DoesGameObjectImplementsEvent(GameObject go, string fsmEvent,bool includeChildren = false)
	{
		if (go==null || string.IsNullOrEmpty(fsmEvent))
		{
			return false;
		}

		if (includeChildren)
		{
			foreach(PlayMakerFSM _fsm in go.GetComponentsInChildren<PlayMakerFSM>())
			{
				if (DoesFsmImplementsEvent(_fsm,fsmEvent))
				{
					return true;
				}
			}
		}else{

			foreach(PlayMakerFSM _fsm in go.GetComponents<PlayMakerFSM>())
			{
				if (DoesFsmImplementsEvent(_fsm,fsmEvent))
				{
					return true;
				}
			}
		}
		return false;
	}
	
	public static bool DoesGameObjectImplementsEvent(GameObject go,string fsmName, string fsmEvent)
	{
		if (go==null || string.IsNullOrEmpty(fsmEvent))
		{
			return false;
		}
		
		bool checkFsmName = !string.IsNullOrEmpty(fsmName);
		
		foreach(PlayMakerFSM _fsm in go.GetComponents<PlayMakerFSM>())
		{
			if ( checkFsmName &&  string.Equals(_fsm,fsmName) )
			{
				if (DoesFsmImplementsEvent(_fsm,fsmEvent))
				{
					return true;
				}
			}
		}
		return false;
	}
	
	public static bool DoesFsmImplementsEvent(PlayMakerFSM fsm, string fsmEvent)
	{
		
		if (fsm==null || string.IsNullOrEmpty(fsmEvent))
		{
			return false;
		}
		
		foreach(FsmTransition _transition in fsm.FsmGlobalTransitions)
		{
			if (_transition.EventName.Equals(fsmEvent))
			{
				return true;
			}
		}
		
		foreach(FsmState _state in fsm.FsmStates)
		{
			
			foreach(FsmTransition _transition in _state.Transitions)
			{
				
				if (_transition.EventName.Equals(fsmEvent))
				{
					return true;
				}
			}
		}
		
		return false;
	}

    public static bool CreateIfNeededGlobalEvent(string globalEventName)
    {
        FsmEvent _event = FsmEvent.GetFsmEvent(globalEventName);

        bool result = false;

        if (!FsmEvent.IsEventGlobal(globalEventName))
        {
            if (_event == null)
            {
                _event = new FsmEvent(globalEventName) { IsGlobal = true };
                FsmEvent.AddFsmEvent(_event);
                if (!FsmEvent.globalEvents.Contains(globalEventName))
                {
                    FsmEvent.globalEvents.Add(globalEventName);
                }
            }

            result = true;

        }

        return result;
    }


	/*
	public bool DoesTargetMissEventImplementation(PlayMakerFSM fsm, string fsmEvent)
	{
		if (DoesTargetImplementsEvent(fsm,fsmEvent))
		{
			return false;
		}
		
		foreach(FsmEvent _event in fsm.FsmEvents)
		{
			if (_event.Name.Equals(fsmEvent))
			{
				return true;
			}
		}
		
		return false;
	}*/

}

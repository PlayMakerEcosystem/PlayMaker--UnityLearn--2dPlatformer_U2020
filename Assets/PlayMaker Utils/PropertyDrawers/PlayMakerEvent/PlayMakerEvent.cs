// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using System;
using UnityEngine;
using System.Text.RegularExpressions;

using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{

	/// <summary>
	/// PlayMaker Event. Use this class in your Components public interface. The Unity Inspector will use the related PropertyDrawer.
	/// It lets user easily choose a PlayMaker Event
	/// 
	/// If there is no attribute "EventTargetVariable" define, the list of events will be all the PlayMaker global events
	/// 
	/// If the attribute "EventTargetVariable" is defined, the PlayMakerEventTarget variable will be used for the context
	///  the list of events will adapt, and warn the user if the selected event is indeed implemented on the target
	/// </summary>
	[Serializable]
	public class PlayMakerEvent{

		/// <summary>
		/// The name of the event.
		/// </summary>
		public string eventName;

		/// <summary>
		/// Store here a user setting, instead of in the PropertyDrawer
		/// Switch between showing global or local events to keep it as choosen by the user.
		/// </summary>
		public bool allowLocalEvents;

		/// <summary>
		/// The default name of the event.
		/// </summary>
		public string defaultEventName;

		/// <summary>
		/// true is no event was selected.
		/// </summary>
		/// <value><c>true</c> if no event selected; otherwise, <c>false</c>.</value>
		public bool isNone {
			get {
				return string.IsNullOrEmpty (eventName);
			}
		}


		public PlayMakerEvent(){}

		public PlayMakerEvent(string defaultEventName)
		{
			this.defaultEventName = defaultEventName;
			this.eventName = defaultEventName;
		}

		public PlayMakerFSM SanitizeFsmEventSender(PlayMakerFSM fsm)
		{
			if (fsm==null)
			{
				if (PlayMakerUtils.FsmEventSender==null)
				{
					PlayMakerUtils.FsmEventSender = new GameObject("PlayMaker Send Event Proxy").AddComponent<PlayMakerFSM>();
					PlayMakerUtils.FsmEventSender.FsmName = "Send Event Proxy";
					PlayMakerUtils.FsmEventSender.FsmDescription = "This Fsm was created at runtime, because a script or component is willing to send a PlayMaker event";
				}

				return PlayMakerUtils.FsmEventSender;
			}

			return fsm;
		}
		#if UNITY_2017
		public bool SendEvent(PlayMakerFSM fromFsm,PlayMakerTimelineEventTarget eventTarget,bool debug = false)
		{
			fromFsm = SanitizeFsmEventSender(fromFsm);

			if (debug) Debug.Log("Sending event <"+eventName+"> from fsm:"+fromFsm.FsmName+" "+eventTarget.eventTarget+" "+eventTarget.gameObject+" "+eventTarget.fsmComponent);

			if (eventTarget.eventTarget == ProxyEventTarget.BroadCastAll)
			{
				PlayMakerFSM.BroadcastEvent(eventName);
			}else if (eventTarget.eventTarget == ProxyEventTarget.Owner || eventTarget.eventTarget == ProxyEventTarget.GameObject)
			{
				PlayMakerUtils.SendEventToGameObject(fromFsm,eventTarget.gameObject,eventName,eventTarget.includeChildren);
			}else if (eventTarget.eventTarget == ProxyEventTarget.FsmComponent)
			{
				eventTarget.fsmComponent.SendEvent(eventName);
			}

			return true;
		}
		#endif

		public bool SendEvent(PlayMakerFSM fromFsm,PlayMakerEventTarget eventTarget,bool debug = false)
		{
			fromFsm = SanitizeFsmEventSender(fromFsm);

			if (debug) Debug.Log("Sending event <"+eventName+"> from fsm:"+fromFsm.FsmName+" "+eventTarget.eventTarget+" "+eventTarget.gameObject+" "+eventTarget.fsmComponent);

			if (eventTarget.eventTarget == ProxyEventTarget.BroadCastAll)
			{
				PlayMakerFSM.BroadcastEvent(eventName);
			}else if (eventTarget.eventTarget == ProxyEventTarget.Owner || eventTarget.eventTarget == ProxyEventTarget.GameObject)
			{
				PlayMakerUtils.SendEventToGameObject(fromFsm,eventTarget.gameObject,eventName,eventTarget.includeChildren);
			}else if (eventTarget.eventTarget == ProxyEventTarget.FsmComponent)
			{
				eventTarget.fsmComponent.SendEvent(eventName);
			}

			return true;
		}

		public override string ToString ()
		{

			string _eventName = "<color=blue>"+eventName+"</color>";
			if (this.isNone)
			{
				_eventName = "<color=red>None</color>";
			}
			return string.Format ("PlayMaker Event : {0}", _eventName);
		}
	}

}
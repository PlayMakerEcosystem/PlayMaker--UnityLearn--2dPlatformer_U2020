// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

#if UNITY_2017
using System;
using UnityEngine;
using System.Text.RegularExpressions;

using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{

	/// <summary>
	/// PlayMaker Timeline Event Target. Use this class in your Components public interface that you want to drop on Timeline. The Unity Inspector will use the related PropertyDrawer.
	/// It lets user easily choose a PlayMaker Event Target: 
	/// Options are: Owner, GameObject, BroadcastAll, or FsmComponent
	/// For Owner and GameObject targets, the user can choose to include children, 
	/// in which case, the PlayMaker event will be send to all childrens
	/// 
	/// This class works on its own. However, it's meant to be used in conjunction with the PlayMakerEvent Class which will point to the variable of that class via the attribute "EventTargetVariable"
	/// So the PlayMakerEvent will then be able to send a PlayMakerEvent to the target defined by this class.
	/// </summary>
	[Serializable]
	public class PlayMakerTimelineEventTarget{

		public ProxyEventTarget eventTarget;
		public GameObject gameObject;
		public ExposedReference<GameObject> GameObject;
		public bool includeChildren = true;
		public PlayMakerFSM fsmComponent;
		public ExposedReference<PlayMakerFSM> FsmComponent;

		public PlayMakerTimelineEventTarget(){}

		public PlayMakerTimelineEventTarget(bool includeChildren = true)
		{
			this.includeChildren = includeChildren;
		}
		public PlayMakerTimelineEventTarget(ProxyEventTarget evenTarget,bool includeChildren = true)
		{
			this.eventTarget = evenTarget;
			this.includeChildren = includeChildren;
		}

		/// <summary>
		/// Use this to force the owner of this Target, in odd situations ( timeline playable Assets), the owner is not known from the SerializedObject.
		/// </summary>
		public void SetOwner(GameObject owner)
		{

			if (this.eventTarget == ProxyEventTarget.Owner)
			{
				this.gameObject = owner;
			}

		}


		/// <summary>
		/// Resolved References
		/// </summary>
		public void Resolve(IExposedPropertyTable resolver)
		{
			
			if (this.eventTarget == ProxyEventTarget.GameObject)
			{
				this.gameObject = GameObject.Resolve(resolver);
			}else if (this.eventTarget == ProxyEventTarget.FsmComponent)
			{
				this.fsmComponent = FsmComponent.Resolve(resolver);
			}

		}

		public override string ToString ()
		{

			string _message = eventTarget.ToString();

			if (this.gameObject == null)
			{
				_message += " : <color=red>unresolved GameObject</color>";
			}

			if (this.gameObject!=null && eventTarget== ProxyEventTarget.GameObject) 
			{
				_message += " : "+this.gameObject.name;
			}

			if (eventTarget == ProxyEventTarget.GameObject || eventTarget == ProxyEventTarget.Owner)
			{
				_message += includeChildren?", ":", not ";
				_message += "including children";
			}

			return _message;

		}
	}

}
#endif
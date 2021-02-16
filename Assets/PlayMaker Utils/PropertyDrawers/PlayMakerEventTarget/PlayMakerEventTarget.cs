﻿// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using System;
using UnityEngine;
using System.Text.RegularExpressions;

using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{

	/// <summary>
	/// Options to define an event target
	/// </summary>
	public enum ProxyEventTarget {Owner,GameObject,BroadCastAll,FsmComponent};


	/// <summary>
	/// PlayMaker Event Target. Use this class in your Components public interface. The Unity Inspector will use the related PropertyDrawer.
	/// It lets user easily choose a PlayMaker Event Target: 
	/// Options are: Owner, GameObject, BroadcastAll, or FsmComponent
	/// For Owner and GameObject targets, the user can choose to include children, 
	/// in which case, the PlayMaker event will be send to all childrens
	/// 
	/// This class works on its own. However, it's meant to be used in conjunction with the PlayMakerEvent Class which will point to the variable of that class via the attribute "EventTargetVariable"
	/// So the PlayMakerEvent will then be able to send a PlayMakerEvent to the target defined by this class.
	/// </summary>
	[Serializable]
	public class PlayMakerEventTarget{

		public ProxyEventTarget eventTarget;
		public GameObject gameObject;
		public bool includeChildren = true;
		public PlayMakerFSM fsmComponent;


		public PlayMakerEventTarget(){}

		public PlayMakerEventTarget(bool includeChildren = true)
		{
			this.includeChildren = includeChildren;
		}
		public PlayMakerEventTarget(ProxyEventTarget evenTarget,bool includeChildren = true)
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

		public override string ToString ()
		{

			string _message = eventTarget.ToString();
			if (eventTarget== ProxyEventTarget.GameObject) 
			{
				_message += " : "+gameObject.name;
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
// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __PROXY__ ---*/

#if UNITY_5_3_OR_NEWER

using UnityEngine;
using System.Collections;
using HutongGames;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{

	/// <summary>
	/// Transform events bridge. catches OnTransformParentChanged() and OnTransformChildrenChanged() and sends a PlayMaker event.
	/// </summary>
	public class TransformEventsBridge : MonoBehaviour {

		public PlayMakerEventTarget eventTarget = new PlayMakerEventTarget(false);

		[EventTargetVariable("eventTarget")]
		public PlayMakerEvent parentChangedEvent;

		[EventTargetVariable("eventTarget")]
		public PlayMakerEvent childrenChangedEvent;

		public bool debug;

		void OnTransformParentChanged()
		{
			if (debug)
			{
				UnityEngine.Debug.Log("OnTransformParentChanged(): Send "+parentChangedEvent.ToString()+" on "+eventTarget.ToString(),this);
			}

			parentChangedEvent.SendEvent(null,eventTarget);
		}

		void OnTransformChildrenChanged()
		{
			if (debug)
			{
				UnityEngine.Debug.Log("OnTransformChildrenChanged(): Send "+childrenChangedEvent.ToString()+" on "+eventTarget.ToString(),this);
			}

			childrenChangedEvent.SendEvent(null,eventTarget);
		}
	}
}
#endif
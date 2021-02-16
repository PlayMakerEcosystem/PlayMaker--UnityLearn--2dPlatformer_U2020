// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__
EcoMetaStart
{
"script dependancies":[
						"Assets/PlayMaker Custom Actions/__Internal/PlayMakerActionsUtils.cs"
					]
}
EcoMetaEnd
---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Linearly interpolates between 2 vectors.\n Advanced features allows selection of update type.")]
	public class Vector3LerpAdvanced : FsmStateAction
	{
		[RequiredField]
		[Tooltip("First Vector.")]
		public FsmVector3 fromVector;
		
		[RequiredField]
		[Tooltip("Second Vector.")]
		public FsmVector3 toVector;
		
		[RequiredField]
		[Tooltip("Interpolate between From Vector and ToVector by this amount. Value is clamped to 0-1 range. 0 = From Vector; 1 = To Vector; 0.5 = half way between.")]
		public FsmFloat amount;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in this vector variable.")]
		public FsmVector3 storeResult;
		
		public PlayMakerActionsUtils.EveryFrameUpdateSelector updateType;
		
		[Tooltip("Repeat every frame. Useful if any of the values are changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			fromVector = new FsmVector3 { UseVariable = true };
			toVector = new FsmVector3 { UseVariable = true };
			storeResult = null;
			updateType = PlayMakerActionsUtils.EveryFrameUpdateSelector.OnUpdate;
			everyFrame = true;
		}
		
		
		public override void OnUpdate()
		{
			if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnUpdate)
			{
				DoVector3Lerp();
			}
			
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnLateUpdate()
		{
			if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnLateUpdate)
			{
				DoVector3Lerp();
			}

			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnFixedUpdate()
		{
			if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnFixedUpdate)
			{
				DoVector3Lerp();
			}

			if (!everyFrame)
			{
				Finish();
			}
		}

		void DoVector3Lerp()
		{
			storeResult.Value = Vector3.Lerp(fromVector.Value, toVector.Value, amount.Value);
		}
	}
}


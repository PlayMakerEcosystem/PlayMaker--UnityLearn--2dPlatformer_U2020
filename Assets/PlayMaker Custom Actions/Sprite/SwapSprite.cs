﻿// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
//--- __ECO__ __ACTION__

using UnityEngine;
using System.Collections;


namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("sprite")]
	[Tooltip("Replaces a single Sprite on any GameObject. Object must have a Sprite Renderer.")]
	public class SwapSprite : FsmStateAction
	{
		
		[RequiredField]
		[CheckForComponent(typeof(SpriteRenderer))]
		public FsmOwnerDefault gameObject;
		
		public Sprite spriteToSwap;

		public FsmBool resetOnExit;

		private SpriteRenderer spriteRenderer;

		Sprite _orig;

		public override void Reset()
		{
			gameObject = null;
			resetOnExit = false;

            spriteToSwap = null;
		}
		
		public override void OnEnter()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
			
			spriteRenderer = go.GetComponent<SpriteRenderer>();
			
			if (spriteRenderer == null)
			{
				LogError("SwapSingleSprite: Missing SpriteRenderer!");
				return;
			}

			_orig = spriteRenderer.sprite;

			SwapSprites();
			
			Finish();
		}

		public override void OnExit()
		{
			if (resetOnExit.Value)
			{
				spriteRenderer.sprite = _orig;
			}
		}
		void SwapSprites()
		{
			spriteRenderer.sprite = spriteToSwap;
		}


		
	}
}
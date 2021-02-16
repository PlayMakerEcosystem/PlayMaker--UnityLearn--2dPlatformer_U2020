// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.
using UnityEngine;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	public partial class PlayMakerInspectorUtils {
		// This is the main file to keep old package happy. Each section of PlayMaker Utils are now in distinct files with partial implementation.

		static string FOCUS_OUT_UID = "some weird UId to make sure it doesn't clash :) ";


		/// <summary>
		/// Removes the UI focus from the current Control 
		/// 
		/// https://forum.unity.com/threads/gui-text-field-remove-focus.41845/#post-786990
		/// 
		/// </summary>
		public static void RemoveFocus()
		{
			GUI.SetNextControlName(FOCUS_OUT_UID);
			GUI.Label(new Rect(-100, -100, 1, 1), "");
			GUI.FocusControl(FOCUS_OUT_UID);
		}


	}
}
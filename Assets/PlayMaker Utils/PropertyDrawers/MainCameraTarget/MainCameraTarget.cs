// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.

using System;
using UnityEngine;
using System.Text.RegularExpressions;

using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	public enum CameraSelectionOptions {MainCamera,Owner,SpecifyGameObject};

	/// <summary>
	/// Defines a Camera GameObject target. Can be the mainCamera, the owner of the component or a specific GameObject.
	/// Use this class in your Components public interface, The Unity Inspector will use a specific PropertyDrawer is defined
	/// </summary>
	[Serializable]
	public class MainCameraTarget{

		public CameraSelectionOptions selection;

		[SerializeField]
		private GameObject _gameObject;

		public GameObject gameObject
		{
			set{
				if (selection == CameraSelectionOptions.MainCamera && value != Camera.main )
				{
					UnityEngine.Debug.LogError ("MainCameraTarget: you are trying to assign a GameObject that is not the MainCamera");
					return;
				}
				_gameObject = value;
			}
			get{
				if (selection == CameraSelectionOptions.MainCamera)
				{
					return Camera.main.gameObject;
				} else
				{
					return _gameObject;
				}

			}


		}

		public Component component;
		public string expectedComponentType;

		public MainCameraTarget(){}

	}

}
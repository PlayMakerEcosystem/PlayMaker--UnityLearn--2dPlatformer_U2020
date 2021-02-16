// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using System;
using UnityEngine;
using UnityEditor;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	[CustomPropertyDrawer(typeof(Owner))]
	public class OwnerDrawer : PlayMakerPropertyDrawerBaseClass 
	{

		SerializedProperty selection;
		SerializedProperty gameObject;
		SerializedProperty expectedComponentType;
		/// <summary>
		/// Flag to know that we have checked for the attributes
		/// </summary>
		bool attributeScanned;

		/// <summary>
		/// The row count. Computed and set by inheriting class
		/// </summary>
		int rowCount;

		GUIStyle controlLabelStyle; // for richText check

		Type expectedType;

		public override float GetPropertyHeight (SerializedProperty prop, GUIContent label)
		{
			

			gameObject = prop.FindPropertyRelative("gameObject");
			expectedComponentType = prop.FindPropertyRelative("expectedComponentType");

			rowCount = 1;

			if (!attributeScanned)
			{
				attributeScanned = true;

				// check for ExpectComponent Attribute
				object[] _expectComponentsAtts = this.fieldInfo.GetCustomAttributes(typeof(ExpectComponent),true);

				if (_expectComponentsAtts.Length>0)
				{
					expectedType = (_expectComponentsAtts[0] as ExpectComponent).expectedComponentType;

				}

			}
		

			CacheOwnerGameObject(prop.serializedObject);

		
			selection = prop.FindPropertyRelative("selection");
			if (selection.enumValueIndex == 1)
			{
				rowCount ++;
			}

			if (expectedType != null)
			{
				expectedComponentType = prop.FindPropertyRelative("expectedComponentType");
				expectedComponentType.stringValue = ExpectComponent.GetTypeAsString (expectedType);
			}


			if (selection.enumValueIndex == 1)
			{
				if (gameObject.objectReferenceValue != null)
				{
					GameObject _go = gameObject.objectReferenceValue as GameObject;

					if (_go != null && expectedType != null && !_go.GetComponent (expectedType))
					{
						rowCount++;
					}

				}
			} else
			{
				if (ownerGameObject != null && expectedType != null && !ownerGameObject.GetComponent (expectedType))
				{
					rowCount++;
				}
			}

			return base.GetPropertyHeight(prop,label) * (rowCount);
		}


		public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) 
		{
			// set style to use rich text.
			if (controlLabelStyle==null)
			{
				controlLabelStyle = GUI.skin.GetStyle("ControlLabel");
				controlLabelStyle.richText = true;
			}


			selection = prop.FindPropertyRelative("selection");
			gameObject = prop.FindPropertyRelative("gameObject");
			expectedComponentType = prop.FindPropertyRelative("expectedComponentType");

			int row =0; 

			if (!attributeScanned)
			{
				attributeScanned = true;

				// check for ExpectComponent Attribute
				object[] _expectComponentsAtts = this.fieldInfo.GetCustomAttributes(typeof(ExpectComponent),true);

				if (_expectComponentsAtts.Length>0)
				{
					expectedType = (_expectComponentsAtts[0] as ExpectComponent).expectedComponentType;

				}
			}

			CacheOwnerGameObject(prop.serializedObject);


			// draw the enum popup Field
			int oldEnumIndex = selection.enumValueIndex;

			EditorGUI.PropertyField(
				GetRectforRow(pos,row++),
				selection,new GUIContent("Target"),true);

			if (oldEnumIndex !=selection.enumValueIndex)
			{
				gameObject.objectReferenceValue = ownerGameObject;
			}

			if (selection.enumValueIndex == 1)
			{
				EditorGUI.indentLevel++;

				EditorGUI.PropertyField (
					GetRectforRow (pos, row++),
					gameObject, new GUIContent ("Game Object"), true);

				EditorGUI.indentLevel--;
			}


			if (expectedType != null)
			{
				expectedComponentType = prop.FindPropertyRelative("expectedComponentType");
				expectedComponentType.stringValue = ExpectComponent.GetTypeAsString (expectedType);
			}

			if (selection.enumValueIndex == 1)
			{
				if (gameObject.objectReferenceValue != null)
				{
					GameObject _go = gameObject.objectReferenceValue as GameObject;

					if (_go != null && expectedType != null && !_go.GetComponent (expectedType))
					{
						EditorGUI.indentLevel++;
						EditorGUI.LabelField (
							GetRectforRow (pos, ++row - 1),
							"<color=red>missing Component</color>",
							"<color=red>" + expectedType + "</color>"
						);

					}

				}

			}else{

				if (ownerGameObject != null && expectedType != null && !ownerGameObject.GetComponent (expectedType))
				{
					EditorGUI.indentLevel++;
					EditorGUI.LabelField (
						GetRectforRow (pos, ++row - 1),
						"<color=red>missing Component</color>",
						"<color=red>" + expectedType + "</color>"
					);
				}
		
			}


		}

	}
}
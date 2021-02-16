// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.

using UnityEngine;

using HutongGames.PlayMaker;

public partial class PlayMakerUtils {


	/// <summary>
	/// Converts to quaternion. if object is null or not of the right type, will return default value
	/// </summary>
	/// <returns>The to quaternion.</returns>
	/// <param name="value">Value.</param>
	/// <param name="defaultValue">Default value.</param>
	public static Quaternion ConvertToQuaternion(System.Object value,Quaternion defaultValue = new Quaternion())
	{
		if (value is Quaternion)
		{
			return (Quaternion)value;
		}
		return defaultValue;
	}

	/// <summary>
	/// Converts to rect. if object is null or not of the right type, will return default value
	/// </summary>
	/// <returns>The to rect.</returns>
	/// <param name="value">Value.</param>
	/// <param name="defaultValue">Default value.</param>
	public static Rect ConvertToRect(System.Object value,Rect defaultValue = new Rect())
	{
		if (value is Rect)
		{
			return (Rect)value;
		}
		return  defaultValue;
	}
	
	/// <summary>
	/// Converts to color. if object is null or not of the right type, will return default value
	/// </summary>
	/// <returns>The to color.</returns>
	/// <param name="value">Value.</param>
	/// <param name="defaultValue">Default value.</param>
	public static Color ConvertToColor(System.Object value,Color defaultValue = new Color() )
	{
		if (value is Color)
		{
			return (Color)value;
		}
		return defaultValue;
	}

	/// <summary>
	/// Converts to vector3. if object is null or not of the right type, will return default value
	/// </summary>
	/// <returns>The to vector3.</returns>
	/// <param name="value">Value.</param>
	public static Vector3 ConvertToVector3(System.Object value,Vector3 defaultValue = new Vector3())
	{
		if (value is Vector3)
		{
			return (Vector3)value;
		}
		return defaultValue;
	}

	/// <summary>
	/// Converts to vector2. if object is null or not of the right type, will return default value
	/// </summary>
	/// <returns>The to vector2.</returns>
	public static Vector2 ConvertToVector2(System.Object value,Vector2 defaultValue = new Vector2())
	{
		if (value is Vector2)
		{
			return (Vector2)value;
		}
		return defaultValue;
	}

	/// <summary>
	/// Converts to vector2. if object is null or not of the right type, will return default value
	/// </summary>
	/// <returns>The to vector2.</returns>
	/// <param name="value">Value.</param>
	/// <param name="defaultValue">Default value.</param>
	public static Vector4 ConvertToVector2(System.Object value,Vector4 defaultValue = new Vector4())
	{
		if (value is Vector4)
		{
			return (Vector4)value;
		}
		return defaultValue;
	}
}

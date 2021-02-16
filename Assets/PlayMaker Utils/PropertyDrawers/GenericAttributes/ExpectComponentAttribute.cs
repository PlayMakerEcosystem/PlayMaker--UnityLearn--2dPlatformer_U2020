using System;
using UnityEngine;


namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	/// <summary>
	/// Expect a component, use this for a public class to make sure it targets a gameobject that has this component, unlike RequireComponent, it doesn't add the component is missing,
	/// Use this with the Owner class for example
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class ExpectComponent : Attribute
	{
		
		public readonly Type expectedComponentType;

		public ExpectComponent(Type type)
		{
			expectedComponentType = type;
		}
			
		/// <summary>
		///     Converts a string to a Type.
		/// </summary>
		/// <returns>The Type.</returns>
		/// <param name="typeString">Type string.</param>
		public static Type GetTypeFromString(string typeString)
		{
			return Type.GetType(typeString);
		}

		/// <summary>
		///     Converts a Type to a string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="type">Type.</param>
		public static string GetTypeAsString(Type type)
		{
			return type.AssemblyQualifiedName;
		}
	}
}
// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

// INSTRUCTIONS
// This set of utils is here to help custom action development, and scripts in general that wants to connect and work with PlayMaker API.
// noticeable, the FsmVar playmaker class needs some extra steps to become convenient to use and to avoid boiler plate ducplication, I defined several helper functions to process FsmVar
// the possible benefit from these wrappers is that I will be able to create bridges from different variable types, like if the user selected an int, but the output is a float, Iwill be able to detect this, same with string
// etc etc. We will be able to parse values across variable types this way. 
// 
// RefreshValueFromFsmVar: Use this to inject the value of the variable selected back into the FsmVar itself. FsmVar do not maintain value binding if the variable selected changes
//
// GetValueFromFsmVar: wrapper to get the proper value of the FsmVar. It overcomes several issues, like data binding if FsmVAr points to a variable, AND bool value that is erronous in some cases
//
// ApplyValueToFsmVar: wrapper that inject back a value to the variable selected for that FsmVar. Only works if the FsmVar points to a Fsm Variable.


using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

using HutongGames.PlayMaker;

public partial class PlayMakerUtils {
	
	// ONLY WORKS IF THE FSMVAR POINTS TO A REGULAR FSM VARIABLE
	public static void RefreshValueFromFsmVar(Fsm fromFsm,FsmVar fsmVar)
	{
		if (fromFsm==null)
		{
			return;
		}
		if (fsmVar==null)
		{
			return;
		}
		
		if (!fsmVar.useVariable)
		{
			return;
		}
		
		switch (fsmVar.Type)
		{
		case VariableType.Int:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmInt(fsmVar.variableName) );
			break;
		case VariableType.Float:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmFloat(fsmVar.variableName));
			break;
		case VariableType.Bool:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmBool(fsmVar.variableName));
			break;
		case VariableType.Color:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmColor(fsmVar.variableName));
			break;
		case VariableType.Quaternion:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmQuaternion(fsmVar.variableName));
			break;
		case VariableType.Rect:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmRect(fsmVar.variableName));
			break;
		case VariableType.Vector2:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmVector2(fsmVar.variableName));
			break;
		case VariableType.Vector3:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmVector3(fsmVar.variableName));
			break;
		case VariableType.Texture:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmVector3(fsmVar.variableName));
			break;
		case VariableType.Material:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmMaterial(fsmVar.variableName));
			break;
		case VariableType.String:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmString(fsmVar.variableName));
			break;
		case VariableType.GameObject:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmGameObject(fsmVar.variableName));
			break;
		#if PLAYMAKER_1_8
		case VariableType.Enum:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmEnum(fsmVar.variableName));
			break;
		case VariableType.Array:
			fsmVar.GetValueFrom( (NamedVariable)fromFsm.Variables.GetFsmArray(fsmVar.variableName));
			break;
		#endif
		}
	}
	
	public static object GetValueFromFsmVar(Fsm fromFsm,FsmVar fsmVar)
	{
		if (fromFsm==null)
		{
			return null;
		}
		if (fsmVar==null)
		{
			return null;
		}
		
		if (fsmVar.useVariable)
		{
			string _name = fsmVar.variableName;
			
			switch (fsmVar.Type){
			case VariableType.Int:
				return fromFsm.Variables.GetFsmInt(_name).Value;
			case VariableType.Float:
				return fromFsm.Variables.GetFsmFloat(_name).Value;
			case VariableType.Bool:
				return fromFsm.Variables.GetFsmBool(_name).Value;
			case VariableType.Color:
				return fromFsm.Variables.GetFsmColor(_name).Value;
			case VariableType.Quaternion:
				return fromFsm.Variables.GetFsmQuaternion(_name).Value;
			case VariableType.Rect:
				return fromFsm.Variables.GetFsmRect(_name).Value;
			case VariableType.Vector2:
				return fromFsm.Variables.GetFsmVector2(_name).Value;
			case VariableType.Vector3:
				return fromFsm.Variables.GetFsmVector3(_name).Value;
			case VariableType.Texture:
				return fromFsm.Variables.GetFsmTexture(_name).Value;
			case VariableType.Material:
				return fromFsm.Variables.GetFsmMaterial(_name).Value;
			case VariableType.String:
				return fromFsm.Variables.GetFsmString(_name).Value;
			case VariableType.GameObject:
				return fromFsm.Variables.GetFsmGameObject(_name).Value;
			case VariableType.Object:
				return fromFsm.Variables.GetFsmObject(_name).Value;
			#if PLAYMAKER_1_8
			case VariableType.Enum:
				return fromFsm.Variables.GetFsmEnum(_name).Value;
			case VariableType.Array:
				return fromFsm.Variables.GetFsmArray(_name).Values;
			#endif
			}
		}else{
			
			switch (fsmVar.Type){
			case VariableType.Int:
				return fsmVar.intValue;
			case VariableType.Float:
				return fsmVar.floatValue;
			case VariableType.Bool:
				return fsmVar.boolValue;
			case VariableType.Color:
				return fsmVar.colorValue;
			case VariableType.Quaternion:
				return fsmVar.quaternionValue;
			case VariableType.Rect:
				return fsmVar.rectValue;
			case VariableType.Vector2:
				return fsmVar.vector2Value;
			case VariableType.Vector3:
				return fsmVar.vector3Value;
			case VariableType.Texture:
				return fsmVar.textureValue;
			case VariableType.Material:
				return fsmVar.materialValue;
			case VariableType.String:
				return fsmVar.stringValue;
			case VariableType.GameObject:
				return fsmVar.gameObjectValue;
			case VariableType.Object:
				return fsmVar.objectReference;
			#if PLAYMAKER_1_8
			case VariableType.Enum:
				return fsmVar.EnumValue;
			case VariableType.Array:
				return fsmVar.arrayValue;
			#endif
			}
		}

		return null;
	}
	#if PLAYMAKER_1_8
	public static bool ApplyValueToFsmVar(Fsm fromFsm,FsmVar fsmVar, object[] value)
	{
		if (fromFsm==null)
		{
			return false;
		}
		if (fsmVar==null)
		{
			return false;
		}
		FsmArray _target;

		if (value==null || value.Length == 0)
		{
			if(fsmVar.Type == VariableType.Array ){
				_target= fromFsm.Variables.GetFsmArray(fsmVar.variableName);
				_target.Reset();
			}
			return true;
		}

		if (fsmVar.Type != VariableType.Array)
		{
			Debug.LogError("The fsmVar value <"+fsmVar.Type+"> doesn't match the value <FsmArray> on state"+fromFsm.ActiveStateName+" on fsm:"+fromFsm.Name+" on GameObject:"+fromFsm.GameObjectName);
			return false;
		}

		_target= fromFsm.Variables.GetFsmArray(fsmVar.variableName);
		_target.Values = value;

		return true;
	}
	#endif


	public static bool ApplyValueToFsmVar(Fsm fromFsm,FsmVar fsmVar, object value)
	{
		if (fromFsm==null)
		{
			return false;
		}
		if (fsmVar==null)
		{
			return false;
		}
		
		
		if (value==null)
		{
			if (fsmVar.Type == VariableType.Bool ){
				FsmBool _target= fromFsm.Variables.GetFsmBool(fsmVar.variableName);
				_target.Value = false;
				
			}else if(fsmVar.Type == VariableType.Color ){
				FsmColor _target= fromFsm.Variables.GetFsmColor(fsmVar.variableName);
				_target.Value = Color.black;
				
			}else if(fsmVar.Type == VariableType.Int ){
				FsmInt _target= fromFsm.Variables.GetFsmInt(fsmVar.variableName);
				_target.Value = 0;
				
			}else if(fsmVar.Type == VariableType.Float ){
				FsmFloat _target= fromFsm.Variables.GetFsmFloat(fsmVar.variableName);
				_target.Value = 0f;
				
			}else if(fsmVar.Type == VariableType.GameObject ){	
				FsmGameObject _target= fromFsm.Variables.GetFsmGameObject(fsmVar.variableName);
				_target.Value = null;
				
			}else if(fsmVar.Type == VariableType.Material ){
				FsmMaterial _target= fromFsm.Variables.GetFsmMaterial(fsmVar.variableName);
				_target.Value = null;
				
			}else if(fsmVar.Type == VariableType.Object ){
				FsmObject _target= fromFsm.Variables.GetFsmObject(fsmVar.variableName);
				_target.Value = null;
				
			}else if(fsmVar.Type == VariableType.Quaternion ){
				FsmQuaternion _target= fromFsm.Variables.GetFsmQuaternion(fsmVar.variableName);
				_target.Value = Quaternion.identity;
				
			}else if(fsmVar.Type == VariableType.Rect ){
				FsmRect _target= fromFsm.Variables.GetFsmRect(fsmVar.variableName);
				_target.Value = new Rect(0,0,0,0);
				
			}else if(fsmVar.Type == VariableType.String ){
				FsmString _target= fromFsm.Variables.GetFsmString(fsmVar.variableName);
				_target.Value = "";
				
			}else if(fsmVar.Type == VariableType.String ){
				FsmTexture _target= fromFsm.Variables.GetFsmTexture(fsmVar.variableName);
				_target.Value = null;
				
			}else if(fsmVar.Type == VariableType.Vector2 ){
				FsmVector2 _target= fromFsm.Variables.GetFsmVector2(fsmVar.variableName);
				_target.Value = Vector2.zero;
				
			}else if(fsmVar.Type == VariableType.Vector3 ){
				FsmVector3 _target= fromFsm.Variables.GetFsmVector3(fsmVar.variableName);
				_target.Value = Vector3.zero;
				
			}
			#if PLAYMAKER_1_8
			else if(fsmVar.Type == VariableType.Enum ){
				FsmEnum _target= fromFsm.Variables.GetFsmEnum(fsmVar.variableName);
				_target.ResetValue();
			}else if(fsmVar.Type == VariableType.Array ){
				FsmArray _target= fromFsm.Variables.GetFsmArray(fsmVar.variableName);
				_target.Reset();
			}
			#endif
			return true;
		}
		
		System.Type valueType = value.GetType();
		
		//Debug.Log("valueType  "+valueType);
		
		System.Type storageType = null;
		
	//	Debug.Log("fsmVar type "+fsmVar.Type);
		
		switch (fsmVar.Type)
		{
		case VariableType.Int:
			storageType = typeof(int);
			break;
		case VariableType.Float:
			storageType = typeof(float);
			break;
		case VariableType.Bool:
			storageType = typeof(bool);
			break;
		case VariableType.Color:
			storageType = typeof(Color);
			break;
		case VariableType.GameObject:
			storageType = typeof(GameObject);
			break;
		case VariableType.Quaternion:
			storageType = typeof(Quaternion);
			break;
		case VariableType.Rect:
			storageType = typeof(Rect);
			break;
		case VariableType.String:
			storageType = typeof(string);
			break;
		case VariableType.Texture:
			storageType = typeof(Texture2D);
			break;
		case VariableType.Vector2:
			storageType = typeof(Vector2);
			break;
		case VariableType.Vector3:
			storageType = typeof(Vector3);
			break;
		case VariableType.Object:
			storageType = typeof(Object);
			break;
		case VariableType.Material:
			storageType = typeof(Material);
			break;
		#if PLAYMAKER_1_8
		case VariableType.Enum:
			storageType = typeof(System.Enum);
			break;
		case VariableType.Array:
			storageType = typeof(System.Array);
			break;
		#endif
		}
		
		bool ok = true;
		if (! storageType.Equals(valueType))
		{
			ok = false;
			if (storageType.Equals(typeof(Object))) // we are ok
			{
				ok = true;
			}
			#if PLAYMAKER_1_8
			if (storageType.Equals(typeof(System.Enum))) // we are ok
			{
				ok = true;
			}
			#endif
			if (!ok)
			{
				#if UNITY_WEBGL || UNITY_2017_1_OR_NEWER  || UNITY_2018_1_OR_NEWER
				// proceduralMaterial not supported
				#else
				if (valueType.Equals(typeof(ProceduralMaterial))) // we are ok
				{
					ok = true;
				}
				#endif
				if (valueType.Equals(typeof(double))) // we are ok
				{
					ok = true;
				}
				if (valueType.Equals(typeof(System.Int64))) // we are ok
				{
					ok = true;
				}
				if (valueType.Equals(typeof(System.Byte))) // we are ok
				{
					ok = true;
				}

			}
		}
		
		
		if (!ok)
		{
			Debug.LogError("The fsmVar value <"+storageType+"> doesn't match the value <"+valueType+"> on state"+fromFsm.ActiveStateName+" on fsm:"+fromFsm.Name+" on GameObject:"+fromFsm.GameObjectName);
			return false;
		}
		
		
		if (valueType == typeof(bool) ){
			FsmBool _target= fromFsm.Variables.GetFsmBool(fsmVar.variableName);
			_target.Value = (bool)value;
			
		}else if(valueType == typeof(Color) ){
			FsmColor _target= fromFsm.Variables.GetFsmColor(fsmVar.variableName);
			_target.Value = (Color)value;
			
		}else if(valueType == typeof(int)){
			FsmInt _target= fromFsm.Variables.GetFsmInt(fsmVar.variableName);
			_target.Value = System.Convert.ToInt32(value);

		}else if(valueType == typeof(byte)){
			FsmInt _target= fromFsm.Variables.GetFsmInt(fsmVar.variableName);
			_target.Value = System.Convert.ToInt32(value);


		}else if(valueType == typeof(System.Int64)){
			
			if (fsmVar.Type == VariableType.Int)
			{
				FsmInt _target= fromFsm.Variables.GetFsmInt(fsmVar.variableName);
				_target.Value = System.Convert.ToInt32(value);
			}else if (fsmVar.Type == VariableType.Float)
			{
				FsmFloat _target= fromFsm.Variables.GetFsmFloat(fsmVar.variableName);
				_target.Value =  System.Convert.ToSingle(value);
			}
			
			
		}else if(valueType == typeof(float) ){
			FsmFloat _target= fromFsm.Variables.GetFsmFloat(fsmVar.variableName);
			_target.Value = (float)value;
			
		}else if(valueType == typeof(double) ){
			FsmFloat _target= fromFsm.Variables.GetFsmFloat(fsmVar.variableName);
			_target.Value =  System.Convert.ToSingle(value);
			
		}else if(valueType == typeof(GameObject) ){	
			FsmGameObject _target= fromFsm.Variables.GetFsmGameObject(fsmVar.variableName);
			_target.Value = (GameObject)value;
			
		}else if(valueType == typeof(Material) ){
			FsmMaterial _target= fromFsm.Variables.GetFsmMaterial(fsmVar.variableName);
			_target.Value = (Material)value;

		#if UNITY_WEBGL || UNITY_2017_1_OR_NEWER  || UNITY_2018_1_OR_NEWER
		// proceduralMaterial not supported
		#else
		}else if(valueType == typeof(ProceduralMaterial) ){
			FsmMaterial _target= fromFsm.Variables.GetFsmMaterial(fsmVar.variableName);
			_target.Value = (ProceduralMaterial)value;
		#endif

		}else if(valueType == typeof(Object) || storageType == typeof(Object) ){
			FsmObject _target= fromFsm.Variables.GetFsmObject(fsmVar.variableName);
			_target.Value = (Object)value;
			
		}else if(valueType == typeof(Quaternion) ){
			FsmQuaternion _target= fromFsm.Variables.GetFsmQuaternion(fsmVar.variableName);
			_target.Value = (Quaternion)value;
			
		}else if(valueType == typeof(Rect) ){
			FsmRect _target= fromFsm.Variables.GetFsmRect(fsmVar.variableName);
			_target.Value = (Rect)value;
			
		}else if(valueType == typeof(string) ){
			FsmString _target= fromFsm.Variables.GetFsmString(fsmVar.variableName);
			_target.Value = (string)value;
			
		}else if(valueType == typeof(Texture2D) ){
			FsmTexture _target= fromFsm.Variables.GetFsmTexture(fsmVar.variableName);
			_target.Value = (Texture2D)value;
			
		}else if(valueType == typeof(Vector2) ){
			FsmVector2 _target= fromFsm.Variables.GetFsmVector2(fsmVar.variableName);
			_target.Value = (Vector2)value;
			
		}else if(valueType == typeof(Vector3) ){
			FsmVector3 _target= fromFsm.Variables.GetFsmVector3(fsmVar.variableName);
			_target.Value = (Vector3)value;

		#if PLAYMAKER_1_8
		}else if(value is System.Enum){ // valueType.BaseType == typeof(System.Enum)
			FsmEnum _target= fromFsm.Variables.GetFsmEnum(fsmVar.variableName);
			_target.Value = (System.Enum)value;
		#endif
		}else{
			Debug.LogWarning("?!?!"+valueType);
			//  don't know, should I put in FsmObject?	
		}
		
		
		return true;
	}
	
	
	public static float GetFloatFromObject(object _obj,VariableType targetType, bool fastProcessingIfPossible)
		{
			if(targetType == VariableType.Int || targetType == VariableType.Float )
			{
				return System.Convert.ToSingle(_obj);
			}
		
			if(targetType == VariableType.Vector2 )
			{
				Vector2 _v2 = (Vector2)_obj;
				if (_v2!=Vector2.zero)
				{
					return fastProcessingIfPossible ? _v2.sqrMagnitude : _v2.magnitude;
				}
				
			}
			
			if(targetType == VariableType.Vector3 )
			{
				Vector3 _v3 = (Vector3)_obj;
				if (_v3!=Vector3.zero)
				{
					return fastProcessingIfPossible ? _v3.sqrMagnitude : _v3.magnitude;
				}
				
			}
			
			if(targetType == VariableType.GameObject)
			{
				GameObject _go = (GameObject)_obj;
				if (_go!=null)
				{
					MeshRenderer _m = _go.GetComponent<MeshRenderer>();
					if (_m!=null)
					{
						return _m.bounds.size.x*_m.bounds.size.y*_m.bounds.size.z;
					}
				}


			}
		
			
			if(targetType == VariableType.Rect)
			{
				Rect _rect = (Rect)_obj;
				return _rect.width*_rect.height;	
			}
			
			if(targetType == VariableType.String)
			{
				string _string = (string)_obj;
				if (_string!=null)
				{
					return float.Parse(_string);
				}
			}
			
			return 0f;
		}
	
	
}

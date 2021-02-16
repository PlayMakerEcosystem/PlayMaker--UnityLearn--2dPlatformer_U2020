// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEditor;
using UnityEngine;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMakerEditor.Ecosystem.Utils
{



	public class PlayMakerEventProxyCreator {
		
        public enum ParameterType
        {
            none,
            Float,
            Int,
            Bool,
            GameObject,
            String,
            Vector2,
            Vector3,
            Color,
            Rect,
            Material,
            Texture,
            Quaternion,
            Object
        }

		
		public class ValidationResult
		{
			public bool success = true;
			public string message = "";
			
			public ValidationResult(bool success,string message){
				this.success = success;
				this.message = message;
			}
			
			public static ValidationResult ValidResult()
			{
				return new ValidationResult(true,"");
			}
		}
		
		[Serializable]
		public class PlayMakerEventProxyCreatorDefinition
		{
			
			#region Data
			/// <summary>
			/// The name space.
			/// </summary>
			public string NameSpace = "com.yourdomain";
			
			/// <summary>
			/// The name.
			/// </summary>
			public string Name = "MyMessageProxy";
			
			/// <summary>
			/// The folder path.
			/// </summary>
			public string FolderPath = "PlayMaker Custom Scripts/EventProxies/";
			
			/// <summary>
			/// The public method name.
			/// </summary>
			public string PublicMethodName = "MyMessage";
			
            /// <summary>
            /// The variable type.
            /// </summary>
            public ParameterType Parameter = ParameterType.none;

			/// <summary>
			/// The full filePath of this definition.
			/// deduced property or injected property 
			/// </summary>
			public string filePath = "";
			
			public string directoryPath ="";
			
			/// <summary>
			/// The script literal. It's only use for preview (shorter version, minus comments and all)
			/// </summary>
			public string LiteralPreview = "";
			
			/// <summary>
			/// The script literal.
			/// </summary>
			public string ScriptLiteral = "";
			
			#endregion
			
			
			public void UpdateFilePath()
			{
				
				string fileName = Name+".cs";
				directoryPath = Path.Combine(Application.dataPath, FolderPath);
				
				filePath = Path.Combine(directoryPath,fileName);
			}
			
			#region Validation
			
			public ValidationResult DefinitionValidation	= ValidationResult.ValidResult();
			public ValidationResult NameSpaceValidation		= ValidationResult.ValidResult();
			public ValidationResult NameValidation			= ValidationResult.ValidResult();
			public ValidationResult FolderPathValidation	= ValidationResult.ValidResult();
			public ValidationResult PublicMethodValidation	= ValidationResult.ValidResult();
			
			public ValidationResult ValidateDefinition()
			{
				NameSpaceValidation		= ValidateNameSpace();
				NameValidation			= ValidateClassName();
				PublicMethodValidation	= ValidateMethodName();
				FolderPathValidation	= ValidateFolderPath();
				
				if (
					NameSpaceValidation.success
					&&
					NameValidation.success
					&& 
					FolderPathValidation.success
					&&
					PublicMethodValidation.success
					)
				{
					DefinitionValidation =  ValidationResult.ValidResult();
				}else{
					DefinitionValidation = new ValidationResult(false,"Invalid Definition: Please correct fields with errors");
				}
				
				return DefinitionValidation;
			}
			
			private readonly Regex doubleDot = new Regex("\\.\\.");
			//private readonly Regex FolderPathRegex =  new Regex("^([a-zA-Z0-9][^*/><?\"|:]*)$");
			//private readonly Regex FolderNameRegex = new Regex("[" + Regex.Escape(Path.GetInvalidPathChars) + "]");
			private readonly CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");
			
			public ValidationResult ValidateNameSpace()
			{
				if ( String.IsNullOrEmpty(NameSpace.Trim()) )
				{
					return new ValidationResult(false, "A Namespace must be provided.");
				}
				
				
				if (doubleDot.IsMatch(NameSpace))
				{
					return new ValidationResult(false, "NameSpace Structure is not valid.");
				}
				
				var inputs = (NameSpace as string).Split('.');
				foreach (var item in inputs)
				{
					if (!provider.IsValidIdentifier(item))
					{
						return new ValidationResult(false, string.Format("NameSpace sub element '{0}' is invalid.", item));
					}
				}
				return ValidationResult.ValidResult();
			}
			
			
			public ValidationResult ValidateClassName()
			{
				if ( String.IsNullOrEmpty(Name.Trim()))
				{
					return new ValidationResult(false, "A name must be provided.");
				}
				if (!provider.IsValidIdentifier(Name))
				{
					return new ValidationResult(false,"Name is invalid");
				}
				
				return ValidationResult.ValidResult();
			}

			public ValidationResult ValidateMethodName()
			{
				if ( String.IsNullOrEmpty(PublicMethodName.Trim()))
				{
					return new ValidationResult(false, "A name must be provided.");
				}
				if (!provider.IsValidIdentifier(PublicMethodName))
				{
					return new ValidationResult(false,"Name is invalid");
				}

				if (Name.Equals(PublicMethodName))
				{
					return new ValidationResult(false,"Method Name can not be the same as the Class Name, append 'Proxy' to the class for example");
				}
				
				return ValidationResult.ValidResult();
			}
			
			
			
			public ValidationResult ValidateFolderPath()
			{
				// we accept not folder path, meaning it will be at the root of the assets.
				if (String.IsNullOrEmpty(FolderPath))
				{
					return ValidationResult.ValidResult();
				}
			//	string outputPath = Path.Combine(Application.dataPath, FolderPath);
				
				
			//	var inputs = (FolderPath as string).Split('/');
				/*
				foreach (var item in inputs)
				{

					if (FolderNameRegex.IsMatch(item))
					{
						return new ValidationResult(false, string.Format("Folder '{0}' is invalid.", item));
					}

				}
				*/
				return ValidationResult.ValidResult();
			}
			
		}


		#endregion Validation
		static string Template_MainStructure =@"[HEADER]

[STRUCTURE]";
		
		static string Template_Header = @"// (c) Copyright HutongGames, LLC 2010-[YEAR]. All rights reserved.
// THIS CONTENT IS AUTOMATICALLY GENERATED. [TAG]
// this script was generated by the 'PlayMaker Event Proxy Wizard'. You can edit this script directly now, but prefer using the wizard if you are not sure.

using UnityEngine;
using HutongGames.PlayMaker.Ecosystem.Utils;
using HutongGames.PlayMaker;";
		
		static string Template_Class = @"namespace [NAMESPACE]
{
	public class [CLASS_NAME] : PlayMakerEventProxy {

		[Button(""[METHOD_NAME]"",""Test : [METHOD_NAME_NICIFIED]""[BUTTON_ATTR_USE_VALUE])] public [BUTTON_ATTR_RETURN_TYPE] _;
		public void [METHOD_NAME]([METHOD_PARAMETERS])
		{
			if (debug || !Application.isPlaying)
			{
				Debug.Log(""[CLASS_NAME] : [METHOD_NAME]([METHOD_PARAMETERS_VALUES])"");
			}

            [EVENTDATA_SETTER]

			base.SendPlayMakerEvent();
		}
	}
}";
		
		/// <summary>
		/// Create a new script featuring the new enum.
		/// </summary>
		public void CreateProxy(PlayMakerEventProxyCreatorDefinition definition)
		{
			BuildScriptLiteral(definition);
			
			definition.UpdateFilePath();
			
			// Ensure that this path actually exists.
			if (!Directory.Exists(definition.directoryPath))
				Directory.CreateDirectory(definition.directoryPath);
			
			File.WriteAllText(definition.filePath, definition.ScriptLiteral);
			
			AssetDatabase.Refresh();
		}
		
		
		public void BuildScriptLiteral(PlayMakerEventProxyCreatorDefinition definition)
		{
			
			
			// build the header
			string headerLiteral = Template_Header;
			headerLiteral = headerLiteral.Replace("[YEAR]",DateTime.Today.Year.ToString());
			headerLiteral = headerLiteral.Replace("[TAG]","__"+"PLAYMAKER_EVENT_PROXY__");

			// build the structure
			string structureliteral = Template_Class;
			structureliteral = structureliteral.Replace("[NAMESPACE]",definition.NameSpace);
			structureliteral = structureliteral.Replace("[CLASS_NAME]",definition.Name);

			structureliteral = structureliteral.Replace("[METHOD_NAME]",definition.PublicMethodName);
			structureliteral = structureliteral.Replace("[METHOD_NAME_NICIFIED]",definition.PublicMethodName);


            structureliteral = structureliteral.Replace("[METHOD_PARAMETERS]", GetMethodParametersLiteral(definition));
            structureliteral = structureliteral.Replace("[METHOD_PARAMETERS_VALUES]", GetMethodParametersValuesLiteral(definition));
            structureliteral = structureliteral.Replace("[EVENTDATA_SETTER]", GetMethodParameterEventDataSetterLiteral(definition));
            structureliteral = structureliteral.Replace("[BUTTON_ATTR_RETURN_TYPE]", GetButtonAttrReturnValueLiteral(definition.Parameter));
            structureliteral = structureliteral.Replace("[BUTTON_ATTR_USE_VALUE]", GetButtonAttrUseValueLiteral(definition.Parameter));

         
			// build script literal
			string scriptLiteral = Template_MainStructure;
			scriptLiteral = scriptLiteral.Replace("[HEADER]",headerLiteral);
			scriptLiteral = scriptLiteral.Replace("[STRUCTURE]",structureliteral);
			definition.ScriptLiteral = scriptLiteral;
			
			definition.LiteralPreview = structureliteral;
		}

        string GetButtonAttrUseValueLiteral(ParameterType _type)
        {
            if (_type == ParameterType.none)
            {
                return string.Empty;
            }

            return ",true";
        }

        string GetButtonAttrReturnValueLiteral(ParameterType _type)
        {
            // when none, we still need something
            if (_type == ParameterType.none)
            {
                return "bool";
            }

            return GetMethodParameterTypeLiteral(_type);
        }

        string GetMethodParameterTypeLiteral(ParameterType _type)
        {
            string _literal = "void";

            if (_type == ParameterType.Float)
            {
                _literal = "float";
            }
            else if (_type == ParameterType.Int)
            {
                _literal = "int";
            }
            else if (_type == ParameterType.Bool)
            {
                _literal = "bool";
            }
            else if (_type == ParameterType.GameObject)
            {
                _literal = "GameObject";
            }
            else if (_type == ParameterType.String)
            {
                _literal = "string";
            }
            else if (_type == ParameterType.Vector2)
            {
                _literal = "Vector2";
            }
            else if (_type == ParameterType.Vector3)
            {
                _literal = "Vector3";
            }
            else if (_type == ParameterType.Color)
            {
                _literal = "Color";
            }
            else if (_type == ParameterType.Rect)
            {
                _literal = "Rect";
            }
            else if (_type == ParameterType.Material)
            {
                _literal = "Material";
            }
            else if (_type == ParameterType.Texture)
            {
                _literal = "Texture";
            }
            else if (_type == ParameterType.Quaternion)
            {
                _literal = "Quaternion";
            }
            else if (_type == ParameterType.Object)
            {
                _literal = "Object";
            }

            return _literal;
        }

        string GetMethodParametersLiteral(PlayMakerEventProxyCreatorDefinition definition)
        {

            if (definition.Parameter == ParameterType.none)
            {
                return string.Empty;
            }
            string _literal = "";

            if (definition.Parameter == ParameterType.Float)
            {
                _literal += "float";
            }
            else if (definition.Parameter == ParameterType.Int)
            {
                _literal += "int";
            }
            else if (definition.Parameter == ParameterType.Bool)
            {
                _literal += "bool";
            }
            else if (definition.Parameter == ParameterType.GameObject)
            {
                _literal += "GameObject";
            }
            else if (definition.Parameter == ParameterType.String)
            {
                _literal += "string";
            }
            else if (definition.Parameter == ParameterType.Vector2)
            {
                _literal += "Vector2";
            }
            else if (definition.Parameter == ParameterType.Vector3)
            {
                _literal += "Vector3";
            }
            else if (definition.Parameter == ParameterType.Color)
            {
                _literal += "Color";
            }
            else if (definition.Parameter == ParameterType.Rect)
            {
                _literal += "Rect";
            }
            else if (definition.Parameter == ParameterType.Material)
            {
                _literal += "Material";
            }
            else if (definition.Parameter == ParameterType.Texture)
            {
                _literal += "Texture";
            }
            else if (definition.Parameter == ParameterType.Quaternion)
            {
                _literal += "Quaternion";
            }
            else if (definition.Parameter == ParameterType.Object)
            {
                _literal += "Object";
            }


            _literal += " parameter";


            return _literal;
        }

        string GetMethodParametersValuesLiteral(PlayMakerEventProxyCreatorDefinition definition)
        {

            if (definition.Parameter == ParameterType.none)
            {
                return string.Empty;
            }

            if (
                definition.Parameter == ParameterType.Bool ||
                definition.Parameter == ParameterType.Float ||
                definition.Parameter == ParameterType.Int
               )
            {
                return "\"+(parameter.ToString())+\"";
            }

            return "\"+(parameter == null?\"NULL\":parameter.ToString())+\"";
        }

        string GetMethodParameterEventDataSetterLiteral(PlayMakerEventProxyCreatorDefinition definition)
        {

            if (definition.Parameter == ParameterType.none)
            {
                return string.Empty;
            }

            string _literal = "Fsm.EventData.";


            if (definition.Parameter == ParameterType.Float)
            {
                _literal += "FloatData";
            }
            else if (definition.Parameter == ParameterType.Int)
            {
                _literal += "IntData";
            }
            else if (definition.Parameter == ParameterType.Bool)
            {
                _literal += "BoolData";
            }
            else if (definition.Parameter == ParameterType.GameObject)
            {
                _literal += "GameObjectData";
            }
            else if (definition.Parameter == ParameterType.String)
            {
                _literal += "StringData";
            }
            else if (definition.Parameter == ParameterType.Vector2)
            {
                _literal += "Vector2Data";
            }
            else if (definition.Parameter == ParameterType.Vector3)
            {
                _literal += "Vector3Data";
            }
            else if (definition.Parameter == ParameterType.Color)
            {
                _literal += "ColorData";
            }
            else if (definition.Parameter == ParameterType.Rect)
            {
                _literal += "RectData";
            }
            else if (definition.Parameter == ParameterType.Material)
            {
                _literal += "MaterialData";
            }
            else if (definition.Parameter == ParameterType.Texture)
            {
                _literal += "TextureData";
            }
            else if (definition.Parameter == ParameterType.Quaternion)
            {
                _literal += "QuaternionData";
            }
            else if (definition.Parameter == ParameterType.Object)
            {
                _literal += "ObjectData";
            }

            _literal += " = parameter;";


            return _literal;
        }
	}
}


using UnityEditor;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;

using System.Collections.Generic;

using HutongGames.PlayMaker.Ecosystem.Utils;

public class PlayMakerCurrentEventDataEditor : EditorWindow {


	public bool LiveUpdate;
	static GUIStyle _BigTitle;

	EventDataSenderProxy _mpes;
	SerializedObject m_serializedObject;
	SerializedProperty m_EventTargetProperty;

	static PlayMakerCurrentEventDataEditor window;


    [MenuItem ("PlayMaker/Addons/Editor Windows/Current Event Data")]
    static void Init () {
        // Get existing open window or if none, make a new one:
		window = (PlayMakerCurrentEventDataEditor)EditorWindow.GetWindow (typeof (PlayMakerCurrentEventDataEditor));

		window.position = new Rect(100,100, 430,600);
		window.minSize = new Vector2(200,500);
		#if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0
		window.title = "Event Data";
		#else
		
		window.titleContent = new GUIContent("Event Data","PlayMaker Current Event Data");
		#endif


    }

	void OnInspectorUpdate() {
		
		if (LiveUpdate) {
			Repaint ();
		}
		
	}

	string SentByFsm ="";
	PlayMakerFSM SentByFsmComponent;
	string SentByFsmState = "";
	//string SentByFsmAction = "";

	void OnGUI () {wantsMouseMove = true;


		EditorGUIUtility.labelWidth = 150f; 
		EditorGUIUtility.wideMode = true;

		if (Event.current.type == EventType.MouseMove) Repaint ();

		GUILayout.BeginHorizontal(EditorStyles.toolbar);
		if (GUILayout.Button ("Reset Event Data", EditorStyles.toolbarButton)) {
			Fsm.EventData = new FsmEventData();

			PlayMakerInspectorUtils.RemoveFocus();

			GUIUtility.ExitGUI();
		}

		GUILayout.FlexibleSpace();
		LiveUpdate = GUILayout.Toggle(LiveUpdate,"Live Update",EditorStyles.toolbarButton);
		
		GUILayout.EndHorizontal ();


		OnGUI_Title("Sender Data");

		if (Fsm.EventData.SentByFsm != null) {
			SentByFsmComponent = Fsm.EventData.SentByFsm.FsmComponent;
			SentByFsm = Fsm.EventData.SentByFsm.Name;
		} else {
			SentByFsmComponent = null;
			SentByFsm = "n/a";
		}

		if (Fsm.EventData.SentByState != null) {
			SentByFsmState = Fsm.EventData.SentByState.Name;
		} else {
			SentByFsmState = "n/a";
		}

//		if (Fsm.EventData.SentByAction != null) {
//			SentByFsmAction = Fsm.EventData.SentByAction.Name;
//		} else {
//			SentByFsmAction = "n/a";
//		}


		EditorGUILayout.LabelField ("Fsm Name", SentByFsm);
		GUI.enabled = false;
		EditorGUILayout.ObjectField("Fsm Component",SentByFsmComponent, typeof(PlayMakerFSM), true);
		GUI.enabled = true;

		EditorGUILayout.LabelField ("Fsm State", SentByFsmState);

		//EditorGUILayout.LabelField ("Fsm Action", SentByFsmAction);

		GUILayout.Space (10f);

		OnGUI_Title("Event Data");

		bool _b = EditorGUILayout.Toggle("Bool", Fsm.EventData.BoolData);
		if (_b != Fsm.EventData.BoolData) {
			Fsm.EventData.BoolData = _b;
		}

		int _i = EditorGUILayout.IntField ("Int", Fsm.EventData.IntData);
		if (_i != Fsm.EventData.IntData) {
			Fsm.EventData.FloatData = _i;
		}

		float _f = EditorGUILayout.FloatField ("Float", Fsm.EventData.FloatData);
		if (_f != Fsm.EventData.FloatData) {
			Fsm.EventData.FloatData = _f;
		}

		Vector2 _v2 = EditorGUILayout.Vector2Field ("Vector2", Fsm.EventData.Vector2Data);
		if (_v2 != Fsm.EventData.Vector2Data) {
			Fsm.EventData.Vector2Data = _v2;
		}
		
		Vector3 _v3 = EditorGUILayout.Vector3Field ("Vector3", Fsm.EventData.Vector3Data);
		if (_v3 != Fsm.EventData.Vector3Data) {
			Fsm.EventData.Vector3Data = _v3;
		}

		string _s = EditorGUILayout.TextField ("String", Fsm.EventData.StringData);
		if (_s != Fsm.EventData.StringData) {
			Fsm.EventData.StringData = _s;
		}

		GameObject _go = (GameObject) EditorGUILayout.ObjectField ("GameObject", Fsm.EventData.GameObjectData,typeof(GameObject),true);
		if (_go != Fsm.EventData.GameObjectData) {
			Fsm.EventData.GameObjectData = _go;
		}

		Rect _r = EditorGUILayout.RectField ("Rect", Fsm.EventData.RectData);
		if (_r != Fsm.EventData.RectData) {
			Fsm.EventData.RectData = _r;
		}

		GUILayout.BeginHorizontal (GUILayout.ExpandWidth(false));
			EditorGUILayout.PrefixLabel ("Quaternion");
			float _lw = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 12f; 
			float x = EditorGUILayout.FloatField ("X",Fsm.EventData.QuaternionData.x);
			float y = EditorGUILayout.FloatField ("Y",Fsm.EventData.QuaternionData.y);
			float z = EditorGUILayout.FloatField ("Z",Fsm.EventData.QuaternionData.z);
			float w = EditorGUILayout.FloatField ("W",Fsm.EventData.QuaternionData.w);
			EditorGUIUtility.labelWidth = _lw; 
		GUILayout.EndHorizontal();

		if (x != Fsm.EventData.QuaternionData.x || 
		    y != Fsm.EventData.QuaternionData.y ||
		    z != Fsm.EventData.QuaternionData.z ||
		    w != Fsm.EventData.QuaternionData.w) {
			Fsm.EventData.QuaternionData = new Quaternion(x,y,z,w);
		}

		Material _m = (Material) EditorGUILayout.ObjectField ("Material", Fsm.EventData.MaterialData,typeof(Material),true);
		if (_m != Fsm.EventData.MaterialData) {
			Fsm.EventData.MaterialData = _m;
		}

		Texture _t = (Texture) EditorGUILayout.ObjectField ("Texture", Fsm.EventData.TextureData,typeof(Texture),true);
		if (_t != Fsm.EventData.TextureData) {
			Fsm.EventData.TextureData = _t;
		}

		Color _c = EditorGUILayout.ColorField ("Color", Fsm.EventData.ColorData);
		if (_c != Fsm.EventData.ColorData) {
			Fsm.EventData.ColorData = _c;
		}

		Object _g = (Object) EditorGUILayout.ObjectField ("Object", Fsm.EventData.ObjectData,typeof(Object),true);
		if (_g != Fsm.EventData.ObjectData) {
			Fsm.EventData.ObjectData = _g;
		}

//		OnGUI_Title("Send Event");
//
//
//		
//		if (_mpes == null)
//		{
//			_mpes =ScriptableObject.CreateInstance<EventDataSenderProxy> ();
//		}
//
//		if (m_serializedObject == null) {
//			m_serializedObject = new UnityEditor.SerializedObject(_mpes);
//			m_EventTargetProperty = m_serializedObject.FindProperty("EventTarget");
//			//window.m_serializedProperty  = m_serializedObject.FindProperty("foobar");
//		}
//
//
//		m_serializedObject.Update();
//
//		EditorGUILayout.PropertyField(m_EventTargetProperty , true);
//
//		m_serializedObject.ApplyModifiedProperties();



    }

	void OnGUI_Title(string title)
	{
		if (_BigTitle == null) {
			_BigTitle = GUI.skin.FindStyle ("IN BigTitle");
		}
		
		GUILayout.BeginHorizontal (_BigTitle,GUILayout.ExpandWidth(true));
		GUILayout.Label (title,EditorStyles.boldLabel,GUILayout.ExpandWidth(true));
		GUILayout.EndHorizontal ();
		
	}


}
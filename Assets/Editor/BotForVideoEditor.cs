using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BotForVideo))]
public class BotForVideoEditor : Editor
{
	private bool toogleEnabled = true, buttonEnabled = false;


	private void Awake()
	{
		if(EditorApplication.isPlaying)
		{
			toogleEnabled = false;
			buttonEnabled = true;
		}
	}

	public override void OnInspectorGUI()
	{
		//DrawDefaultInspector();
		BotForVideo bot = (BotForVideo)target;

		#region Script & Transform:
		GUI.enabled = false;
		EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((BotForVideo)target), typeof(BotForVideo), false);
		GUI.enabled = true;

		bot.Player = (Transform)EditorGUILayout.ObjectField(new GUIContent("Player"), bot.Player, typeof(Transform), true);
		#endregion Script & Transform.

		#region Toggle:
		GUI.enabled = toogleEnabled;
		//EditorStyles.label.fontStyle = FontStyle.Bold;
		bot.Record = EditorGUILayout.Toggle("Record", bot.Record);
		//EditorStyles.label.fontStyle = FontStyle.Normal;
		#endregion Toggle.

		#region Button:
		GUI.enabled = buttonEnabled;
		string buttonName = (bot.Record) ? "Stop & Save Record" : "Stop Playback";
		if(GUILayout.Button(buttonName))
		{
			bot.Stop();
			buttonEnabled = false;
		}
		GUI.enabled = true;
		#endregion Button.
	}
}

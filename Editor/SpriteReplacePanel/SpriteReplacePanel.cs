using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpriteReplacePanel : EditorWindow {

	GameObject prefab = null;
	public GameObject source;
	//public SpriteRenderer[] renderers;

	private Vector2 _scrollPosition;

	SpriteLinkageCollection slc;

	int edit_type = 0;
	string[] options = new string[]
	{
		"Sprite", "Material", 
	};

	[MenuItem ("Window/Sprite Replace Panel")]
	public static void ShowWindow () {
		EditorWindow.GetWindow (typeof(SpriteReplacePanel));
	}


	void OnGUI () {
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label ("Parent", EditorStyles.boldLabel);
		var tmp_source = EditorGUILayout.ObjectField(source, typeof(Object), true) as GameObject;
		EditorGUILayout.EndHorizontal();

		edit_type = EditorGUILayout.Popup("Label", edit_type, options);


		if (_IsSourceModified(tmp_source)) {
			source = tmp_source;
			if (edit_type == 0) {
				slc = new SpriteLinkageCollection (source.GetComponentsInChildren (typeof(SpriteRenderer)));
			} else if (edit_type == 1) {
				//slc = new SpriteLinkageCollection (source.GetComponentsInChildren (typeof(SpriteRenderer)));
			}
		}


		_DisplayMainView ();


		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button ("Save")) {
			_Save ();
		}

		if (GUILayout.Button ("Reset")) {
			_Reset ();
		}
		EditorGUILayout.EndHorizontal();

	}

	private bool _IsSourceModified(GameObject tmp_source){
		return tmp_source != null && (source == null || source != tmp_source);
	}

	private void _DisplayMainView(){
		if (source != null && slc != null) {
			_scrollPosition = EditorGUILayout.BeginScrollView (_scrollPosition);
			_SetColumnHeader ();

			foreach(var sl in slc.linkages){
				EditorGUILayout.BeginHorizontal();

				GUILayout.Label (sl.GetDisplayNames());
				if (edit_type == 0) {
					sl.obj = EditorGUILayout.ObjectField (sl.obj, typeof(Sprite), true, GUILayout.Width (500)) as Sprite;
				} else if (edit_type == 1) {
					//sl.obj = EditorGUILayout.ObjectField (sl.obj, typeof(Material), true, GUILayout.Width (500)) as Material;
				}
				slc.ApplyChanges ();
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView ();
		}
	}

	private void _SetColumnHeader(){
		EditorGUILayout.BeginHorizontal();

		GUILayout.Label ("Objects");
		GUILayout.Label ("Sprite");

		EditorGUILayout.EndHorizontal();

	}

	private void _Save(){
		if (slc != null) {
			slc.SaveChanges ();
		}
	}

	private void _Reset(){
		if (slc != null) {
			slc.RevertChanges ();
		}

		slc = new SpriteLinkageCollection (source.GetComponentsInChildren(typeof(SpriteRenderer)));
	}
}

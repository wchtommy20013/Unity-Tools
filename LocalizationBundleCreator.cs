using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;
public class LocalizationBundleCreator : EditorWindow 
{
	public string _lastselectedAssetPath = "";
	private string _searchAssetPath = "";


	bool createFolderEnabled = false;

	Dictionary<string, bool> d = new Dictionary<string, bool>{
		{"_en", false},
		{"_zh-hant",false},
		{"_ko",false},
		{"_ja",false},
		{"_fr",false},
		{"_de",false},
		{"_es",false},
		{"share",false}
	};

	public bool selectedAllEnabled =false;
	string selectedString = "SelectedAll";

	// Add menu name
	[MenuItem("Tools/Localization Bundle Creator")]
	public static void Init()
	{
		LocalizationBundleCreator window = (LocalizationBundleCreator)EditorWindow.GetWindow (typeof(LocalizationBundleCreator));
		window.titleContent.text = "Localization Bundle"; //set a window title
		window.Show ();
	}
	void OnEnable()
	{
		//Selection.selectionChanged += OnSelectionChanged;


	}
	void OnDisable()
	{
		//Selection.selectionChanged -= OnSelectionChanged;

	}

	void OnSelectionChanged()
	{
		var selectedAssetGUID = Selection.assetGUIDs.FirstOrDefault();
		//Debug.Log (selectedAssetGUID);

		if (selectedAssetGUID == null)
			return;

		var selectedAssetPath = AssetDatabase.GUIDToAssetPath (selectedAssetGUID);
		_lastselectedAssetPath = selectedAssetPath;

	}

	bool last_selectedAllEnabled;
	void OnGUI() // create the layout
	{
		GUILayout.Label ("Select your folder path:", EditorStyles.boldLabel);

		_searchAssetPath = EditorGUILayout.TextField ("Search AssetPath", _searchAssetPath);
		OnSelectionChanged ();
		EditorGUILayout.LabelField (_lastselectedAssetPath, GUILayout.ExpandWidth (true));

		if (_lastselectedAssetPath == null)
			return;

		createFolderEnabled = EditorGUILayout.Toggle ("Create Folder", createFolderEnabled);

		if (createFolderEnabled) {
			
			for (var i = 0; i < d.Keys.Count; ++i) 
			{
				var key = d.Keys.ToArray () [i];
				d [key] = EditorGUILayout.Toggle (key, d [key]);
			}
			EditorGUILayout.Space ();
			selectedAllEnabled = EditorGUILayout.Toggle (selectedString, selectedAllEnabled);

			if (last_selectedAllEnabled == null || last_selectedAllEnabled != selectedAllEnabled) 
			{
				for (var n = 0; n < d.Keys.Count; ++n) {
					var key = d.Keys.ToArray () [n];
					d [key] = selectedAllEnabled;
				}
				last_selectedAllEnabled = selectedAllEnabled;
			}
		}

		GUILayout.FlexibleSpace();

		if (GUILayout.Button ("Create")) {
			for (var i = 0; i < d.Keys.Count; ++i) {
				var key = d.Keys.ToArray () [i];

				if (d [key] == true && key != "share") {
					string guid = AssetDatabase.CreateFolder (_lastselectedAssetPath, key);
					string _LocalizationFolderPath = AssetDatabase.GUIDToAssetPath (guid);
					string _defaultFolder = AssetDatabase.CreateFolder (_LocalizationFolderPath, "default");

				} else if (d [key] == true && key == "share") 
				{
					string shareID = AssetDatabase.CreateFolder (_lastselectedAssetPath, key);
				}
			}


		}
		Repaint();
	}

}


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TextMeshProModifier))]
public class TextMeshProModifierEditor : Editor
{
	TextMeshProModifier modifier = null;
	//SerializedProperty underlay;

	void OnEnable()
	{
		modifier = (TextMeshProModifier)target;
		//underlay = serializedObject.FindProperty("");
	}

	[MenuItem("EssentialShortcut/Select All Active TMPModifier")]
	private static void SelectAllModifier(MenuCommand command)
	{
		Transform[] ts = FindObjectsOfType<Transform>();
		List<GameObject> selection = new List<GameObject>();
		foreach (Transform t in ts)
		{
			Component[] cs = t.gameObject.GetComponents<TextMeshProModifier>();
			foreach (Component c in cs)
			{
				selection.Add(t.gameObject);
			}
		}
		Selection.objects = selection.ToArray();
	}

	[MenuItem("EssentialShortcut/Select All Active TMP")]
	private static void SelectAllTMP(MenuCommand command)
	{
		//GameObject[] gameObjects = Selection.gameObjects;
		//List<Transform> ts = new List<Transform>();
		//foreach (GameObject gameObject in gameObjects)
		//{
		//	ts.Add(gameObject.transform);
		//}
		Transform[] ts = FindObjectsOfType<Transform>();
		List<GameObject> selection = new List<GameObject>();
		foreach (Transform t in ts)
		{
			Component[] cs = t.gameObject.GetComponents<TMPro.TextMeshPro>();
			foreach (Component c in cs)
			{
				selection.Add(t.gameObject);
			}
		}
		Selection.objects = selection.ToArray();
	}

	[MenuItem("EssentialShortcut/Select All Active TMPSubMesh")]
	private static void SelectAllActiveTMPSubMesh(MenuCommand command)
	{
		//GameObject[] gameObjects = Selection.gameObjects;
		//List<Transform> ts = new List<Transform>();
		//foreach (GameObject gameObject in gameObjects)
		//{
		//	ts.Add(gameObject.transform);
		//}
		Transform[] ts = FindObjectsOfType<Transform>();
		List<GameObject> selection = new List<GameObject>();
		foreach (Transform t in ts)
		{
			Component[] cs = t.gameObject.GetComponents<TMPro.TMP_SubMesh>();
			foreach (Component c in cs)
			{
				selection.Add(t.gameObject);
			}
		}
		Selection.objects = selection.ToArray();
	}

	[MenuItem("EssentialShortcut/Copy TMP Alpha Color to MOC property")]
	private static void CopyTMPColorToMOC()
	{
		Transform[] ts = FindObjectsOfType<Transform>();
		List<GameObject> selection = new List<GameObject>();
		foreach (Transform t in ts)
		{
			TMPro.TextMeshPro cs = t.gameObject.GetComponent<TMPro.TextMeshPro>();
			MaterialOverrideController moc = t.gameObject.GetComponent<MaterialOverrideController>();
			if (cs == null || moc == null)
			{
				continue;
			}

			if(cs.color.a >= 1)
			{
				continue;
			}

			var match = moc.Properties.FirstOrDefault<MaterialOverrideProperty>(
				x => x.name.Equals("_FaceColor")
				);

			if (match != null)
			{
				match.type = MaterialOverrideProperty.PropType.Color;
				match.colorValue = cs.color;
			}
			else
			{
				moc.Properties.Add(new MaterialOverrideProperty()
				{
					name = "_FaceColor",
					type = MaterialOverrideProperty.PropType.Color,
					colorValue = cs.color
				});
			}

			selection.Add(t.gameObject);

		}
		Selection.objects = selection.ToArray();
	}

	public override void OnInspectorGUI()
	{
		//base.OnInspectorGUI();
		serializedObject.Update();

		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Underlay", EditorStyles.boldLabel, GUILayout.MaxWidth(70));
		modifier.enableUnderlay = EditorGUILayout.Toggle(modifier.enableUnderlay);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginVertical(EditorStyles.textArea);
		if (modifier.enableUnderlay)
			ModifierUnderlayGUI();
		EditorGUILayout.EndVertical();

		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Curve", EditorStyles.boldLabel, GUILayout.MaxWidth(70));
		modifier.enableCurve = EditorGUILayout.Toggle(modifier.enableCurve);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginVertical(EditorStyles.textArea);
		if (modifier.enableCurve)
			ModifierCurveGUI();
		EditorGUILayout.EndVertical();

		EditorGUILayout.Space();
		if (GUILayout.Button("Bake Result (you can remove the script after baked.)"))
		{
			modifier.BakeResult();
		}

		/*if (GUILayout.Button("Remove Current Bake Result)"))
        {
            modifier.RemoveCurrentBakeResult();
        }*/

		serializedObject.ApplyModifiedProperties();

	}

	private int selectedTemplate = 0;	
	private int last_selectedTemplate = 0;
	// Dictionary<string, TextMeshProModifierAttributes> TemplateMap = new Dictionary<string, TextMeshProModifierAttributes>
	// {
	//     {"Option1", } "Option2", "Option3", 
	// };

	private void ModifierUnderlayGUI()
	{
		EditorGUILayout.Space();


		// selectedTemplate = EditorGUILayout.Popup("Use Template", selectedTemplate, TemplateMap.Keys);
		if(selectedTemplate > 0 || selectedTemplate != last_selectedTemplate){

		}

		List<TextMeshProModifier.ModifierUnderlay> underlay;
		if (modifier.underlay != null)
			underlay = modifier.underlay.ToList();
		else
			underlay = new List<TextMeshProModifier.ModifierUnderlay>();
		//if (modifier.underlay == null)
		//    modifier.underlay = new TextMeshProModifier.ModifierUnderlay[0];

		if (GUILayout.Button("Add Underlay", GUILayout.MaxWidth(200)))
		{
			underlay.Add(new TextMeshProModifier.ModifierUnderlay(modifier));
		}

		//var numOfUnderlay = EditorGUILayout.IntField("Number of Underlay", underlay.Count);
		//if (numOfUnderlay != underlay.Count)
		//{
		//    Debug.Log("Updated underlay number: " + numOfUnderlay);
		//    if (numOfUnderlay > underlay.Count)
		//    {
		//    }

		//modifier.ClearUnderlayModifier();
		//modifier.underlay = new TextMeshProModifier.ModifierUnderlay[numOfUnderlay];
		//Debug.Log(modifier);
		//for (int a = 0; a < numofunderlay; a++)
		//{
		//    modifier.underlay[a] = new textmeshpromodifier.modifierunderlay(modifier, a);
		//}
		//modifier.ApplyUnderlayModifier();

		//}

		for (int a = 0; a < underlay.Count; a++)
		{
			var item = underlay[a];
			item.modifier = modifier;

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(string.Format("Underlay {0}:", (a + 1)), EditorStyles.boldLabel);
			if (GUILayout.Button("U", GUILayout.MaxWidth(30)))
			{
				if (a != 0)
				{
					underlay.Remove(item);
					underlay.Insert(a - 1, item);
				}
			}
			if (GUILayout.Button("D", GUILayout.MaxWidth(30)))
			{
				if (a != underlay.Count - 1)
				{
					underlay.Remove(item);
					underlay.Insert(a + 1, item);
				}
			}
			if (GUILayout.Button("Remove", GUILayout.MaxWidth(80)))
			{
				underlay.Remove(item);
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Color", GUILayout.MaxWidth(60));
			item.color = EditorGUILayout.ColorField(item.color);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Offset X", GUILayout.MaxWidth(60));
			item.offsetX = EditorGUILayout.FloatField(item.offsetX);
			EditorGUILayout.LabelField("Offset Y", GUILayout.MaxWidth(60));
			item.offsetY = EditorGUILayout.FloatField(item.offsetY);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Softness", GUILayout.MaxWidth(60));
			item.softness = EditorGUILayout.Slider(item.softness, 0, 1, GUILayout.MinWidth(100));
			EditorGUILayout.LabelField("Dilate", GUILayout.MaxWidth(60));
			item.dilate = EditorGUILayout.Slider(item.dilate, 0, 1f);
			EditorGUILayout.EndHorizontal();

		}

		// update the order
		for (int a = 0; a < underlay.Count; a++)
		{
			var item = underlay[a];
			item.order = a;
		}

		modifier.underlay = underlay.ToArray();

		//EditorGUILayout.
	}

	private void ModifierCurveGUI()
	{
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("This feature is not supported yet.", EditorStyles.boldLabel);
	}

	private void UnderlayItem()
	{

	}

}



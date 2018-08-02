using UnityEngine;
using UnityEditor;
using System.Collections;
using TMPro;
using System.Collections;
public class MenuItems
{
	[MenuItem("CONTEXT/TextMeshPro/Setup")]
	private static void Setup(MenuCommand menuCommand)
	{
		var tmp = menuCommand.context as TextMeshPro;

		MaterialOverrideController moc = tmp.gameObject.GetComponent<MaterialOverrideController> ();
		if (moc == null) {
			moc = tmp.gameObject.AddComponent<MaterialOverrideController> ();
		}

		moc.RefreshMode = MaterialOverrideRefreshMode.EveryFrame;

		tmp.gameObject.GetComponent<RectTransform>().sizeDelta =  new Vector2 (500, 500);

		tmp.alignment = TextAlignmentOptions.Center;

		tmp.font = Resources.Load ("Localization/Fonts/en/BeachBarScriptBlack SDF") as TMP_FontAsset;
		tmp.isOrthographic = true;
		tmp.parseCtrlCharacters = true;
		tmp.richText = true;
		tmp.useMaxVisibleDescender = true;
	}
}
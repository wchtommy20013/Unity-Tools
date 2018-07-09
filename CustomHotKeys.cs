using UnityEngine;
using UnityEditor;
 
public class CustomHotKeys : MonoBehaviour
{

    [MenuItem("GameObject/ActiveToggle _a")]
    public static void ToggleActivationSelection()
    {
        foreach(GameObject go in Selection.gameObjects)
            go.SetActive(!go.activeSelf);
    }


    [MenuItem("GameObject/Apply Prefab Changes %#p")]
    public static void applyPrefabChanges()
    {
        var obj = Selection.activeGameObject;
        if(obj!=null)
        {
            var prefab_root = PrefabUtility.FindPrefabRoot(obj);
            var prefab_src = PrefabUtility.GetPrefabParent(prefab_root);
            if(prefab_src!=null)
            {
                PrefabUtility.ReplacePrefab(prefab_root, prefab_src,  ReplacePrefabOptions.ConnectToPrefab);
                Debug.Log("Updating prefab : "+AssetDatabase.GetAssetPath(prefab_src));
            }
            else
            {
                Debug.Log("Selected has no prefab");
            }
        }
        else
        {
            Debug.Log("Nothing selected");
        }
    }
}

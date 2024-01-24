using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ReplaceWithPrefab : EditorWindow
{
    private GameObject prefabToReplaceWith;

    [MenuItem("Tools/Replace Selected Objects with Prefab")]
    static void Init()
    {
        ReplaceWithPrefab window = (ReplaceWithPrefab)EditorWindow.GetWindow(typeof(ReplaceWithPrefab));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Replace Selected Objects with Prefab", EditorStyles.boldLabel);

        prefabToReplaceWith = EditorGUILayout.ObjectField("Prefab to Replace With:", prefabToReplaceWith, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Replace"))
        {
            ReplaceObjectsWithPrefab();
        }
    }

    private void ReplaceObjectsWithPrefab()
    {
        if (prefabToReplaceWith == null)
        {
            EditorUtility.DisplayDialog("Error", "Please select a prefab to replace with.", "OK");
            return;
        }

        // Get the selected objects
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog("Error", "Please select one or more objects to replace.", "OK");
            return;
        }

        foreach (GameObject selectedObject in selectedObjects)
        {
            GameObject newObject = PrefabUtility.InstantiatePrefab(prefabToReplaceWith) as GameObject;
            newObject.transform.position = selectedObject.transform.position;
            newObject.transform.rotation = selectedObject.transform.rotation;
            if (selectedObject.transform.parent != null)
            {
                newObject.transform.SetParent(selectedObject.transform.parent);
            }

            DestroyImmediate(selectedObject);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Replaced " + selectedObjects.Length + " objects with " + prefabToReplaceWith.name);
    }
}
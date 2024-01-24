using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Collections;


public class SnapToInt : EditorWindow
{
    [MenuItem("Tools/SnapToInt")]
    static void RoundSelectedObjectCoordinates()
    {
        Transform[] selectedObjects = Selection.transforms;
        if (selectedObjects != null && selectedObjects.Length > 0)
        {
            foreach (Transform selectedObject in selectedObjects)
            {
                Undo.RecordObject(selectedObject, "Round Coordinates");
                Vector3 position = selectedObject.position;
                position.x = Mathf.Round(position.x);
                position.y = Mathf.Round(position.y);
                position.z = Mathf.Round(position.z) ;
                selectedObject.position = position;
            }
        }
    }
}
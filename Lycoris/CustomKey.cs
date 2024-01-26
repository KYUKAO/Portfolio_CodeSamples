using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomKey
{
    [MenuItem("Custom¿ì½Ý¼ü/¿ìËÙ _`")]
    static void SpeedGameCommand()
    {
        var player = Resources.FindObjectsOfTypeAll<PlayerController>()[0].gameObject;
        UnityEditor.EditorGUIUtility.PingObject(player);
        Selection.activeGameObject = player;
        SceneView.lastActiveSceneView.FrameSelected();
    }
}

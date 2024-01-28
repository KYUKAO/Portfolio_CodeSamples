using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected GameObject cameraObj;
    protected Camera SmallCamera;
    protected void Awake()
    {
        cameraObj = GameObject.FindGameObjectsWithTag("SmallCamera")[0];
        SmallCamera = cameraObj.GetComponent<Camera>();
    }
    protected void AddOnce<T>(List<T> team, T member)
    {
        if (!team.Contains(member))
        {
            team.Add(member);
        }
    }

    protected void RemoveOnce<T>(List<T> team, T member)
    {
        if (team.Contains(member))
        {
            team.Remove(member);
        }
    }
    public static bool IsInCameraView(Transform pos, Camera camera)
    {
        Vector3 targetObjViewportCoord = camera.WorldToViewportPoint(pos.position);
        if (targetObjViewportCoord.x > 0 && targetObjViewportCoord.x < 1 && targetObjViewportCoord.y > 0f 
            && targetObjViewportCoord.y < 1 && targetObjViewportCoord.z > camera.nearClipPlane && targetObjViewportCoord.z < camera.farClipPlane)
            return true;
        return false;
    }
}

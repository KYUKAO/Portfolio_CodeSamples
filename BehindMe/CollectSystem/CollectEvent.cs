using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectEvent : MonoBehaviour
{
    public GameObject Collectable;
    public Transform HandPoint;
    public CollectEvent (GameObject collectable,Transform handPoint)
    {
        Collectable = collectable;
        HandPoint = handPoint;
    }
}
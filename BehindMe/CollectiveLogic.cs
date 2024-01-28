using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiveLogic : MonoBehaviour
{
    public static int BraceletCount = 0;
    GameObject OriginObject;
    private void Start()
    {
        OriginObject = Resources.Load("Collectives/Bracelet") as GameObject;
        for (int i = 0; i< BraceletCount; i++)
        {
           var gameObject= Instantiate(OriginObject, this.transform.position, this.transform.rotation);
            gameObject.transform.position = this.transform.GetChild(i + 1).position;
            gameObject.GetComponent<CollectableComponent>().canBeCollected = false;
        }
    }
}

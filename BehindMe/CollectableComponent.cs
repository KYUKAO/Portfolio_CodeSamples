using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableComponent : MonoBehaviour
{
    public bool canBeCollected = true;
    public Transform HandPoint;
    GameObject MyBracelet;
    public void Collect()
    {if(canBeCollected)
        {
            MyBracelet = Resources.Load("SingleBracelet") as GameObject;
            Evently.Instance.Publish(new CollectEvent(this.gameObject,this.HandPoint));
            var myBracelet = Instantiate(MyBracelet, HandPoint.transform.position, HandPoint.transform.rotation);
            myBracelet.transform.SetParent(HandPoint);
            myBracelet.transform.localPosition = Vector3.zero;
            myBracelet.transform.localEulerAngles = new Vector3(0, 0, 0);
            
        }
    }
}

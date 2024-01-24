using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionSystem : MonoBehaviour
{
    //One system-----One event----
    //Multiple systems------through event-----same component
    public void OnEnable()
    {
        Evently.Instance.Subscribe<CollectionEvent>(OnCollected);
    }
    public void OnDisable()
    {
        Evently.Instance.Unsubscribe<CollectionEvent>(OnCollected);
    }
    private void OnCollected(CollectionEvent evt)
    {
        CollectorComponent.CurrentNumOfCollection+= evt.numToPlus;
        Destroy(evt.Collectable.gameObject);
    }
}

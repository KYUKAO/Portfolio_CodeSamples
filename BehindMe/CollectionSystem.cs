using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionSystem : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip collectedSound;
    public void OnEnable()
    {
        //CollectiveLogic.BraceletCount = 0;
        Evently.Instance.Subscribe<CollectEvent>(OnCollected);
    }
    public void OnDisable()
    {
        Evently.Instance.Unsubscribe<CollectEvent>(OnCollected);
    }
    private void OnCollected(CollectEvent evt)
    {
            CollectiveLogic.BraceletCount++;
        //collectedSound = Resources.Load("Audio/CollectedSound") as AudioClip;
        //audioSource.clip = collectedSound;
        //audioSource.Play();
        Destroy(evt.Collectable.gameObject);
    }
}

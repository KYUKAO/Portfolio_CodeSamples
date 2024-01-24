using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionEvent
{
    public CollactableComponent Collectable;
    public CollectorComponent Collector;
    public int numToPlus;
    //constructor
    public CollectionEvent(CollactableComponent collectable,int num)
    {
        Collectable = collectable;
        numToPlus = num;
    }
}
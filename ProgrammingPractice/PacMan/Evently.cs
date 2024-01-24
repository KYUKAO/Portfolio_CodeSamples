using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Evently//not set to monobehavior
{
    //private static eventManagerInstance
    //public static Instance =>private member
    //Dictionary that maps Types to Delegates
    //public,generic Subscribe method that allows us to subscribe to events.
    //public ,generic Publish method that allows us to subscribe to events

    //Observer Pattern
    //Publisher:     When something happens ,a publisher publish/fire the event.
    //Subscribers:  Listen to events
    private static Evently instance;
    //if it's null, assign it to a new Evently()
    //Otherwise,just return eventManagerInstance(the right side never gets evaluated)
    public static Evently Instance => instance ??= new Evently();

    private readonly Dictionary<Type, Delegate> delegates = new Dictionary<Type, Delegate>();//why readonly?

    public void Subscribe<T>(Action<T> del)
    {
        //Add an Action to the a key of the dictionary(which is the type of the Action)
        if (delegates.ContainsKey(typeof(T)))
        {
            delegates[typeof(T)] = Delegate.Combine(delegates[typeof(T)], del);//by using"typeof"gets the name of a given type
        }
        else
        {
            delegates[typeof(T)] = del;
        }
    }
    public void Unsubscribe<T>(Action<T> del)
    {
        //If there's already not this type
        if (delegates.ContainsKey(typeof(T))) return;
        //otherwise remove a delegate from the key of type
        var tempDelegate = Delegate.Remove(delegates[typeof(T)], del);
        //After removing ,if there's null in the key, remove the key.
        if (tempDelegate == null)
        {
            delegates.Remove(typeof(T));
        }
        else
        {
            delegates[typeof(T)] = tempDelegate;
        }
    }
    public void Publish<T>(T e)
    {
        //For safety incase input is null?
        if (e == null)
        {
            Debug.Log($"invalid event arg:{typeof(T)}");
            return;
        }
        //If there is this key in "delegates",then do the Actions.
        if (delegates.ContainsKey(e.GetType()))
        {
            delegates[typeof(T)].DynamicInvoke(e);//DynamicInboke needs a  parameter
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;


    public class Evently
    {
        // private static eventManagerInstance
        // public static Instance => private member
        // Dictionary that maps Types to Delegates
        // public, generic Subscribe method that allows us to subscribe to events
        // public, generic unSubscribe method that allows us to subscribe to events
        // public, generic Publish method that allows us to subscribe to events

        private static Evently _instance;
        // Null coalescing assignment...
        // If it's null, assign it to a new NewEventSystem()
        // Otherwise, just return _instance (the right side never gets evaluated)
        // tl;dr just some fancy c# syntax..
        public static Evently Instance => _instance ??= new Evently();

        private readonly Dictionary<Type, Delegate> _delegates = new Dictionary<Type, Delegate>();

        public void Subscribe<T>(Action<T> del)
        {
            if (_delegates.ContainsKey(typeof(T)))
            {
                _delegates[typeof(T)] = Delegate.Combine(_delegates[typeof(T)], del);
            }
            else
            {
                _delegates[typeof(T)] = del;
            }
        }

        public void Unsubscribe<T>(Action<T> del)
        {
            if (!_delegates.ContainsKey(typeof(T))) return;

            var currentDel = _delegates[typeof(T)] = Delegate.Remove(_delegates[typeof(T)], del);

            if (currentDel == null)
            {
                // Housekeeping, to keep our dictionary clean
                _delegates.Remove(typeof(T));
            }
            else
            {
                _delegates[typeof(T)] = currentDel;
            }
        }

        public void Publish<T>(T e)
        {
            if (e == null)
            {
                Debug.Log($"Invalid Event Arg: {typeof(T)}");
                return;
            }

            if (_delegates.ContainsKey(e.GetType()))
            {
                _delegates[typeof(T)]?.DynamicInvoke(e);
            }
        }
    }
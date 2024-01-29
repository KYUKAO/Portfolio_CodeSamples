using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T Instance = null;
    //Our public property of Type T
    //This is a pattern you see a lot in C#: private fields, with public properties
    public static T Instantce
    {//T has a getter and setter 
     //This is a public "Getter"
        get
        {
            if (Instance != null) return Instance;//We Only want ONE instance ,ever with a singleton,never more than ONE.
                                                  //Getter Checks to see if the private instance is null, if not, returns it.

            //if it is null ,it first checks the scene,and tries to grab it 
            Instance = FindObjectOfType<T>();
            //if still null after checking the scene,instantiate a New Game Object
            if (Instance == null)
            {
                Instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }
            DontDestroyOnLoad(Instance.gameObject);
            return Instance;
        }
    }
    public virtual void Awake()//
    {
        if (Instance != null) Destroy(gameObject);
    }
}
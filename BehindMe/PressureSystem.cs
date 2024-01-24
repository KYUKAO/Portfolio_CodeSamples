using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class PressureSystem : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip playerHurtSFX, playerPressureSFX;
    public static float currentPressure;
    public float MaxPressure;
    public float BleedRate;
    public float RecoverRate;
    float timer = 0;
    public Volume postprocessing;
    public GameObject dieVolume;
    private Animator postAnimator;
    private List<VolumeComponent> list;
    public static List<GameObject> ThreateningEnemies=new List<GameObject>();
    public float redStep;
    private GameObject damagedVFX;
    public enum State
    {
        Idle,
        Damaged,
        Bleeding,
        Recovering,
    }
   public  static State currentState;
    void Start()
    {
        dieVolume.GetComponent<Animator>().enabled = false;
        damagedVFX = this.transform.GetChild(0).gameObject;
        damagedVFX.SetActive(false);
        postAnimator = GetComponent<Animator>();
        ThreateningEnemies.Clear();

        //reset the hitVFX
        postprocessing.weight = 0;

        list = postprocessing.profile.components;
        currentPressure = 0f;
        currentState = State.Idle;
        //foreach (VolumeParameter item in list[1].parameters)
        //{
        //    Debug.Log(item.GetType() + " " + item.ToString());
        //}
        audioSource = this.GetComponent<AudioSource>();
        playerPressureSFX = Resources.Load("Audio/PlayerPressure") as AudioClip;
    }

    // Update is called once per frame
    void Update()
    {
        RedScreen_ver2();
        //RedScreen();
        TestInput();
        //StateChanger
        switch (currentState)
        {
            case State.Bleeding:
                Bleed();
                break;
            case State.Recovering:
                Recover();
                break;
        }
        //DamageState
        if (currentState == State.Damaged)//After the Player get damaged
        {
            DamagedVFX();
            timer = 0;
            currentState = State.Bleeding;
            VibrationSystem.Instance.StartProfileVibration(4);
            audioSource.clip = playerPressureSFX;
            audioSource.Play();
        }
        //If there's no threatening enemy, recover
        if (ThreateningEnemies.Count == 0 && currentState == State.Bleeding)
        {
            currentState = State.Recovering;
            audioSource.Stop();
            timer = 0;
        }
        //Death judgement
        if (currentPressure >= MaxPressure)
        {
            currentPressure = MaxPressure;
            Die();
        }
        //When Pressure is 0
        else if (currentPressure <0) 
        {
            currentPressure = 0;
        }
    }
    
    void Bleed()
    {
        timer += Time.deltaTime;
        currentPressure = timer * BleedRate + currentPressure;
    }

    void Recover()
    {
        timer += Time.deltaTime;
        if (currentPressure <= 0)
        {
            timer = 0; 
            currentState = State.Idle;
        }
        currentPressure = currentPressure - RecoverRate * timer * timer;
    }

    void Die()
    {
        currentPressure = 0;
        dieVolume.GetComponent<Animator>().enabled=true;
        Debug.Log("Dead");
    }
    
    void RedScreen()
    {
        Debug.Log(currentPressure);
        if (currentPressure<MaxPressure)
        {
            if(currentPressure%redStep==0)
                list[1].parameters[3].SetValue(new FloatParameter(currentPressure * 1.0f / MaxPressure));
           // redVFX.weight=currentPressure*1.0f/MaxPressure*0.63f;
            list[0].parameters[0].SetValue(new FloatParameter(currentPressure * 1.0f / MaxPressure * 2.4f));
        }
    }
    void RedScreen_ver2()
    {
        Debug.Log(currentPressure);
        if (currentPressure < MaxPressure)
        {
            if (currentPressure % redStep == 0)
            {
                postprocessing.weight = currentPressure * 1.0f / MaxPressure;
            }
            if(currentPressure>=MaxPressure/2)
            {
                VibrationSystem.Instance.StartProfileVibration(6);
            }
        }
    }
    void DamagedVFX()
    {
        damagedVFX.SetActive(true);
        Invoke("CloseDamagedVFX", 0.2f);
    }
    void CloseDamagedVFX()
    {
        damagedVFX.SetActive(false);
    }
    public void BeHit()
    {
        postAnimator.SetTrigger("BeHit");
    }
    
    void TestInput()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentPressure += 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentPressure = 100;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentState = State.Damaged;
        }
    }
}

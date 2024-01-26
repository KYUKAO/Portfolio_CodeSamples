using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GhostFireControl : MonoBehaviour
{
    public enum GhostFireTypes
    {
        GhostFire1,
        GhostFire2,
        GhostFire3
    };
    public GhostFireTypes ghostFireTypes;
    ParticleSystem particle;
    public float zDir;
    public float yDir;
    public GameObject PlayerSoulPos;
    public SoulMoveComponent SoulMoveComponent;
    public BodyMoveComponent BodyMoveComponent;
    public GhostFireControl attractGhostFire;
    public GhostFireControl lastAttractGhostFire;
    private bool canKeepAttrackPlayer;
    private bool hasBeenSelect;

    private float Speed = 1f;

    public Color Color1;
    public Color Color2;
    public float MaxSpeed;
    Material wabalabadubdub;
    [HideInInspector]
    public Material mat;

    private bool resetGhostFireOnce;
    private bool doOnceGhostFireSelected;
    //public bool doOncehostFireSuckedSFX;
    private bool canActiveGhostFire;
    float alphaOfFire;

    public UnityEvent ghostFireEndEvent;

    //AudioSource audioSource;
    //AudioClip fireActiveSFX;
    //AudioClip fireIdleSFX;
    //AudioClip fireDieSFX;
    //AudioClip testSFX;

    void Start()
    {
        /*
        audioSource = this.GetComponent<AudioSource>();
        fireActiveSFX = Resources.Load("Audio/GhostFire/GhostFire_Active") as AudioClip;
        fireIdleSFX = Resources.Load("Audio/GhostFire/GhostFire_Idle") as AudioClip;
        fireDieSFX = Resources.Load("Audio/GhostFire/GhostFire_Out") as AudioClip;
        audioSource.clip = fireIdleSFX;
        testSFX = Resources.Load("Audio/GhostFire/SuckGhostFire") as AudioClip;
        audioSource.Play();
        audioSource.loop = true;
        */

        canKeepAttrackPlayer = false;
        doOnceGhostFireSelected = true;
        //doOncehostFireSuckedSFX = true;
        BodyMoveComponent = FindObjectOfType<BodyMoveComponent>();
        wabalabadubdub=this.GetComponent<ParticleSystemRenderer>().trailMaterial;
        mat = new Material(wabalabadubdub);
        this.GetComponent<ParticleSystemRenderer>().trailMaterial = mat;
        mat.SetFloat("_Speed", Speed);
        mat.SetColor("_Color", Color1);
        alphaOfFire = mat.GetFloat("_Alpha");
        particle = this.GetComponent<ParticleSystem>();
        //particle.Stop();
        
        AkSoundEngine.SetRTPCValue("GhostFire_RTPC", 0);
    }
    void Update()
    {
        AttrackGhostFire();
        ChangeFireDirection(zDir, yDir);
       
        //if (Input.GetKey(KeyCode.Tab))
        //{
        //    //ActiveGhostFire();
        //    //ResetGhostFireValue();
        //    //ResetGhostFireValue();
        //    canKeepAttrackPlayer = true;
        //}
        if (BodyMoveComponent&&!BodyMoveComponent.isAlive)
        {
            resetGhostFireOnce = true;
            if (canActiveGhostFire)
            {
                ActiveGhostFire();
            }
        }
        else
        {
            //if (resetGhostFireOnce)
            //{
            //    canKeepAttrackPlayer = false;
            ResetGhostFireValue();
            //if (canKeepAttrackPlayer)
            //{
            DeActiveGhostFire();
            canKeepAttrackPlayer = false;
            //}
            //    resetGhostFireOnce = false;
            //}
        }
        //if (canKeepAttrackPlayer == false)
        //{
        //    ResetGhostFireValue();
        //}
        //GetPlayerSoulPosAndScript();
        if (PlayerSoulPos == null || SoulMoveComponent == null)
        {
            //Debug.Log("Did not get Player Soul Component");
            return;
        }
        if (!hasBeenSelect)
        {
            ResetGhostFireValue();
            return;
        }
        ChangeGhostFireValues();
        //SFX
        //if (!(audioSource.isPlaying && audioSource.clip == fireActiveSFX)&&doOnceGhostFireSelected)
        //{
        //    audioSource.clip = fireActiveSFX;
        //    audioSource.Play();
        //    audioSource.loop = true;
        //    doOnceGhostFireSelected = false;
        //}
        
    }
    public void ChangeFireDirection(float zDir, float yDir)
    {
        var fo = particle.forceOverLifetime;
        AnimationCurve curve = new AnimationCurve();
        //yDir=0---Up  yDir=10----Down  zDir=5-----Right  zDir=-5----Left
        fo.z = new ParticleSystem.MinMaxCurve(zDir, zDir);
        fo.y = new ParticleSystem.MinMaxCurve(yDir, yDir);
    }
    void ChangeGhostFireValues()
    {
        var distance = Vector3.Distance(PlayerSoulPos.transform.position, transform.position);
        var y = Mathf.Abs(transform.position.y - PlayerSoulPos.transform.position.y);
        var distaceY = (transform.position.y - PlayerSoulPos.transform.position.y);
        var x = (transform.position.z - PlayerSoulPos.transform.position.z);
        var angle = Mathf.Asin(y / distance);

        if (ghostFireTypes == GhostFireTypes.GhostFire1)
        {
            if (SoulMoveComponent.attractObj && SoulMoveComponent.attractObj.isAttracted)
            {
                if (!BodyMoveComponent.isAlive)
                {
                    yDir = -Mathf.Sin(angle) * 5 + 5;
                    zDir = -Mathf.Cos(angle) * 5;
                    if (x > 0)
                    {
                        zDir = -zDir;
                    }
                    if (distaceY > 0)
                    {
                        yDir = 10 - yDir;
                    }
                    DieFire();
                }
                //audioSource.loop = false;
            }
            else
            {
                ResetGhostFireValue();
            }
        }
        else if (ghostFireTypes == GhostFireTypes.GhostFire2)
        {
            if (SoulMoveComponent.attractObj && SoulMoveComponent.attractObj.isAttracted)
            {
                if (!BodyMoveComponent.isAlive)
                {
                    yDir = -Mathf.Sin(angle) * 5 + 5;
                    zDir = -Mathf.Cos(angle) * 5;
                    if (x > 0)
                    {
                        zDir = -zDir;
                    }
                    if (distaceY > 0)
                    {
                        yDir = 10 - yDir;
                    }
                }
            }
            else
            {
                ResetGhostFireValue();
            }
        }
        else if (ghostFireTypes == GhostFireTypes.GhostFire3)
        {
            if (SoulMoveComponent.attractObj && SoulMoveComponent.attractObj.isAttracted)
            {
                canKeepAttrackPlayer = true;
            }
            if (!canKeepAttrackPlayer) return;
            if (!BodyMoveComponent.isAlive)
            {
                yDir = -Mathf.Sin(angle) * 5 + 5;
                zDir = -Mathf.Cos(angle) * 5;
                if (x > 0)
                {
                    zDir = -zDir;
                }
                if (distaceY > 0)
                {
                    yDir = 10 - yDir;
                }
            }
        }
    }
    //void GetPlayerSoulPosAndScript()
    //{
    //    if (PlayerSoulPos == null || SoulMoveComponent == null)
    //    {
    //        PlayerSoulPos = FindObjectOfType<SoulComponent>().gameObject;
    //        SoulMoveComponent = FindObjectOfType<SoulMoveComponent>();
    //    }
    //}
    void ResetGhostFireValue()
    {
        yDir = 0f;
        zDir = 0f;
    }
    void ActiveGhostFire()
    {
        Speed = 4f;
        mat.SetFloat("_Speed", Speed);
        mat.SetColor("_Color", Color2);
        AkSoundEngine.SetRTPCValue("GhostFire_RTPC", 50);
    }
    void DeActiveGhostFire()
    {
        Speed = 1f;
        mat.SetFloat("_Speed", Speed);
        mat.SetColor("_Color", Color1);
        /*
        if (!(audioSource.isPlaying && audioSource.clip == fireIdleSFX))
        {
            audioSource.clip = fireIdleSFX;
            audioSource.Play();
            audioSource.loop = true;
        }
        */
        
        doOnceGhostFireSelected = true;
        //doOncehostFireSuckedSFX = true;
        AkSoundEngine.SetRTPCValue("GhostFire_RTPC", 0);
    }
    private AttractableComponent FindAttracableObj()
    {
        Collider[] objs = Physics.OverlapSphere(transform.position, SoulMoveComponent.sphereRadius);
        List<AttractableComponent> attractableObjs = new List<AttractableComponent>();
        foreach (var obj in objs)
        {
            if (obj.GetComponent<AttractableComponent>())
            {
                //add 
                attractableObjs.Add(obj.GetComponent<AttractableComponent>());
            }
        }
        if (attractableObjs.Count <= 0)
        {
            return null;
        }
        AttractableComponent nearestAttractableObj = attractableObjs[0];
        foreach (var attractableObj in attractableObjs)
        {
            if (DistanceBetweenSoul(attractableObj) < DistanceBetweenSoul(nearestAttractableObj))
            {
                nearestAttractableObj = attractableObj;
            }
        }
        return nearestAttractableObj;
    }
    private float DistanceBetweenSoul(AttractableComponent obj)
    {
        return Vector3.Distance(obj.transform.position, transform.position);
    }
    void AttrackGhostFire()
    {
        if (SoulMoveComponent && SoulMoveComponent.attractObj)
            attractGhostFire = SoulMoveComponent.attractObj.GetComponentInChildren<GhostFireControl>();
        else
            attractGhostFire = null;
        if (lastAttractGhostFire != attractGhostFire)
        {
            if (lastAttractGhostFire != null)
            {
                lastAttractGhostFire.hasBeenSelect = false;
                //if (canKeepAttrackPlayer)
                //{
                canActiveGhostFire = false;
                DeActiveGhostFire();
                //}
                //lastAttractGhostFire.canKeepAttrackPlayer = false;
            }
            if (attractGhostFire != null)
            {
                attractGhostFire.hasBeenSelect = true;
                if (hasBeenSelect)
                {
                    canActiveGhostFire = true;
                    //ActiveGhostFire();
                    //canKeepAttrackPlayer = true;
                }
                //attractGhostFire.canKeepAttrackPlayer = true;
            }
        }
        lastAttractGhostFire = attractGhostFire;
    }

    //For spell, make ghost fire die
    void DieFire()
    {
        /*
        if (!(audioSource.isPlaying && audioSource.clip == fireDieSFX))
        {
            audioSource.clip = fireDieSFX;
            audioSource.Play();
            audioSource.loop = false;
        }
        */

        if (alphaOfFire>0f)
        {
            alphaOfFire -=3*Time.deltaTime;
            mat.SetFloat("_Alpha", alphaOfFire);
            //var main = particle.main;
            //main.startLifetime = new ParticleSystem.MinMaxCurve();
        }
        else
        {
            ghostFireEndEvent.Invoke();
            gameObject.SetActive(false);
        }
    }

    void PlayGhostFire()
    {
        particle.Play();
    }
}

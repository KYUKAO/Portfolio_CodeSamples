using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShadowManControl : EnemyBase
{
   Camera MainCamera;
    float time;
    public float IntervalTime;
    bool isMoving = true;
    MeshRenderer mr;
    Rigidbody rb;
    float timer2;
    public float pressureTime;

    float rand1;
    float rand2;
    public enum MovementState
    {
        Randomly,
        BetweenCloset,
        Scripted
    }
    public MovementState CurrentMovementState;

    [Header("当在Random状态时：")]
    public float MinDistance;

    public Vector3 Centre;
    public float Radius;

    private Transform point1;
    private Transform point2;
    private Transform point3;
    private Transform point4;
    private List<Transform> points;

    public float Damage;
    [Header("当在BetweenCloset状态时 ： ")]
    public float TransportDistance;

    bool isCollided = false;


    AudioSource audioSource;
    AudioClip appearSFX, disappearSFX;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        appearSFX= Resources.Load("Audio/ShadowMan_Appear") as AudioClip;
        disappearSFX = Resources.Load("Audio/Example") as AudioClip;
        MainCamera = Camera.main;
        time = 0;
        mr = this.GetComponent<MeshRenderer>();
        mr.enabled = false;
        rb = this.GetComponent<Rigidbody>();
        //ShadowMan 身上有四个点，每个点都要检测
        point1 = this.transform.GetChild(0);
        point2 = this.transform.GetChild(1);
        point3 = this.transform.GetChild(2);
        point4 = this.transform.GetChild(3);
        points = new List<Transform>() { point1, point2, point3, point4 };
    }

    void FixedUpdate()
    {
        if (isMoving)
        {//每过一段时间移动一次
            time += Time.deltaTime;
            if (time >= IntervalTime)
            {
                Move();
                timer2 = 0f;
            }
        }
        else
        {
            if (CurrentMovementState==MovementState.Randomly)
            {
                timer2 += Time.deltaTime;
                if (timer2 >= pressureTime)
                {
                    timer2 = 0f;
                    this.transform.position = Camera.main.gameObject.transform.position + Camera.main.gameObject.transform.forward * 2.6f;
                    PressureSystem.currentPressure += Damage;
                    PressureSystem.currentState = PressureSystem.State.Damaged;
                    AddOnce<GameObject>(PressureSystem.ThreateningEnemies, this.gameObject);
                }
            }
        }
        //四个点中任意一个点在MainCamera范围中，且不在SmallCamera范围中，且距离玩家一定距离
        if ((!IsInCameraView(point1, SmallCamera) &&! IsInCameraView(point2, SmallCamera) && !IsInCameraView(point3, SmallCamera) &&! IsInCameraView(point4, SmallCamera) &&
            (Vector3.Distance(this.transform.position, MainCamera.transform.position) >= MinDistance)))
        {
            //如果符合，ShadowMan停止运动且显现MeshRenderer
            //Debug.Log("isInView" );
            rb.MovePosition(transform.position);
            if(isMoving && CurrentMovementState == MovementState.Randomly)
            {
                VibrationSystem.Instance.StartProfileVibration(2);
                audioSource.Stop();
                audioSource.clip = appearSFX;
                audioSource.Play();
            }
            isMoving = false;
            time = 0;
            mr.enabled = true;
        }
        //如果ShadowMan在小相机范围内或者离玩家太近，立刻更换到另一个位置
        else
        {
            if(!isMoving&&CurrentMovementState!=MovementState.BetweenCloset)
            {
                VibrationSystem.Instance.StartProfileVibration(4);
                audioSource.Stop();
                audioSource.clip = disappearSFX;
                audioSource.Play();
            }
            RemoveOnce<GameObject>(PressureSystem.ThreateningEnemies, this.gameObject);
            isMoving = true;
            mr.enabled = false;
            return;
        }
    }

    //检测某个点是否在某个相机范围内
    //防止ShadowMan与柜子重合

    private void OnCollisionEnter(Collision collision)
    {
        mr.enabled = false;
        Move();
        Debug.Log("Collided");
    }

    private void Move()
    {//移动方式与开发者选择的状态有关
        switch (CurrentMovementState)
        {
            case MovementState.Randomly:
                rand1 = Random.Range(-Radius, Radius);
                rand2 = Random.Range(-Radius, Radius);
                rb.MovePosition(new Vector3(Centre.x + rand1, Centre.y, Centre.z + rand2));
                return;
            //ShadowMan在玩家的两侧固定距离瞬移
            case MovementState.BetweenCloset:
                mr.enabled = false;
                return;
            case MovementState.Scripted:
                Level1Events.isNight = true;
                return;
        }
    }
}

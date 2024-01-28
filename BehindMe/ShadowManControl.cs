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

    [Header("����Random״̬ʱ��")]
    public float MinDistance;

    public Vector3 Centre;
    public float Radius;

    private Transform point1;
    private Transform point2;
    private Transform point3;
    private Transform point4;
    private List<Transform> points;

    public float Damage;
    [Header("����BetweenCloset״̬ʱ �� ")]
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
        //ShadowMan �������ĸ��㣬ÿ���㶼Ҫ���
        point1 = this.transform.GetChild(0);
        point2 = this.transform.GetChild(1);
        point3 = this.transform.GetChild(2);
        point4 = this.transform.GetChild(3);
        points = new List<Transform>() { point1, point2, point3, point4 };
    }

    void FixedUpdate()
    {
        if (isMoving)
        {//ÿ��һ��ʱ���ƶ�һ��
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
        //�ĸ���������һ������MainCamera��Χ�У��Ҳ���SmallCamera��Χ�У��Ҿ������һ������
        if ((!IsInCameraView(point1, SmallCamera) &&! IsInCameraView(point2, SmallCamera) && !IsInCameraView(point3, SmallCamera) &&! IsInCameraView(point4, SmallCamera) &&
            (Vector3.Distance(this.transform.position, MainCamera.transform.position) >= MinDistance)))
        {
            //������ϣ�ShadowManֹͣ�˶�������MeshRenderer
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
        //���ShadowMan��С�����Χ�ڻ��������̫�������̸�������һ��λ��
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

    //���ĳ�����Ƿ���ĳ�������Χ��
    //��ֹShadowMan������غ�

    private void OnCollisionEnter(Collision collision)
    {
        mr.enabled = false;
        Move();
        Debug.Log("Collided");
    }

    private void Move()
    {//�ƶ���ʽ�뿪����ѡ���״̬�й�
        switch (CurrentMovementState)
        {
            case MovementState.Randomly:
                rand1 = Random.Range(-Radius, Radius);
                rand2 = Random.Range(-Radius, Radius);
                rb.MovePosition(new Vector3(Centre.x + rand1, Centre.y, Centre.z + rand2));
                return;
            //ShadowMan����ҵ�����̶�����˲��
            case MovementState.BetweenCloset:
                mr.enabled = false;
                return;
            case MovementState.Scripted:
                Level1Events.isNight = true;
                return;
        }
    }
}

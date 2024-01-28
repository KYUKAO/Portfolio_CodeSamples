using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class EyeClockControl : EnemyBase
{
    #region Variables
    // Start is called before the first frame update
    public float RayLength;
    private Animator anim;
    GameObject pupil;
    //  Light redLight;
    GameObject EyeLight;
    public GameObject Player;
    public GameObject Body;
    GameObject playerHeadset;
    GameObject playerLeftHand;
    GameObject playerRightHand;
    Vector3 lastHeadPos;
    Vector3 lastLeftHandPos;
    Vector3 lastRightHandPos;
    Vector3 lastBodyPos;
    Quaternion lastHeadRotate;
    bool isHeadMove=false;
    bool isLeftHandMove = false;
    bool isRightHandMove = false;
    bool isHeadRotate = false;
    bool isBodyMove = false;
    public float HeadLimitDistance;
    public float HandLimitDistance;
    public float HeadLimitRotation;
    public float BodyLimitDistance;
    bool canDamage = true; //this bool guarantees that the eye can only take damage once each turn.
    public GameObject LeftJoint;
    public GameObject RightJoint;
    float leftJointAngle;
    float rightJointAngle;
    public float CabinetAngleLimit;
    public float Damage;
    float firstTimeTimer = 0f;
    public float InvincibleTime;
    AudioSource audioSource;
    AudioClip countingSFX, attackingSFX;
#endregion
    public enum State
    {
        Counting,
        Looking,
    }
    public State currentState;
    void Start()
    {
        leftJointAngle = LeftJoint.transform.localEulerAngles.y;
        rightJointAngle = RightJoint.transform.localEulerAngles.y;

        playerHeadset = Player.transform.GetChild(0).gameObject;
        playerLeftHand = Player.transform.GetChild(1).gameObject;
        playerRightHand = Player.transform.GetChild(2).gameObject;

        pupil = transform.GetChild(1).gameObject;
        // redLight = transform.GetChild(2).gameObject.GetComponent<Light>();
        EyeLight = transform.GetChild(2).gameObject;
        anim = pupil.GetComponent<Animator>();
        EyeLight.SetActive(false);
        audioSource = this.GetComponent<AudioSource>();
        countingSFX = Resources.Load("Audio/EyeClock_Counting") as AudioClip;
        attackingSFX = Resources.Load("Audio/EyeClock_Attack") as AudioClip;
        // redLight.enabled = false;
    }
    void Update()
    {
        //��ȡ���ȹ��ӵĽǶ�
        leftJointAngle = GetInspectorRotationValueMethod(LeftJoint.transform);
        rightJointAngle = GetInspectorRotationValueMethod(RightJoint.transform);
        if ((leftJointAngle < CabinetAngleLimit) && (rightJointAngle > -CabinetAngleLimit))
        {
            //��ɹ��˺��������Ϲ��ţ����˲��ٶ���������в����û�е��˶���������вʱ��ֹͣ���ǵ�Bleeding
            RemoveOnce<GameObject>(PressureSystem.ThreateningEnemies, this.gameObject);

            EyeLight.SetActive(false);
            anim.Play("EyeEnemy_Idle");
            audioSource.Stop();
            return;
        }
        //�մ�ʱ�����һ���޵�ʱ��
        if (firstTimeTimer < InvincibleTime)
        {
            firstTimeTimer += Time.deltaTime;
            return;
        }
        //ת��״̬�߼�
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
        {
            if (currentState == State.Looking)
            {
                canDamage = true;
                currentState = State.Counting;
                anim.Play("EyeEnemy_Count");
                audioSource.clip = countingSFX;
                audioSource.Play();
                EyeLight.SetActive(false);
                lastHeadPos = playerHeadset.transform.position;
                lastLeftHandPos = playerLeftHand.transform.position;
                lastRightHandPos = playerRightHand.transform.position;
                lastHeadRotate = playerHeadset.transform.rotation;
                lastBodyPos = Body.transform.position;
            }
            else if (currentState == State.Counting)
            {
                canDamage = true;
                currentState = State.Looking;
                anim.Play("EyeEnemy_Look");
                audioSource.clip = attackingSFX;
                audioSource.Play();
                EyeLight.SetActive(true);
                lastHeadPos = playerHeadset.transform.position;
                lastLeftHandPos = playerLeftHand.transform.position;
                lastRightHandPos = playerRightHand.transform.position;
                lastHeadRotate = playerHeadset.transform.rotation;
                lastBodyPos = Body.transform.position;
            }
        }
        //ÿ��״̬�ֱ���ʲô
        switch (currentState)
        {
            case State.Counting:
                return;
            case State.Looking:
                Debug.DrawRay(transform.position, transform.forward * RayLength, Color.red);
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, RayLength))
                {
                    if (raycastHit.collider.gameObject.CompareTag("Player"))
                    {
                        DetectMove(playerHeadset, HeadLimitDistance, ref lastHeadPos, ref isHeadMove);
                        DetectMove(playerLeftHand, HandLimitDistance, ref lastLeftHandPos, ref isLeftHandMove);
                        DetectMove(playerRightHand, HandLimitDistance, ref lastRightHandPos, ref isRightHandMove);
                        DetectRotate(playerHeadset, HeadLimitRotation, ref lastHeadRotate, ref isHeadRotate);
                        DetectMove(Body, BodyLimitDistance, ref lastBodyPos, ref isBodyMove);
                        //Debug.Log($"{ isHeadMove},{ isLeftHandMove}, { isRightHandMove}");
                        // Debug.Log(lastRightHandPos);
                        if (isHeadMove || isLeftHandMove || isRightHandMove||isHeadRotate)
                        {
                            if (canDamage)
                            {
                                PressureSystem.currentPressure += Damage;
                                PressureSystem.currentState = PressureSystem.State.Damaged;
                               AddOnce<GameObject>(PressureSystem.ThreateningEnemies, this.gameObject);
                                canDamage = false;
                            }
                        }
                    }
                }
                return;
        }
        //�������ƶ�
        void DetectMove(GameObject obj, float limitDistance, ref Vector3 lastPos, ref bool isMove)
        {
            float distancePerUpdate = Vector3.Distance(obj.transform.position, lastPos);
            lastPos = obj.transform.position;
            if (distancePerUpdate > limitDistance)
            {
                isMove = true;
            }
            else
            {
                isMove = false;
            }
        }
        //��������ת
        void DetectRotate(GameObject obj, float limitRotation, ref Quaternion lastRotate, ref bool isRotated)
        {
            float rotationPerUpdatex = obj.transform.eulerAngles.x - lastRotate.eulerAngles.x;
            float rotationPerUpdatey = obj.transform.eulerAngles.y - lastRotate.eulerAngles.y;
            float rotationPerUpdatez = obj.transform.eulerAngles.z - lastRotate.eulerAngles.z;
            if (rotationPerUpdatex > limitRotation || rotationPerUpdatey > limitRotation || rotationPerUpdatez > limitRotation)
            {
                isRotated = true;
            }
            else
            {
                isRotated = false;
            }
        }
    }
    //ת�����ӵĻ���
    public float GetInspectorRotationValueMethod(Transform transform)
    {
        // ��ȡԭ��ֵ
        System.Type transformType = transform.GetType();
        PropertyInfo m_propertyInfo_rotationOrder = transformType.GetProperty("rotationOrder", BindingFlags.Instance | BindingFlags.NonPublic);
        object m_OldRotationOrder = m_propertyInfo_rotationOrder.GetValue(transform, null);
        MethodInfo m_methodInfo_GetLocalEulerAngles = transformType.GetMethod("GetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
        object value = m_methodInfo_GetLocalEulerAngles.Invoke(transform, new object[] { m_OldRotationOrder });
        string temp = value.ToString();
        //���ַ�����һ�������һ��ȥ��
        temp = temp.Remove(0, 1);
        temp = temp.Remove(temp.Length - 1, 1);
        //�á������ŷָ�
        string[] tempVector3;
        tempVector3 = temp.Split(',');
        //���ָ�õ����ݴ���Vector3
        Vector3 vector3 = new Vector3(float.Parse(tempVector3[0]), float.Parse(tempVector3[1]), float.Parse(tempVector3[2]));
        return vector3.y;
    }
}

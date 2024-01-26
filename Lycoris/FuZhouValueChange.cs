using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuZhouValueChange : MonoBehaviour
{
    public bool IsDisappear = false;
    public float Max;
    public float Speed;
    private ParticleSystem endWhiteSparks;
    private ParticleSystem endBlackSparks;
    float value;
    Material mat;
    void Start()
    {
        mat = this.GetComponent<MeshRenderer>().material;
        value = 0;
        endWhiteSparks = transform.GetChild(0).GetComponent<ParticleSystem>();
        endBlackSparks = transform.GetChild(1).GetComponent<ParticleSystem>();
        endWhiteSparks.Stop();
        endBlackSparks.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDisappear)
        {
            endWhiteSparks.Play();
            endWhiteSparks.Play();
            if (value < Max)
            {
                value += Speed*Time.deltaTime;
            }

            var whiteShapeModule = endWhiteSparks.shape;
            whiteShapeModule.position= new Vector3(0,value - 0.5f,0);
            var blackShapeModule = endBlackSparks.shape;
            blackShapeModule.position= new Vector3(0,value - 0.5f,0);
            mat.SetFloat("_LerpValue", value);
        }

        if (value >= Max)
        {
            value = Max;
            endWhiteSparks.Stop();
            endWhiteSparks.Stop();
        }
    }
}

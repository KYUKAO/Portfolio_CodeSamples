using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PipelineController : MonoBehaviour
{
    Mesh mesh;
    Material material;
    public bool AlwaysFullPip;
    public float Delay;
    public Color liquidColor;
    public Color surfaceColor;
    float flowDirection;
    public float flowSpeed;
    GameObject thisPipe;
    public int[] directions;
    public int pipeID;
    public int lastPipeID;
    public UnityEvent PipFull;
    public UnityEvent PipEmpty;
    public int PipNumBeforeSpeel;
    public bool HasSpeel;
    private bool canSpeelBreak;
    private bool isPressedButtom;
    private bool pipFullDoOnce;

    void Start()
    {
        lastPipeID = -1;
        pipeID = -1;
        canSpeelBreak = false;
        if (AlwaysFullPip)
        {
            BeginFlow();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SpeelBreaked();
        }
        WhenPipFull();
        if (pipeID >= PipNumBeforeSpeel && canSpeelBreak == false && HasSpeel == true)
        {
            return;
        }
        if (pipeID != lastPipeID)
        {
            if (!isPressedButtom) return;
            if (pipeID < directions.Length)
            {
                thisPipe = this.transform.GetChild(pipeID).GetChild(0).gameObject;
                flowDirection = directions[pipeID];
            }
            mesh = thisPipe.GetComponent<MeshFilter>().mesh;//MeshFilter instead of meshRenderer
            var bounds = mesh.bounds;
            var boundSize = thisPipe.GetComponent<PipeBoundSize>().BoundSize;
            var boundCenter = thisPipe.GetComponent<PipeBoundSize>().BoundCenter;
            material = thisPipe.GetComponent<MeshRenderer>().material;
            // material.SetVector("_BoundsSize", bounds.size);
            material.SetVector("_BoundsSize", boundSize);
            // material.SetVector("_BoundsCenter", bounds.center);
            material.SetVector("_BoundsCenter", boundCenter);

            if (pipeID >= directions.Length)
            {
                return;
            }
            lastPipeID = pipeID;
            StartCoroutine(Flow());
        }
    }

    IEnumerator Flow()
    {
        //material.SetColor("_LiquidColor", liquidColor);
        material.SetColor("_SurfaceColor", surfaceColor);
        material.SetFloat("_Direction", (float)flowDirection);

        for (float fill = flowSpeed; fill <= 1+flowSpeed; fill += flowSpeed)
        {
            if (isPressedButtom)
            {
                material.SetFloat("_Fill", fill);
                yield return new WaitForSeconds(Delay);
            }
        }
        pipeID += 1;
    }
    public void BeginFlow()
    {
        isPressedButtom = true;
        pipeID = 0;
        pipFullDoOnce = true;
    }
    public void ClearPip()
    {
        isPressedButtom = false;
        lastPipeID = -1;
        pipeID = -1;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            thisPipe = this.transform.GetChild(i).GetChild(0).gameObject;
            mesh = thisPipe.GetComponent<MeshFilter>().mesh;//meshFilter, not meshRenderer
            var bounds = mesh.bounds;
            material = thisPipe.GetComponent<MeshRenderer>().material;
            material.SetFloat("_Fill", 0f);
        }
    }
    void WhenPipFull()
    {
        if (pipeID >= directions.Length)
        {
            if (pipFullDoOnce)
            {
                PipFull.Invoke();
            }
            pipFullDoOnce = false;
        }
    }
    public void SpeelBreaked()
    {
        canSpeelBreak = true;
    }
}
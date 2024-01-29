using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectComponent : MonoBehaviour
{
    public FuZhouValueChange FuZhou;
    public GhostFireControl GhostFire;
    //AudioSource audioSource;
    //AudioClip spellDisappearSFX;
    private void Start()
    {
        //spellDisappearSFX = Resources.Load("Audio/GhostFire/GhostFire_SpellDisappear") as AudioClip;
    }
    private void Update()
    {
        if(GhostFire.mat.GetFloat("_Alpha")<=0f)
        {
            DieSpell();
        }
    }
    public void DieSpell()
    {
        this.transform.GetChild(1).gameObject.SetActive(false);
        FuZhou.IsDisappear = true;
        /*
        if (!(audioSource.isPlaying && audioSource.clip == spellDisappearSFX))
        {
            audioSource.clip = spellDisappearSFX;
            audioSource.Play();
        }
        */
    }
}

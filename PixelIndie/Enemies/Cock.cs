using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cock : CharacterControl
{
    PlayerControl Player;
    GameObject BossHP;

    public AK.Wwise.Event die;
    public AK.Wwise.Event BossMusic;
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        BossHP = GameObject.FindWithTag("GameUI").transform.GetChild(0).gameObject;
        BossHP.SetActive(true);
        AkSoundEngine.StopAll();
        BossMusic.Post(gameObject);
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        BossHP.transform.GetChild(1).GetComponent<Image>().fillAmount = (float)currentHP / maxHP;

        Player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        AttackVector = (Player.transform.position - transform.position).normalized;

        if (rb.velocity.x > 0.01f)
        {
            sr.flipX = true;
        }
        else if (rb.velocity.x < -0.01f)
        {
            sr.flipX = false;
        }

        AnimatorStateInfo Info = anim.GetCurrentAnimatorStateInfo(0);

        if (currentHP <= 0)
        {
            rb.velocity = new Vector3(0, 0, 0);
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            isAlive = false;
            BossHP.SetActive(true);
            AkSoundEngine.StopAll();
            anim.Play("Dead");
        }
        if (Info.normalizedTime > 1f && currentHP <= 0)
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            die.Post(gameObject);
            Destroy(gameObject);
        }

        if (Info.normalizedTime > 1f && canShoot)
        {
            rb.velocity = new Vector3(0, 0, 0);
            anim.Play("Attack");
            canShoot = false;
        }

        if (Info.normalizedTime > 1f && !canShoot)
        {
            rb.velocity = AttackVector * movementSpeed_Final;
            anim.Play("Walk");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaHua : CharacterControl
{

    public float changeMovingDirectionIntervalTime;
    float timer;
    PlayerControl Player;
    void Start()
    {
        rb.velocity = new Vector2(0, 0);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        timer += Time.deltaTime;

        //Flip the sprite
        if (rb.velocity.x > 0.01f)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }


        //Die Logic,same as other enemies
        AnimatorStateInfo Info = anim.GetCurrentAnimatorStateInfo(0);
        if (currentHP <= 0)
        {
            rb.velocity = new Vector3(0, 0, 0);
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            anim.Play("Dead");
        }

        if (Info.normalizedTime > 1f && currentHP <= 0)
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            Destroy(gameObject);
        }

        //idle and Attack
        if (Info.normalizedTime > 1f && canShoot)
        {
            anim.Play("Attack");
            rb.velocity = new Vector2(0, 0);
            CommonShoot();
        }
        if (Info.normalizedTime > 1f && !canShoot)
        {
            Idle();
            anim.Play("idle");
        }
    }

    private void Idle()
    {
        anim.Play("idle");
        //after each period of time , it turns to a random direction
        if (timer > changeMovingDirectionIntervalTime)
        {
            Player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
            AttackVector = (Player.transform.position - transform.position).normalized;
            rb.velocity = AttackVector * movementSpeed_Final;
            timer = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponent : CharacterBaseUnit
{
    float timer;
    public float IntervalTime;
    GameObject player1;
    GameObject player2;
    public float DetectRadius;
    public float BindRadius;
    GameObject bindedPlayer = null;
    public float DamageValue;
    bool isActivated = false;

    private void Start()
    {
        timer = IntervalTime;
        var players = FindObjectsOfType<PlayerComponent>();
        foreach (var player in players)
        {
            if (player.ThisPlayerID == PlayerComponent.PlayerID._P1)
            {
                player1 = player.gameObject;
            }
            else if (player.ThisPlayerID == PlayerComponent.PlayerID._P2)
            {
                player2 = player.gameObject;
            }
        }
    }

    private void Update()
    {
        //Enemies normally change moving direction after sometime ,change animation
        //And detect if there's player nearby
        if (!isActivated)
        {
            timer += Time.deltaTime;
            if (timer >= IntervalTime)
            {
                processedLookInput = Random.Range(-1, 2);
                horizontalInput = Random.Range(-1, 2);
                verticalInput = Random.Range(-1, 2);
                timer = 0;
            }
            AnimationControl();
            if (player1)
            {
                DetectPlayer(player1);
            }
            if (player2)
            {
                DetectPlayer(player2);
            }
        }
        else
        {
            //The enemy stick to the target player and the player and contantly get hurt.
            if (bindedPlayer)
            {
                if (bindedPlayer.GetComponent<PlayerComponent>() != null)
                {
                    bindedPlayer.GetComponent<PlayerComponent>().CurrentHealth -= DamageValue * Time.deltaTime;
                    //The player can rescue the other player by hitting the enemy with body
                    if (Vector3.Distance(bindedPlayer.transform.position, this.transform.position) > BindRadius)
                    {
                        bindedPlayer.GetComponent<PlayerComponent>().CanMove = true;
                        bindedPlayer = null;
                        isActivated = false;
                    }
                }
            }
        }
        HealthControl();
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        Move();
        RotateBody();
    }

    private void DetectPlayer(GameObject player)
    {
        //If the enemy successfully detected a player,it automatically head for him.
        if (player.GetComponent<PlayerComponent>() != null)
        {
            if ((Vector3.Distance(player.transform.position, this.transform.position) < DetectRadius)
                && (player1.GetComponent<PlayerComponent>().CanMove == true)
                && (player2.GetComponent<PlayerComponent>().CanMove == true))
            {
                horizontalInput = (player.transform.position - this.transform.position).normalized.x;
                verticalInput = (player.transform.position - this.transform.position).normalized.z;
                //If the enemy gets close successfully, the target player can't move.
                if (Vector3.Distance(player.transform.position, this.transform.position) < BindRadius)
                {
                    horizontalInput = 0f;
                    verticalInput = 0f;
                    if (player.GetComponent<PlayerComponent>() != null)
                    {
                        bindedPlayer = player;
                        bindedPlayer.GetComponent<PlayerComponent>().CanMove = false;
                    }
                    isActivated = true;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If the enemy hits wall, change a direction;
        if (collision.gameObject.GetComponent<PlayerComponent>() == null)
        {
            timer = IntervalTime;
        }
    }

    private void Die()
    {
        //if the enemy is dead , free the target player and change the UI;
        if (bindedPlayer)
        {
            bindedPlayer.GetComponent<PlayerComponent>().CanMove = true;
            bindedPlayer = null;
        }
        GameSystem.Instantce.Enemies = FindObjectsOfType<EnemyComponent>();
        DestroyImmediate(this.gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : CharacterBaseUnit
{
    #region Variables
    public enum PlayerID
    {
        _P1,
        _P2,
    }
    public float RayLength;
    public Transform RayCastPoint;
    GameObject targetObj = null;
    EnemyComponent enemyComponent;
    [HideInInspector]
    public bool CanMove = true;
    public PlayerID ThisPlayerID;
    #endregion

    private void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        //Local multiplayer input
        processedTurnInput = Input.GetAxis("Mouse X" + ThisPlayerID);
        processedLookInput = -Input.GetAxis("Mouse Y" + ThisPlayerID);
        horizontalInput = Input.GetAxis("Horizontal" + ThisPlayerID);
        verticalInput = Input.GetAxis("Vertical" + ThisPlayerID);
        RayCast();
        MoveCamera();
        HealthControl();
        if (CanMove)
        {
            AnimationControl();
        }
        if (CurrentHealth <= 0)
        {
            GameSystem.Instantce.IsGameOver = true;
        }
    }

    private void FixedUpdate()
    {
        RotateBody();
        if (CanMove)
        {
            Move();
        }
    }

    private void RayCast()
    {
        //Draw a ray to detect if the players's looking at something.
        Debug.DrawRay(RayCastPoint.position, CameraContainer.transform.forward * RayLength, Color.red);
        if (Physics.Raycast(RayCastPoint.position, CameraContainer.transform.forward, out RaycastHit raycast, RayLength))
        {
            targetObj = raycast.collider.gameObject;
            //If he's looking at an enemy, put the player into a list, put the enemy into another List.
            if (targetObj.GetComponent<EnemyComponent>() != null)
            {
                enemyComponent = targetObj.GetComponent<EnemyComponent>();
                AddOnce<EnemyComponent>(GameSystem.Instantce.TargetEnemies, enemyComponent);
                AddOnce<PlayerComponent>(GameSystem.Instantce.AttackingPlayers, this.GetComponent<PlayerComponent>());
            }
            else
            {
                RemoveOnce<PlayerComponent>(GameSystem.Instantce.AttackingPlayers, this.GetComponent<PlayerComponent>());
                RemoveOnce<EnemyComponent>(GameSystem.Instantce.TargetEnemies, enemyComponent);
            }
            //If he's looking at a player and clicking, he can deliver health to him.
            if (targetObj.GetComponent<PlayerComponent>() != null)
            {
                if (targetObj.GetComponent<PlayerComponent>().CanMove == true && Input.GetButton("Fire" + ThisPlayerID)
                    &&targetObj.GetComponent<PlayerComponent>().CurrentHealth< targetObj.GetComponent<PlayerComponent>().MaxHealth)
                {
                    this.CurrentHealth -= DeliverHealth * Time.deltaTime;
                    targetObj.GetComponent<PlayerComponent>().CurrentHealth += DeliverHealth * Time.deltaTime;
                }
            }
        }
    }

    private void AddOnce<T>(List<T> team, T member)
    {
        if (!team.Contains(member))
        {
            team.Add(member);
        }
    }

    private void RemoveOnce<T>(List<T> team, T member)
    {
        if (team.Contains(member))
        {
            team.Remove(member);
        }
    }
}

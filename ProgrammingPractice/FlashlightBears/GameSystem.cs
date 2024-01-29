using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : Singleton<GameSystem>
{
    [HideInInspector]
    public  List<PlayerComponent> AttackingPlayers = new List<PlayerComponent>();
    [HideInInspector]
    public  List<EnemyComponent> TargetEnemies = new List<EnemyComponent>();
    [HideInInspector]
    public EnemyComponent[] Enemies;
    [HideInInspector]
    public  bool IsGameOver = false;
    public float Power;
    public GameObject GameOver;
    public GameObject WinInterface;
    public Text EnemyNumText;

    private void Start()
    {
        Time.timeScale = 1;
        Enemies = FindObjectsOfType<EnemyComponent>();
        if (GameOver)
        {
            GameOver.SetActive(false);
        }
        if (WinInterface)
        {
            WinInterface.SetActive(false);
        }
    }

    private void Update()
    {
        //If there's two players looking at the same enemy, the enemy gets hurt.
        if (AttackingPlayers.Count == 2 && TargetEnemies.Count != 0)
        {
            foreach(var enemy in TargetEnemies)
            {
                enemy.CurrentHealth -= Power * Time.deltaTime;
            }
        }
        if (GameOver)
        {
            if (IsGameOver)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                GameOver.SetActive(true);
            }
        }
        if (WinInterface)
        {
            if (Enemies.Length == 0)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                WinInterface.SetActive(true);
            }
        }
        if (EnemyNumText)
        {
            EnemyNumText.text = "Enemy :"+Enemies.Length;
        }
    }
}

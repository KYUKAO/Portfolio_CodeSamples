using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverSystem : MonoBehaviour
{
    public Text StateText;
    public static bool IsGameOver = true;

    public void OnEnable()
    {
        Evently.Instance.Subscribe<GameOverEvent>(WinningJudgement);
        if (StateText)
        {
            StateText.text = "Run";
        }
    }

    public void OnDisable()
    {
        Evently.Instance.Unsubscribe<GameOverEvent>(WinningJudgement);
    }

    public void WinningJudgement(GameOverEvent evt)
    {
        if (StateText && evt.IsWin)
        {
            StateText.text = "Win";
        }
        else
        {
            StateText.text = "Lose";
        }
        Invoke("ChangeTextToRun",0.5f);
    }
    void ChangeTextToRun()
    {
        StateText.text = "Run";
        IsGameOver = true;
    }
}

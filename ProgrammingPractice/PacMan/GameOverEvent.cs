using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEvent 
{
    public bool IsWin;
    public GameOverEvent(bool isWin)
    {
        this.IsWin = isWin;
    }
}

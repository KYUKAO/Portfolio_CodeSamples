using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HackMan_GD07;
using UnityEngine.SceneManagement;
using System;

public class PlayerInputComponent : MovementComponent
{
    protected override void Update()
    {
        //We want to get playerinput first before moving
        //calling 'base' calls the virtual method , first
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentInputDirection = new IntVector2(0, -1);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentInputDirection = new IntVector2(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentInputDirection = new IntVector2(-1, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentInputDirection = new IntVector2(1, 0);
        }
        base.Update();
    }
}

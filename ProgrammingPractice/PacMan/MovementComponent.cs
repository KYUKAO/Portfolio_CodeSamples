using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HackMan_GD07;

namespace HackMan_GD07
{
    public class MovementComponent : BaseGridObject
    {
        public float MovementSpeed;
        protected IntVector2 targetGridPosition;
        protected float progressToTarget = 1f;
        protected IntVector2 currentInputDirection;
        protected IntVector2 previousInputDirection;

        protected virtual void Start()
        {
            targetGridPosition = GridPosition;
        }

        protected virtual void Update()
        {
            if (transform.localPosition == targetGridPosition.ToVector3())
            {
                progressToTarget = 0f;
                GridPosition = targetGridPosition;
            }
            //if we set a new target AND out current input is VALID ->  NOT A WALL
            if (GridPosition == targetGridPosition 
                && !(GridPosition+currentInputDirection).isWall())
            {
                targetGridPosition += currentInputDirection;
                previousInputDirection = currentInputDirection;
            }
            //If we set a new target AND our current input is NOT VALID-> IT IS A WALL
            else if (GridPosition == targetGridPosition
                && !(GridPosition + currentInputDirection).isWall())
            {
                targetGridPosition += previousInputDirection;
            }
            else if(GridPosition==targetGridPosition)
            {
                //Debug.Log("Die");
            }
            if (GridPosition == targetGridPosition) return;
            progressToTarget += MovementSpeed * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(GridPosition.ToVector3(), targetGridPosition.ToVector3(), progressToTarget);
            //Lerp from A to B,A must be a fixed value.
        }
    }

    public static class ExtensionMethods
    {//Extension methods allows us to EXTEND the functionality of our classes,without MODIFYING the class itself
        //This follows one of our principles
        public static Vector3 ToVector3(this IntVector2 vector2)
        {
            return new Vector2(vector2.x, vector2.y);
        }
        public static IntVector2 ToIntVector2(this Vector3 vector3)
        {
            return new IntVector2((int)vector3.x, (int)vector3.y);
        }
        public static bool isWall(this IntVector2 vector2)
        {
            return LevelGeneratorSystem.Grid[Mathf.Abs(vector2.y), Mathf.Abs(vector2.x)]==1;
        }
    }
}


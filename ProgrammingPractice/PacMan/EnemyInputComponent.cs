using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HackMan_GD07;
using System.Linq;

public class EnemyInputComponent : MovementComponent
{
    //Controller
    private IntVector2[] movementDirections = new IntVector2[]
    {
        IntVector2.up,
        IntVector2.down,
        IntVector2.left,
        IntVector2.right,
    };

    protected override void Update()
    {
        if (transform.localPosition == targetGridPosition.ToVector3())
        {
            var possibleDirections = new List<IntVector2>();
            foreach (var movementDirection in movementDirections)
            {
                var potentialTargetPosition = targetGridPosition + movementDirection;
                if (potentialTargetPosition.isWall()) continue;
                if (movementDirection != -currentInputDirection)
                {
                    possibleDirections.Add(movementDirection);
                }
            }
            if (possibleDirections.Count < 1)
            {
                possibleDirections.Add(-currentInputDirection);
            }
            var direction = Random.Range(0, possibleDirections.Count);
            currentInputDirection = possibleDirections[direction];
        }
        //var possibleDirections = movementDirections.Where(movementDirections
        //    => !((targetGridPosition + movementDirection).IsWall())
        //    && movementDirection
        base.Update();
    }
}

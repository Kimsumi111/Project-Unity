using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMove : EnemyMove
{ 
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Rigid.velocity = new Vector2(nextMove * 1.7f, Rigid.velocity.y);

        if (rayHit.collider == null)
            Turn();
    }
}

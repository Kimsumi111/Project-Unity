using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWormMove : EnemyMove
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Rigid.velocity = new Vector2(nextMove, Rigid.velocity.y);

        if (rayHit.collider == null)
            Turn();
    }
}

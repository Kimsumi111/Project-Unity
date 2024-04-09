using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogDie : EnemyDie
{
    protected override void MoveControl(){
        base.MoveControl();

        FrogMove frogMove = GetComponent<FrogMove>();

        frogMove.anim.speed = 0;

        frogMove.isDead = true;

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders){
            collider.enabled = false;
        }
    }
}
    


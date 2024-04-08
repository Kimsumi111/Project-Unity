using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogDie : EnemyDie
{
    FrogMove frogMove;
    Animator anim;

    protected override void MoveControl(){
        base.MoveControl();
        FrogMove frogMove = GetComponent<FrogMove>();
            if (frogMove != null)
                frogMove.enabled = false;  
            else
                Debug.LogWarning("FrogMove 스크립트가 없습니다.");
        
        anim = GetComponent<Animator>();
        anim.speed = 0;
        Debug.Log("애니 멈춤");

        frogMove.walkCollide.enabled = true;
        frogMove.jumpCollide.enabled = false;

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders){
            collider.enabled = false;
        }
    }
}
    


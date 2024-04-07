using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMove : EnemyMove
{
    public float jumpForce, nextJumpTime;
    //public float nextJumpTime;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("점프 시작함다");
    }   
    protected override void Turn(){
        base.Turn();
        Invoke("Jump", nextJumpTime);
    }

    void Jump(){
        Debug.Log("점프함다");
        // 점프
        Rigid.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
    }
}

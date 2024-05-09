using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMove : EnemyMove
{
    public Collider2D walkCollide;
    public Collider2D jumpCollide;
    public RaycastHit2D rayHit2;
    public float jumpForce;
    public bool isDead = false;

    protected override void Awake()
    {
        base.Awake();
        walkCollide.enabled = false;
        jumpCollide.enabled = false;
    }
    protected override void Think(){
        base.Think();
        if(!anim.GetBool("isJumping")){
            if(isDead == false){
                Jump();
            }
        }        
    }
    protected override void FixedUpdate(){
        base.FixedUpdate();

        Rigid.velocity = new Vector2(nextMove * 0.5f, Rigid.velocity.y);

        Vector2 downVec = new Vector2(Rigid.position.x + nextMove*0.05f, Rigid.position.y - 1f); 
        Debug.DrawRay(downVec, Vector2.down, new Color(0,1,0));
        rayHit2 = Physics2D.Raycast(downVec, Vector3.down, 3, LayerMask.GetMask("Platform", "Ladder"));
        if(rayHit2.collider == null){
            if (Mathf.Abs(Rigid.velocity.y) == 0)
                Turn();
        }
    }   

    void Jump(){
        // 점프
        anim.SetBool("isJumping", true);
        Rigid.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
        Invoke("StopJumpping", 0.7f);
    }
    void StopJumpping(){
        anim.SetBool("isJumping", false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Platform")){
            anim.SetBool("isJumping", false);
        }
    }

    public void ActivateWalkCollide(){
        if(!isDead){
            walkCollide.enabled = true;
            jumpCollide.enabled = false;
        }
        
    }
    public void ActivateJumpCollide(){
        walkCollide.enabled = false;
        jumpCollide.enabled = true;
    }
}

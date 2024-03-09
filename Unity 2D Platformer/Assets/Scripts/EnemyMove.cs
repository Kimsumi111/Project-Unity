using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;

    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        Invoke("Think", 3);
    }    

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.35f, rigid.position.y );
        Debug.DrawRay(frontVec, Vector2.down, new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform")); 

        if(rayHit.collider == null){
            Turn();
        }
    }
    //재귀 함수 
    void Think(){
        //Set Next Active
        int[] arr = {-1, 1};
        nextMove = arr[Random.Range(0, arr.Length)];
        
        //Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);
        
        //Flip Sprite
        if(nextMove != 0)
            spriteRenderer.flipX = nextMove == 1; //spriteRenderer.flipX은 bool타입
        
        //Recursive
        float nextThinkTime = Random.Range(0.5f,3f);
        Invoke("Think", nextThinkTime); 
        //보통 재귀함수를 맨 아래에 쓴다.   
    }

    void Turn(){
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think",3);
    }

    public void OnDamaged(){
        //Sprite Alpha
        spriteRenderer.color = new Color(1,1,1,0.4f);
        //Sprtie Flip X Freeze
        nextMove = 0;
        //Sprite Flip Y
        spriteRenderer.flipY = true;
        //Collider Disable
        GetComponent<CapsuleCollider2D>().enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up*5, ForceMode2D.Impulse);
        //Destroy
        Invoke("DeActive", 3);
    }

    void DeActive(){
        gameObject.SetActive(false);
    }
}

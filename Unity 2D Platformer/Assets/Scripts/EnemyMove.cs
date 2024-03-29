using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D Rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    Collider2D collid;

    public int nextMove;

    void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collid = GetComponent<Collider2D>();
        Invoke("Think", 3);
    }    

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move
        if(gameObject.name.Contains("EarthWorm"))
            Rigid.velocity = new Vector2(nextMove, Rigid.velocity.y);
        else if(gameObject.name.Contains("Mushroom"))
            Rigid.velocity = new Vector2(nextMove*1.7f, Rigid.velocity.y);
        //Platform Check
        //래이캐스트 x축 위치
        Vector2 frontVec = new Vector2(Rigid.position.x + nextMove*0.35f, Rigid.position.y );
        
                
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
        collid.enabled = false;
        
        //Die Effect Jump
        Rigid.AddForce(Vector2.up*5, ForceMode2D.Impulse);
        //Destroy
        Invoke("DeActive", 3);
    }

    void DeActive(){
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Vector3 scale;
    public int[] nextMoveArr = {-1, 1};
    protected Rigidbody2D Rigid;
    public Animator anim;
    public RaycastHit2D rayHit;
    SpriteRenderer spriteRenderer;
    Collider2D collid;

    public int nextMove;

    protected virtual void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collid = GetComponent<Collider2D>();
        Invoke("Think", 3);
    }    

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        //Platform Check
        //래이캐스트 x축 위치
        Vector2 frontVec = new Vector2(Rigid.position.x + nextMove*0.35f, Rigid.position.y );        
        Debug.DrawRay(frontVec, Vector2.down, new Color(0,1,0));
        rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform", "Ladder")); 
    }
    //재귀 함수 
    protected virtual void Think(){
        //Set Next Active
        nextMove = nextMoveArr[Random.Range(0, nextMoveArr.Length)];
        
        if(!gameObject.name.Contains("Frog"))
        //Sprite Animation
            anim.SetInteger("WalkSpeed", nextMove);
        
        //Flip Sprite
        if(nextMove != 0){
            if(nextMove == 1){
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else{
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
           
        //Recursive
        float nextThinkTime = Random.Range(0.5f,3f);
        Invoke("Think", nextThinkTime); 
        //보통 재귀함수를 맨 아래에 쓴다.   
    }

    public void Turn(){
        nextMove *= -1;
        
        if(nextMove != 0){
            scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        
        CancelInvoke();
        Invoke("Think",3);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public float maxSpeed;
    public float jumpPower;
    public bool isLadder = false;
    public bool canJump = true;
    public Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }
    void Jump(){
        if(Input.GetButtonDown("Jump"))
            rigid.AddForce(Vector2.up*jumpPower, ForceMode2D.Impulse);
    }

    void Update()
    {
        if ((!isLadder || canJump) && Input.GetButtonDown("Jump")){
            Jump();
        }
        //Stop Speed
        if(Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x*0.5f, rigid.velocity.y);
        }
        //Direction Sprite
        if(Input.GetButton("Horizontal")) {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;    
        }
        //Player Animation
        if(Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("IsWalking", false);
        else
            anim.SetBool("IsWalking", true);
    }
    void FixedUpdate()
    {
        //Move By Key Control
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right*h, ForceMode2D.Impulse);

        if(rigid.velocity.x > maxSpeed) //Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if(rigid.velocity.x < maxSpeed*(-1)) //Left Max Speed
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
        
        if(isLadder){
            float v = Input.GetAxis("Vertical");
            rigid.gravityScale = 0;
            Vector3 ladderMove = new Vector3(0, v*3.5f*Time.deltaTime, 0);
            transform.Translate(ladderMove);
        }
        else{
            rigid.gravityScale = 4f;  
        }
        //만약 플레이어가 클라임 벽돌 위에 있을 경우 점프 가능. 아니면 불가능.
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy"){
            //Attack
            if(rigid.velocity.y < -0.05 && transform.position.y > collision.transform.position.y-0.1){
                OnAttack(collision.transform); //자신의 collider가 아니라 닿는 collider
            }
            //Damage
            else
                OnDamaged(collision.transform.position);
        }
        if(collision.gameObject.tag == "Climb"){
            canJump = true;
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Item"){
            //Point
            bool isYellow = collision.gameObject.name.Contains("Yellow");
            bool isRed = collision.gameObject.name.Contains("Red");
            bool isHeart = collision.gameObject.name.Contains("Heart");
            if(isYellow)
                gameManager.stagePoint += 10;
            else if(isRed)
                gameManager.stagePoint += 50;
            else if(isHeart)
                gameManager.HealthUp();
                
            //Deactive Item
            collision.gameObject.SetActive(false);
        }              
        else if (collision.gameObject.tag == "Finish"){
            //Next Stage
            gameManager.NextStage();
        }
        if(collision.CompareTag("Ladder")){
            isLadder = true;
            canJump = false;
        }        
    } 
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder"))
            isLadder = false;
            canJump = true;
    }

    void OnAttack(Transform enemy){
        //point
        gameManager.stagePoint += 100;
        //Reaction Force
        rigid.AddForce(Vector2.up*25, ForceMode2D.Impulse);
        //Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }
    void OnDamaged(Vector2 targetPos){
        //Health Down
        gameManager.HealthDown();
        //Change Layer (Immortal  Active)
        gameObject.layer = 9;
        //View Alpha
        spriteRenderer.color = new Color(1,1,1,0.4f);
        //Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc,1)*10, ForceMode2D.Impulse);
        //Animation
        anim.SetTrigger("doDamaged");
        //Freeze
        
        Invoke("OffDamaged", 2);
    }

    void OffDamaged(){
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1,1,1,1);
    }
    
    public void OnDie(){
        //Sprite Alpha
        spriteRenderer.color = new Color(1,1,1,0.4f);
        //Sprite Flip Y
        spriteRenderer.flipY = true;
        //Collider Disable
        capsuleCollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up*5, ForceMode2D.Impulse);
    }
    public void VelocityZero(){
        rigid.velocity = Vector2.zero;
    }     
}

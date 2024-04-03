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
    public float dashingPower = 20f;
    public float dashingTime= 0.2f;
    public float dashingCooldown = 2f;
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CapsuleCollider2D capsuleCollider;
    [SerializeField] private Animator anim;
    [SerializeField] private TrailRenderer tr;
    RaycastHit2D rayHit;

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
        //Dash 중 행동 방지
        if (isDashing){
            return;
        }
        //Jump
        if ((!isLadder || canJump) && Input.GetButtonDown("Jump") && !anim.GetBool("IsJumping") && !anim.GetBool("IsFalling")){
            Jump();
            anim.SetBool("IsJumping", true);
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
        //Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash){
            StartCoroutine(Dash());
        }
    }
    void FixedUpdate()
    {
        //Dash 중 행동 방지
        if (isDashing){
            return;
        }

        //Move By Key Control
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right*h, ForceMode2D.Impulse);

        //Max Speed
        if(rigid.velocity.x > maxSpeed) //Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if(rigid.velocity.x < maxSpeed*(-1)) //Left Max Speed
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
        
        //Landing Platform
        if(rigid.velocity.y < 0){
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", true);
            
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));
            rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if(rayHit.collider != null){
                if(rayHit.distance < 0.5f)
                    anim.SetBool("IsFalling", false);
            }
        }
        if(isLadder){
            float  v = Input.GetAxis("Vertical");
            rigid.gravityScale = 0;
            Vector3 ladderMove = new Vector3(0, v*3.5f*Time.deltaTime, 0);
            transform.Translate(ladderMove);
            anim.SetBool("IsClimbing", true);     
            
            if(Input.GetAxis("Vertical") == 0){
                anim.speed = 0f;
            }
            else
                anim.speed = 1f;

                
        }
        else{
            rigid.gravityScale = 4f;  
            anim.speed = 1f;
        }

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
            
        else if(collision.gameObject.tag == "Finish"){
            //Next Stage
            gameManager.NextStage();
        }
        //만약 플레이어가 클라임 벽돌 위에 있을 경우 점프 가능. 아니면 불가능.
        if(collision.CompareTag("Ladder")){
            isLadder = true;
            canJump = false;
            
            if(collision.gameObject.tag == "Climb")
                canJump = true;
        }        
    } 

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder")){
            isLadder = false;
            canJump = true;
            gameObject.layer = 8;
            anim.SetBool("IsClimbing", false);
        }
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
    private IEnumerator Dash(){
        canDash = false;
        isDashing = true;
        rigid.gravityScale = 0f;
        if(spriteRenderer.flipX == false)
            rigid.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        else
            rigid.velocity = new Vector2(transform.localScale.x * dashingPower*(-1), 0f);
        tr.emitting = true;
        
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rigid.gravityScale = 4f;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }  
}

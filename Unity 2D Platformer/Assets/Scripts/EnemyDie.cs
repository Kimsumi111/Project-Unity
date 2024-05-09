using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    public bool filpXFreeze;
    EnemyMove enemyMove;

    void Start()
    {
        enemyMove = GetComponent<EnemyMove>();
    }
    public void OnDamaged(){
        //Sprite Alpha
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1,1,1,0.4f);

        //Sprite Flip Y
        spriteRenderer.flipY = true;
        
        //Collider Disable
        Collider2D[] colliders = GetComponents<Collider2D>(); 

        foreach (Collider2D collider in colliders){
            collider.enabled = false;
        }
        enemyMove.nextMove = 0;
        
        DieEffectJump();

        MoveControl();

        //Destroy
        Invoke("DeActive", 3);
    }
    public void DieEffectJump(){
        //Die Effect Jump
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up*5, ForceMode2D.Impulse);
    }
    protected virtual void MoveControl(){
        enemyMove.nextMoveArr = new int[] { 0 };
        if (filpXFreeze){
            enemyMove.anim.SetInteger("WalkSpeed", 0);
        }        
    }

    public void DeActive(){
        gameObject.SetActive(false);
    }
}

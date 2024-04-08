using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    public bool filpXFreeze;

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
        if (filpXFreeze){
            EnemyMove enemyMove = GetComponent<EnemyMove>();
            if (enemyMove != null)
                enemyMove.enabled = false;  
            else
                Debug.LogWarning("EnemyMove 스크립트가 없습니다.");
        }        
        

    }

    public void DeActive(){
        gameObject.SetActive(false);
    }
}

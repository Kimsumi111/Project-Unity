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
        Collider2D collid= GetComponent<Collider2D>();
        collid.enabled = false;

        if (filpXFreeze){
            EnemyMove enemyMove = GetComponent<EnemyMove>();
            if (enemyMove != null)
                enemyMove.nextMove = 0;  
            else
                Debug.LogWarning("EnemyMove 스크립트가 없습니다.");
        }
        
        //Die Effect Jump
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up*5, ForceMode2D.Impulse);
        
        //Destroy
        Invoke("DeActive", 3);
    }

    public void DeActive(){
        gameObject.SetActive(false);
    }
}

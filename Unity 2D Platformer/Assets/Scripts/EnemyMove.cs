using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Invoke("Think", 3);
    }    

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y );
        Debug.DrawRay(frontVec, Vector2.down, new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform")); 
        if (rayHit.collider == null){
            Debug.Log("경고! 앞에 낭떠러지");
        }
    
    }
    //재귀 함수 
    void Think(){
        nextMove = Random.Range(-1, 2);
        Invoke("Think", 3);
    }
    
}

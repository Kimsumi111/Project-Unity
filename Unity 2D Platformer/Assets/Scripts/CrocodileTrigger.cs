using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileTrigger : MonoBehaviour
{
    public GameObject crocodile;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            Debug.Log("플레이어 닿았다");
            CrocodileJump crocodileJump = crocodile.GetComponent<CrocodileJump>();

            Debug.Log("점프 접근 완료");
            crocodileJump.enabled = true;
            crocodileJump.GetPlayerPos();          
        }        
    }
}

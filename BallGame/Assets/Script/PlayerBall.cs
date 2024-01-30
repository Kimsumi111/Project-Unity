using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBall : MonoBehaviour
{
    public int itemCount;
    public float jumpPower;
    public GameManagerLogic manager;
    bool isJump;
    Rigidbody rigid;
    AudioSource audio;

    void Awake()
    {
        isJump = false;
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        Debug.Log("first itemCount: "+ itemCount);
    }

    void Update() {
        if (Input.GetButtonDown("Jump") && !isJump) {
            isJump = true;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }

    void FixedUpdate() {   
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rigid.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "Floor")
            isJump = false;
    }
    void OnTriggerEnter(Collider other)
    {
        //여기서 itemCount가 자동으로 +1 됨...어째서?
        //Debug.Log("Trigger before itemCount: "+ itemCount);
        AudioSource audio = GetComponent<AudioSource>();
        if (other.tag=="item"){
            //itemCount++;
            //Debug.Log("itemCount: "+ itemCount);
            audio.Play();
            other.gameObject.SetActive(false);
            manager.GetItem(itemCount);
        }

        else if (other.tag=="Finish"){
            if (itemCount == manager.totalItemCount) {
                if (manager.stage == 2)
                    SceneManager.LoadScene(0);                
                else
                    SceneManager.LoadScene(manager.stage+1);
            }
            else {
                // 재시작...
                SceneManager.LoadScene(manager.stage);
            }
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCan : MonoBehaviour
{
    public float rotateSpeed;
    AudioSource audio;

    void Update()
    {
        transform.Rotate(Vector3.up*rotateSpeed*Time.deltaTime, Space.World);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerBall"){
            PlayerBall player = other.GetComponent<PlayerBall>();
            player.itemCount++;
            
            gameObject.SetActive(false);
        }
    }
}

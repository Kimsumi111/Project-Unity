using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemCoin : MonoBehaviour
{
    public float rotateSpeed;
    void Update()
    {
        transform.Rotate(Vector3.forward*rotateSpeed*Time.deltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerBall")
            gameObject.SetActive(false);
    }
  
}

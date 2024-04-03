using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float moveSpeed;

    void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        Invoke("TurnFish", 3f);
    }
    void TurnFish(){
        moveSpeed *= -1f;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }  
}

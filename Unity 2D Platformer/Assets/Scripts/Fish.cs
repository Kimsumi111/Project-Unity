using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float moveSpeed = 1f;

    void Update()
    {
        StartCoroutine(MoveFish());
    }
    private IEnumerator MoveFish(){

        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        yield return new WaitForSeconds(3f);

        moveSpeed *= -1f;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
        
        yield return new WaitForSeconds(3f);
    }  
}

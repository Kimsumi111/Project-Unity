using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrodcodileJump : MonoBehaviour
{
    public float jumpTime;
    public float jumpSpeed;
    private float playerPosY;

    public void Start(){
        playerPosY = GameObject.FindGameObjectWithTag("Player").transform.position.y;
        StartCoroutine(Jump());
    }
    IEnumerator Jump(){
        yield return new WaitForSeconds(jumpTime);

        float targetY = playerPosY - 6f; // 이동할 위치
        float elapsedTime = 0f;
        while (elapsedTime < jumpSpeed)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), elapsedTime / jumpSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
}

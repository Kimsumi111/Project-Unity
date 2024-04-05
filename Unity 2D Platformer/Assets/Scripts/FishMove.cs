using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public Hop hop;
    private bool movingRight = true; // 오른쪽으로 이동 중인지 여부

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(MoveAndScale());
    }

    IEnumerator MoveAndScale()
    {
        while (true)
        {
            // 1. 오른쪽으로 이동
            if (movingRight)
            {  
                float targetX = transform.position.x + 2f; // 이동할 위치
                float elapsedTime = 0f;
                while (elapsedTime < 3f)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), elapsedTime / 3f);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

            }
            // 2. 스케일 변경 (-로)
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
            movingRight = !movingRight;
            yield return null;

            // 3. 왼쪽으로 이동
            if (!movingRight)
            {
                float targetX = transform.position.x - 3f; // 이동할 위치
                float elapsedTime = 0f;
                while (elapsedTime < 3f)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), elapsedTime / 3f);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
            }
            // 4. 스케일 변경 (+로)
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
            movingRight = !movingRight;
            yield return null;
        }
    }
}

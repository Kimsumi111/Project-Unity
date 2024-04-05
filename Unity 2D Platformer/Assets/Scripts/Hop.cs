using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hop : MonoBehaviour
{
    public float speed = 3f; // 물고기의 이동 속도
    public float jumpForce = 5f; // 물고기의 점프 힘
    public float jumpInterval = 2f; // 점프 간격
    public float directionChangeInterval = 5f; // 이동 방향 변경 간격


    private Rigidbody2D rb;
    private bool facingRight = true; // 물고기가 오른쪽을 보고 있는지 여부

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 일정한 간격으로 자동으로 점프를 시작
        InvokeRepeating("Jump", jumpInterval, jumpInterval);
        // 일정한 시간 후에 이동 방향 변경
        Invoke("ChangeDirection", directionChangeInterval);
    }

    void Update()
    {
        // 자동으로 전진
        rb.velocity = new Vector2(speed * (facingRight ? 1 : -1), rb.velocity.y);
    }

    void Jump()
    {
        // 점프
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void ChangeDirection()
    {
        // 이동 방향 변경
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // 다음 방향 변경을 위해 다시 Invoke 호출
        Invoke("ChangeDirection", directionChangeInterval);
    }

}



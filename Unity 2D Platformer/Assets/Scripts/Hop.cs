using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hop : MonoBehaviour
{
    public float initialSpeed = 10f;
    public float angle = 45f;
    private Vector3 velocity;
    void Start()
    {
        // 각도를 라디안으로 변환
        float radianAngle = angle * Mathf.Deg2Rad;

        // 초기 속도 벡터 계산
        velocity = new Vector3(initialSpeed * Mathf.Cos(radianAngle), initialSpeed * Mathf.Sin(radianAngle), 0);
    }
    void Update()
    {
        // 포물선 운동 적용
        transform.position += velocity * Time.deltaTime;

        // 시간에 따라 속도를 변화시켜 포물선 운동 구현
        velocity.y -= Time.deltaTime * 9.8f;
    }
}

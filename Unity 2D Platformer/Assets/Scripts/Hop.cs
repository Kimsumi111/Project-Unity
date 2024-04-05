using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hop : MonoBehaviour
{
    private Vector3 startPos, endPos;
    protected float timer; //땅에 닿기까지 걸리는 시간
    protected float timeToFloor;

    protected static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t){
        Func<float, float> f = x => -4 * height * x *x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
    protected IEnumerator Move(){
        while(true){
            timer = 0;
            while(transform.position.y >= startPos.y){
                timer += Time.deltaTime;
                Vector3 tempPos = Parabola(startPos, endPos, 5, timer);
                transform.position = tempPos;
                yield return new WaitForEndOfFrame();
            }
        }
    }
    private void Start()
    {
        startPos = transform.position;
        endPos = startPos + new Vector3(5, 0, 0);
        StartCoroutine("Move");
    }
}



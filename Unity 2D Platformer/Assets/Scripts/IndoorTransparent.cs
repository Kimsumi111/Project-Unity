using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorTransparent : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public GameObject player;
    public float maxDistance;
    public float minDistance;
    public float maxAlpha;
    public float minAlpha;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        float alpha = Mathf.Lerp(maxAlpha, minAlpha, Mathf.InverseLerp(minDistance, maxDistance, distance));
        SetTransparency(alpha);
    }
    void SetTransparency(float alpha){
        spriteRenderer.color = new Color(1, 1, 1, alpha);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Vector2[] stageCenters;
    public Vector2[] stageMapSizes;

    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    Vector3 cameraPosition;
    
    [SerializeField]
    Vector2 center;
    [SerializeField]
    Vector2 mapSize;

    [SerializeField]
    float height;
    [SerializeField]
    float width;
    
    void Awake()
    {
        stageCenters = new Vector2[2];
        stageCenters[0] = new Vector2(-18, 0);
        stageCenters[1] = new Vector2(28, -13);
        
        stageMapSizes = new Vector2[2];
        stageMapSizes[0] = new Vector2(51, 9);
        stageMapSizes[1] = new Vector2(53, 21);

        //플레이어 transform 가져오기
        playerTransform = GameObject.Find("Player").transform;
        
        //현재 카메라 비추는 구역의 높이/2, 너비/2 값
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;

        if (playerTransform == null){
            Debug.LogError("Player를 찾을 수 없습니다!");
        }
    }
    void FixedUpdate()
    {
        //스테이지마다 center 변경
        center = GetStageCenter();
        //스테이지마다 mapSize 변경
        mapSize = GetStageMapSize();
        //카메라 영역 제한
        LimitCameraArea(mapSize, center); 
        //카메라가 플레이어 추적
        transform.position = playerTransform.position + cameraPosition;
    }

    Vector2 GetStageMapSize(){
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mapSize = stageMapSizes[gameManager.stageIndex];
        return mapSize;
    }
    Vector2 GetStageCenter(){
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        center = stageCenters[gameManager.stageIndex];
        return center;
    }

    void LimitCameraArea(Vector2 mapSize, Vector2 center){
        float lx = mapSize.x - width;
        float clampX = Mathf.Clamp(transform.position.x, center.x - lx, center.x + lx);

        float ly = mapSize.y - height;
        float clampY = Mathf.Clamp(transform.position.y, center.x - ly, center.y + ly);

        transform.position = new Vector3(clampX, clampY, -10f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize*2);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogDie : EnemyDie{
    protected override void MoveControl(){
        base.MoveControl();
        FrogMove frogMove = GetComponent<FrogMove>();
            if (frogMove != null)
                frogMove.enabled = false;  
            else
                Debug.LogWarning("FrogMove 스크립트가 없습니다.");
    }
}
    


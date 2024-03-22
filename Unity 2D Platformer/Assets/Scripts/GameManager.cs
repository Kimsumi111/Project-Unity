using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;
    public Image[] UIhealth;
    public Text UIPoint;
    public GameObject UIRestartBtn;
    BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }
    public void NextStage()
    {
        //Change Stage
        if(stageIndex < Stages.Length-1){
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            SetCollider(stageIndex);
            PlayerReposition();
        }
        else { //Game Clear
            //Player Control Lock
            Time.timeScale = 0;
            //Result UI
            Debug.Log("게임 클리어!");
            //Restart Button UI => Clear Button
            Text btnText = UIRestartBtn.GetComponentInChildren<Text>();
            btnText.text = "Clear!";
            UIRestartBtn.SetActive(true);
        }

        //Calculate Point
        totalPoint += stagePoint;
        stagePoint = 0;
    }
    
    public void HealthDown(){
        health--;
        UIhealth[health].color = new Color(0, 0, 0, 0.2f);
        
        if(health < 1){    
            //Player Die Effect
            player.OnDie();
            //Retry Button UI
            ViewBtn();
        }
    }
    public void HealthUp(){
        if(health == 3){
            stagePoint += 200;
        }
        else if(health < 3 && health > 0){
            health++;
            UIhealth[health-1].color = new Color(1, 1, 1, 1);
        }
    }    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"){
            if(health > 1){
                //Player Reposition
                PlayerReposition();
                //이 로직은 플레이어 시작지점이 반드시 (0, 0) 이어야 가능!
            }
            //Health Down
            HealthDown();
        }
    }
    void PlayerReposition(){
        player.transform.position = new Vector3(0,0,0);
        player.VelocityZero();
    }
    void ViewBtn(){
        UIRestartBtn.SetActive(true);
    }
    public void Restart(){
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    void SetCollider(int stageIndex){
        if(stageIndex == 1)
            boxCollider.enabled = false;
    }
}

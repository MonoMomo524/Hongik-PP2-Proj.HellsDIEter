using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightPuzzleController : MonoBehaviour
{
    public static int level = 1;
    public GameObject Scale;
    private GameObject player;
    private int goalSlimes = 2;
    public int GoalSlimes
    {
        get { return goalSlimes; }
        set { goalSlimes = value; }
    }
    private float timer = 120;
    public float Timer
    {
        get { return timer; }
    }
    private bool isClear = false;
    
    void Start()
    {
        // 체중 퍼즐 스테이지에서는 마우스를 화면 중앙에 고정함
        player = GameObject.Find("Player");
        //player.GetComponent<Player>().Usable = false;

        switch (level)
        {
            case 2:
                goalSlimes = 2;
                timer = 90;
                break;
            case 3:
                goalSlimes = 3;
                timer = 90;
                break;
            default:
                break;
        }

        Debug.Log("GOAL: " + goalSlimes);
    }

    private void Update()
    {
        if (isClear == false)
        {
            timer -= Time.deltaTime;
        }

        if(timer<0)
        {
            timer = 0;
        }

        if (goalSlimes == 0)  // && goal_turtles == 0)
        {
            this.transform.Find("Scale").transform.Find("Rocket").gameObject.SetActive(true);
            isClear = true;
        }
    }
}

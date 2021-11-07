using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightPuzzleController : MonoBehaviour
{
    public static int wLevel = 1;
    public static bool result = false;
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
    private bool gameOver = false;
    
    void Start()
    {
        player = GameObject.Find("Player");

        switch (WeightPuzzleController.wLevel)
        {
            case 1:
                goalSlimes = 2;
                timer = 90;
                GameObject.Find("Slime (5)").SetActive(false);
                break;
            case 2:
                goalSlimes = 3;
                timer = 90;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            isClear = true;
            goalSlimes = 0;
        }

        if (isClear == false)
        {
            timer -= Time.deltaTime;
        }

        if(timer<0)
        {
            timer = 0;
            gameOver = true;
        }

        if (gameOver == true)
        {
            StartCoroutine(EndGame());
        }
        

        if (goalSlimes == 0 && isClear == false)  // && goal_turtles == 0)
        {
            this.transform.Find("Scale").transform.Find("Rocket").gameObject.SetActive(true);
            GameObject.Find("UI").transform.Find("GameResult").gameObject.SetActive(true);
            GameObject.Find("UI").transform.Find("GameResult").transform.GetChild(2).gameObject.SetActive(true);
            isClear = true;
            if (WeightPuzzleController.wLevel == 1)
            {
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 50);
                PlayerPrefs.SetInt("SaveData", PlayerPrefs.GetInt("SaveData") + 1);
                WeightPuzzleController.result = true;
}
            else
            {
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 100);
                PlayerPrefs.SetInt("SaveData", PlayerPrefs.GetInt("SaveData") + 1);
                WeightPuzzleController.result = true;
            }

            WeightPuzzleController.wLevel++;
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        if (!isClear)
            StopCoroutine(EndGame());
        else
            isClear = false;

        GameObject.Find("GameManager").GetComponent<AudioSource>().Stop();
        int i = 0;
        while (i < 6)
        {
            yield return new WaitForSecondsRealtime(1.0f);
            i++;
        }

        SceneLoader.Instance.LoadScene("2.Main");
    }
}

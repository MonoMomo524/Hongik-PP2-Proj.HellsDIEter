using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Count"))
            return;

        playerScript = FindObjectOfType<Player>();
        if (SceneManager.GetActiveScene().buildIndex == 3)
            playerScript.transform.position = GameObject.Find("SavePoint0").transform.position;

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            SetPosition();
        }

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            switch (PlayerPrefs.GetInt("Dumb"))
            {
                case 3:
                    Destroy(GameObject.Find("dumbbell1").gameObject);
                    Destroy(GameObject.Find("dumbbell2").gameObject);
                    Destroy(GameObject.Find("dumbbell3").gameObject);
                    break;
                case 2:
                    Destroy(GameObject.Find("dumbbell1").gameObject);
                    Destroy(GameObject.Find("dumbbell2").gameObject);
                    break;
                case 1:
                    Destroy(GameObject.Find("dumbbell1").gameObject);
                    break;
            }
            if (PlayerPrefs.GetInt("Map") == 1)
            {
                Destroy(GameObject.Find("Book"));
            }
            if (PlayerPrefs.GetInt("Key") == 1)
            {
                Destroy(GameObject.Find("Key"));
            }

            if (PlayerPrefs.GetInt("Fuel") > 10)
            {
                Destroy(GameObject.Find("Fuel"));
                playerScript.gameObject.transform.position = GameObject.Find("SavePoint1").transform.position;
            }
        }

        SetState();
    }

    // 플레이어의 기존 상태에 맞게 세팅해주는 메소드
    private void SetState()
    {
        playerScript.CoinCounts = PlayerPrefs.GetInt("Coin");
        playerScript.DumCounts = PlayerPrefs.GetInt("Dumb");
        playerScript.MaxFuel = PlayerPrefs.GetInt("Fuel");
        playerScript.MinWeight = PlayerPrefs.GetInt("Min");
        playerScript.Weight = PlayerPrefs.GetInt("Weight");

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (PlayerPrefs.HasKey("Key") && PlayerPrefs.GetInt("Key") == 1)
            {
                Destroy(GameObject.Find("Key").gameObject);
                playerScript.HasKey = true;
            }
            else
                playerScript.HasKey = false;

            if (PlayerPrefs.GetInt("Map") == 1)
            {
                playerScript.HasMap = true;
            }
            else
                playerScript.HasMap = false;
        }
    }

    private void SetPosition()
    {
        // P1 실패
        if (PlayerPrefs.GetInt("Puzzle") == 1 && PlayerPrefs.GetInt("Count") == 0)
        {
            playerScript.gameObject.transform.position = GameObject.Find("SavePoint1").transform.position;
        }
        // P1 성공
        if (PlayerPrefs.GetInt("Count") >= 1)
        {
            playerScript.gameObject.transform.position = GameObject.Find("LoadPoint1").transform.position;
            GameObject.Find("B3").transform.Find("Room2").transform.Find("PannelTrigger").gameObject.SetActive(false);
        }

        // W1 실패
        if (PlayerPrefs.GetInt("Puzzle") == 0 && PlayerPrefs.GetInt("Count") == 1)
        {
            playerScript.gameObject.transform.position = GameObject.Find("SavePoint2").transform.position;
        }
        // W1 성공
        if (PlayerPrefs.GetInt("Count") >= 2)
        {
            playerScript.gameObject.transform.position = GameObject.Find("LoadPoint2").transform.position;
            GameObject.Find("B2").transform.Find("Room4").transform.Find("WeightTrigger").gameObject.SetActive(false);
        }

        // P2 실패
        if (PlayerPrefs.GetInt("Puzzle") == 1 && PlayerPrefs.GetInt("Count") == 2)
        {
            playerScript.gameObject.transform.position = GameObject.Find("LoadPoint3").transform.position;
        }
        // P2 성공
        if (PlayerPrefs.GetInt("Count") >= 3)
        {
            playerScript.gameObject.transform.position = GameObject.Find("LoadPoint3").transform.position;
            GameObject.Find("B2").transform.Find("Room7").transform.Find("PannelTrigger").gameObject.SetActive(false);
        }

        // W2 실패
        if (PlayerPrefs.GetInt("Puzzle") == 0 && PlayerPrefs.GetInt("Count") == 3)
        {
            playerScript.gameObject.transform.position = GameObject.Find("SavePoint1").transform.position;
        }
        // W2 성공
        if (PlayerPrefs.GetInt("Count") >= 4)
        {
            playerScript.gameObject.transform.position = GameObject.Find("LoadPoint4").transform.position;
            GameObject.Find("B3").transform.GetChild(7).transform.Find("WeightTrigger").gameObject.SetActive(false);
        }
    }
}

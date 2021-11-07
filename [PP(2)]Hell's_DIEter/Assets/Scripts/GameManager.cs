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
        if (!PlayerPrefs.HasKey("SaveData"))
            return;

        playerScript = FindObjectOfType<Player>();
        if (SceneManager.GetActiveScene().name.Contains("2.Main"))
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
                    GameObject.Find("dumbbell1").gameObject.SetActive(false);
                    GameObject.Find("dumbbell2").gameObject.SetActive(false);
                    GameObject.Find("dumbbell3").gameObject.SetActive(false);
                    break;
                case 2:
                    GameObject.Find("dumbbell1").gameObject.SetActive(false);
                    GameObject.Find("dumbbell2").gameObject.SetActive(false);
                    break;
                case 1:
                    GameObject.Find("dumbbell1").gameObject.SetActive(false);
                    break;
            }

            if (PlayerPrefs.GetInt("Map") == 1)
            {
                GameObject.Find("Book").gameObject.SetActive(false);
            }

            if (PlayerPrefs.GetInt("Key") == 1)
            {
                GameObject.Find("Key").gameObject.SetActive(false);
                playerScript.HasKey = true;
            }
            else
                playerScript.HasKey = false;

            if (PlayerPrefs.GetInt("Fuel") > 10)
            {
                GameObject.Find("Fuel").gameObject.SetActive(false);
                playerScript.gameObject.transform.position = GameObject.Find("SavePoint1").transform.position;
                playerScript.HasMap = true;
            }
            else
                playerScript.HasMap = false;
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
    }

    private void SetPosition()
    {
        switch (PlayerPrefs.GetInt("Recent"))
        {
            case 0:
                playerScript.gameObject.transform.position = GameObject.Find("SavePoint0").transform.position;
                break;

            case 1:
                // P1 실패
                if (PanelPuzzleController.result == false)
                {
                    Vector3 LoadPos = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));
                    playerScript.gameObject.transform.position = GameObject.Find("SavePoint1").transform.position;
                }
                // P1 성공
                else
                {
                    playerScript.gameObject.transform.position = GameObject.Find("LoadPoint1").transform.position;  // P1 발동한 위치
                    GameObject.Find("B3").transform.Find("Room2").transform.Find("PannelTrigger").gameObject.SetActive(false);
                    PanelPuzzleController.result = false;
                }
                break;

            case 2:
                // W1 실패
                if (WeightPuzzleController.result == false)
                {
                    playerScript.gameObject.transform.position = GameObject.Find("SavePoint2").transform.position;
                }
                // W1 성공
                else
                {
                    playerScript.gameObject.transform.position = GameObject.Find("LoadPoint2").transform.position;
                    GameObject.Find("B2").transform.Find("Room4").transform.Find("WeightTrigger").gameObject.SetActive(false);
                    WeightPuzzleController.result = false;
                }
                break;

            case 3:
                // P2 실패
                if (PanelPuzzleController.result == false)
                {
                    playerScript.gameObject.transform.position = GameObject.Find("LoadPoint2").transform.position;
                }
                // P2 성공
                else
                {
                    playerScript.gameObject.transform.position = GameObject.Find("LoadPoint3").transform.position;
                    GameObject.Find("B2").transform.Find("Room7").transform.Find("PannelTrigger").gameObject.SetActive(false);
                    PanelPuzzleController.result = false;
                }
                break;

            case 4:
                // W2 실패
                if (WeightPuzzleController.result == false)
                {
                    playerScript.gameObject.transform.position = GameObject.Find("SavePoint1").transform.position;
                }
                // W2 성공
                else
                {
                    playerScript.gameObject.transform.position = GameObject.Find("LoadPoint4").transform.position;
                    GameObject.Find("B3").transform.GetChild(7).transform.Find("WeightTrigger").gameObject.SetActive(false);
                    WeightPuzzleController.result = false;
                }
                break;

            default:
                playerScript.gameObject.transform.position = GameObject.Find("SavePoint0").transform.position;
                break;
        }

       

        
    }
}

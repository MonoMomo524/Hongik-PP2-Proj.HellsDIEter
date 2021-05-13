using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// 객체가 특정 이벤트를 받을 수 있도록 하는 클래스
public class HotKeys : MonoBehaviour
{
    public GameObject dumb;
    public GameObject fuel;

    private void Update()
    {
        // HOT KEYS
        // 1. 씬 이동
        if (Input.GetKeyDown(KeyCode.F1))
            SceneManager.LoadScene(0);

        else if (Input.GetKeyDown(KeyCode.F2))
            SceneManager.LoadScene(1);

        else if (Input.GetKeyDown(KeyCode.F3))
            SceneManager.LoadScene(2);

        else if (Input.GetKeyDown(KeyCode.F4))
            SceneManager.LoadScene(3);

        else if (Input.GetKeyDown(KeyCode.F5))
            SceneManager.LoadScene(4);

        else if (Input.GetKeyDown(KeyCode.F6))
            SceneManager.LoadScene(5);

        // 덤벨 획득
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            GameObject player = GameObject.Find("Player");
            Instantiate(dumb, player.transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            GameObject player = GameObject.Find("Player");
            Instantiate(fuel, player.transform.position, Quaternion.identity);
        }
    }
}

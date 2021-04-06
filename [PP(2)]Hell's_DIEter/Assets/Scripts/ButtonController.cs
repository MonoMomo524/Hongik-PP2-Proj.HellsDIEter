using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonController : MonoBehaviour
{
    public Sprite[] buttons;    // 버튼 스프라이트 저장
    public GameObject menu;
    bool isOpened = false;          // 인벤토리 창이 열려있는지
    bool isPaused = false;
    bool isListening = true;

    // 인벤토리 창 열기/닫기
    public void Inventory()
    {
        if (isOpened == true)
        {
            RectTransform inventory = GameObject.Find("Inventory").GetComponent<RectTransform>();
            inventory.pivot = new Vector2(0.5f, 0.85f);
            inventory.position = Vector3.Lerp(inventory.position, new Vector3(inventory.position.x, 0.0f), 3.0f);

            this.GetComponent<Image>().sprite = buttons[0];
            isOpened = false;
        }
        else
        {
            RectTransform inventory = GameObject.Find("Inventory").GetComponent<RectTransform>();
            inventory.pivot = new Vector2(0.5f, 0.0f);
            inventory.position = Vector3.Lerp(inventory.position, new Vector3(inventory.position.x, 0.0f), 3.0f);

            this.GetComponent<Image>().sprite = buttons[1];
            isOpened = true;
        }

        return;
    }

    public void MenuPopUp()
    {
        isPaused = !isPaused;

        // 일시정지 및 버튼 스프라이트 변경
        if (isPaused)
        {
            Time.timeScale = 0.0f;
            this.GetComponent<Image>().sprite = Resources.Load("UI/Sprite/Buttons/button_play", typeof(Sprite)) as Sprite;
        }
        else
        {
            Time.timeScale = 1.0f;
            this.GetComponent<Image>().sprite = Resources.Load("UI/Sprite/Buttons/button_pause", typeof(Sprite)) as Sprite;
        }

        // 메뉴 팝업 띄우기
        menu.gameObject.SetActive(isPaused);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void SoundControll()
    {
        isListening = !isListening;
        AudioSource[] soundObjects = GameObject.FindObjectsOfType<AudioSource>();
        if(soundObjects != null)
        {
            foreach (var item in soundObjects)
            {
                item.mute = !isListening;
                if (item.mute == false)
                {
                    this.GetComponent<Image>().sprite = Resources.Load("UI/Sprite/Buttons/button_soundOFF", typeof(Sprite)) as Sprite;
                    this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
                }
                    
                else
                {
                    this.GetComponent<Image>().sprite = Resources.Load("UI/Sprite/Buttons/button_soundON", typeof(Sprite)) as Sprite;
                    this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
                }
                    
                Debug.Log("Turn Off Audio: " + item.mute);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public CanvasGroup dialogueGroup;
    public int limit;

    private Text sentence;
    private GameObject nextText;

    private void Start()
    {
        sentence = GameObject.Find("UI").transform.Find("Dialogue").GetChild(0).GetComponent<Text>();
        nextText = GameObject.Find("UI").transform.Find("Dialogue").GetChild(1).gameObject;
    }

    private void OnDoorwayOpen(float weight)
    {
        if ((int)weight > limit)
        {
            ShowDoorInfo();
            return;
        }

        this.gameObject.SetActive(false);
    }

    private void OnDoorwayClose()
    {
        this.gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            float weight = collision.transform.GetComponent<Player>().Weight;
            OnDoorwayOpen(weight);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            OnDoorwayClose();
        }
    }

    private void OnMouseDown()
    {
        dialogueGroup.alpha = 1;
        dialogueGroup.blocksRaycasts = true;    // 마우스 이벤트 감지
        OnDoorwayOpen(GameObject.FindObjectOfType<Player>().Weight);
    }

    private void ShowDoorInfo()
    {
        // 안내창 띄우기
        sentence.text = "여길 지나가고 싶거든 " + limit + "kg 이하로 감량해라!\n-앙마-\n...라고 쓰여있다.";
        nextText.SetActive(true);
    }

    private void CloseWindow()
    {
        dialogueGroup.alpha = 0;
        dialogueGroup.blocksRaycasts = false;
        nextText.SetActive(false);
    }
}

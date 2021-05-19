using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public CanvasGroup dialogueGroup;
    public int limit;
    public bool isSpecial;

    private Text sentence;
    private GameObject nextText;

    private void Start()
    {
        sentence = GameObject.Find("UI").transform.Find("Dialogue").GetChild(0).GetComponent<Text>();
        nextText = GameObject.Find("UI").transform.Find("Dialogue").GetChild(1).gameObject;
    }

    private void OnDoorwayOpen(float weight)
    {
        if( (int) weight != limit && isSpecial==true)
        {
            ShowDoorInfo(1);
            return;
        }
        if ((int)weight > limit)
        {
            ShowDoorInfo(0);
            return;
        }

        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<MeshCollider>().enabled = false;
    }

    private void OnDoorwayClose()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<MeshCollider>().enabled = true;
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

    private void ShowDoorInfo(int type)
    {
        dialogueGroup.alpha = 1;
        dialogueGroup.blocksRaycasts = true;    // 마우스 이벤트 감지

        switch (type)
        {
            case 0:
                // 안내창 띄우기
                sentence.text = " ";
                sentence.text = "여길 지나가고 싶거든 " + limit + "kg 이하로 감량해라!\n-앙마-\n...라고 쓰여있다.";
                nextText.SetActive(true);
                break;
            case 1:
                // 안내창 띄우기
                sentence.text = " ";
                sentence.text = "여길 지나가고 싶거든 " + limit + "kg이 되어 오너라! !\n-앙마-\n...라고 쓰여있다.";
                nextText.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void CloseWindow()
    {
        dialogueGroup.alpha = 0;
        dialogueGroup.blocksRaycasts = false;
        nextText.SetActive(false);
    }
}

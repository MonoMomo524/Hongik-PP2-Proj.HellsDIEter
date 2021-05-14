using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    public CanvasGroup dialogueGroup;
    public GameObject fuel;

    private Player player;
    private Animator anim;
    private GameObject icon;
    private GameObject nextText;
    private Text sentence;

    private void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Player>();
        anim = GetComponent<Animator>();
        sentence = GameObject.Find("UI").transform.Find("Dialogue").GetChild(0).GetComponent<Text>();
        nextText = GameObject.Find("UI").transform.Find("Dialogue").GetChild(1).gameObject;
        icon = this.transform.GetChild(0).gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            icon.SetActive(true);

            // 플레이어가 어디서 아이콘을 보더라도 같은 모습으로 보이도록 회전
            Vector3 vec = other.transform.position - icon.transform.position;
            vec.Normalize();
            Quaternion q = Quaternion.LookRotation(vec);
            q.x = 0.0f;
            q.z = 0.0f;
            icon.transform.rotation = q;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            icon.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        dialogueGroup.alpha = 1;
        dialogueGroup.blocksRaycasts = true;    // 마우스 이벤트 감지
        StartCoroutine(GetReward(player.CoinCounts));
    }

    IEnumerator GetReward(int coins)
    {
        if (coins >= 30)
        {
            // 상자가 열리는 애니메이션
            anim.SetBool("IsOpen", true);
            yield return new WaitUntil(IsOpen);

            // 연료 보상
            Vector3 pos = Vector3.back * 1.5f;
            Instantiate(fuel, this.transform.position + pos, Quaternion.identity);
        }
        else
        {
            // 안내창 띄우기
            sentence.text = "100원을 투입하시오...라고 써져 있다.";
            nextText.SetActive(true);
            yield return null;
        }
    }

    private bool IsOpen()
    {
        // 애니메이션이 모두 진행됐는지
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            return true;
        else return false;
    }

    //private void CloseWindow()
    //{
    //    dialogueGroup.alpha = 0;
    //    dialogueGroup.blocksRaycasts = false;
    //    nextText.SetActive(false);
    //}
}

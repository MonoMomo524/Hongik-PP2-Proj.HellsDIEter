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

    private bool isLoading = false;
    private bool isOpen = false;

    private void Start()
    {
        PlayerPrefs.SetInt("MainChest", 0);
        player = FindObjectOfType<Player>().GetComponent<Player>();
        anim = GetComponent<Animator>();
        sentence = GameObject.Find("UI").transform.Find("Dialogue").GetChild(0).GetComponent<Text>();
        nextText = GameObject.Find("UI").transform.Find("Dialogue").GetChild(1).gameObject;
        if (this.tag == "Ground")
            icon = this.transform.GetChild(1).gameObject;
        else
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
        if (this.name == "Chest")
            StartCoroutine(GetReward(player.CoinCounts));
        else
            StartCoroutine(TurnOnSystem(player.HasKey));
    }

    IEnumerator GetReward(int coins)
    {
        if (isOpen)
            StopCoroutine("GetReward");

        if (coins >= 100 && isOpen == false)
        {
            isOpen = true;

            // 안내창 띄우기
            sentence.text = " ";
            sentence.text = "상자에서 연료를 얻었다.";

            // 상자가 열리는 애니메이션
            anim.SetBool("IsOpen", true);
            yield return new WaitUntil(IsOpen);

            // 연료 보상
            Vector3 pos = Vector3.back * 1.5f;
            Instantiate(fuel, this.transform.position + pos, Quaternion.identity);
            yield return null;
        }
        else if (coins < 100)
        {
            // 안내창 띄우기
            sentence.text = " ";
            sentence.text = "100원을 투입하시오...라고 써져 있다.";
            nextText.SetActive(true);
        }

        yield return null;
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

    IEnumerator TurnOnSystem(bool hasKey)
    {
        if (hasKey==true && isLoading==false)
        {
            isLoading = true;
            sentence.text = " ";
            sentence.text = "작동한다!";

            // 가동 소리

            // 바람 작동
            GameObject.Find("B2").transform.Find("Room4").transform.Find("Exit Wind Particle").gameObject.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            SceneLoader.Instance.LoadScene("EndingScene");
        }
        else
        {
            // 안내창 띄우기
            sentence.text = " ";
            sentence.text = "작동시킬 뭔가가 필요한 것 같아...";
            nextText.SetActive(true);
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    private Player player;
    private Animator anim;

    private delegate void ChestCheckPoint();
    private ChestCheckPoint checker;

    private Text text;

    private void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Player>();
        anim = GetComponent<Animator>();
        text = GameObject.Find("UI").transform.Find("Dialogue").GetChild(0).GetComponent<Text>();
    }

    private void OnMouseDown()
    {
        StartCoroutine(GetReward(player.CoinCounts));
    }

    private bool IsOpen()
    {
        // 애니메이션이 모두 진행됐는지
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            return true;
        else return false;
    }

    IEnumerator GetReward(int coins)
    {
        if (coins >= 30)
        {
            // 상자가 열리는 애니메이션
            anim.SetBool("IsOpen", true);
            yield return new WaitUntil(IsOpen);

            // 플레이어 연료 늘리기
            player.MaxFuel += 10;
        }
        else
        {
            // 안내창 띄우기

        }
    }
}

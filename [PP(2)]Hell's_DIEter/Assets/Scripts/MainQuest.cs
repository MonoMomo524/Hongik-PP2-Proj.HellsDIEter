using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainQuest : MonoBehaviour
{
    private Player player;
    private bool isDone;    // 한 퀘스트 내 세부 퀘스트 성공 여부(순서가 중요할 때)
    private GameObject questClearPanel;

    private delegate void CheckPoint(int i);
    private CheckPoint checkerDelegate;
    private int value;

    private List<TextMeshProUGUI> missions;

    // Start is called before the first frame update
    void Start()
    {
        if (!SceneManager.GetActiveScene().name.Contains("Main"))
        {
            return;
        }


        // 외부 참조 세팅
        player = GameObject.Find("Player").GetComponent<Player>();
        questClearPanel = GameObject.Find("UI").transform.Find("QuestClearPanel").gameObject;
        questClearPanel.SetActive(false);

        // 미션 생성
        missions = new List<TextMeshProUGUI>();
        for (int i = 0; i < 3; i++)
        {
            TextMeshProUGUI mission = GameObject.Find("UI").transform.Find("MissionPanel").transform.Find("Mission Text" + (i + 1) + " (TMP)").GetComponent<TextMeshProUGUI>();
            if (i == 0)
                mission.text = "덤벨을 찾기";
            else if (i == 1)
                mission.text = "90kg까지 감량하기";
            else
                mission.text = "";
            missions.Add(mission);
        }

        missions[0].color = Color.white;
        missions[1].color = Color.white;
        missions[2].color = Color.white;

        checkerDelegate = null; ;
        isDone = false;
        value = 0;

        StartCoroutine(MainStage());
    }

    // Update is called once per frame
    void Update()
    {
        checkerDelegate?.Invoke(value);
    }

    // 메인 던전 스테이지에서만 나타나는 미션 패널
    IEnumerator MainStage()
    {
        // 덤벨1 획득하기
        value = 0;
        checkerDelegate = CheckDumbells;
        yield return new WaitUntil(IsDone);
        missions[0].color = Color.grey;
        isDone = false;

        // 체중 감량하기
        value = 90;
        checkerDelegate = CheckWeights;
        yield return new WaitUntil(IsDone);
        missions[1].color = Color.grey;
        isDone = false;

        // 첫 번째 스테이지 클리어 하기
        checkerDelegate = CheckClear;
        yield return new WaitUntil(IsDone);
        missions[0].color = Color.grey;
        isDone = false;

        // 100kg까지 다시 찌우기
        checkerDelegate = CheckDumbells;
        yield return new WaitUntil(IsDone);
        missions[0].color = Color.grey;
        isDone = false;

        // 두 번째 스테이지 클리어 하기
        checkerDelegate = CheckDumbells;
        yield return new WaitUntil(IsDone);
        missions[0].color = Color.grey;
        isDone = false;

        // 덤벨2 획득하기

        // 세 번째 스테이지 클리어 하기

        // 덤벨3 획득하기

        // 네번째 

    }

    private bool IsDone()
    {
        return isDone;
    }

    // 덤벨 개수 확인하기
    private void CheckDumbells(int count)
    {
        if (player.DumCounts > count)
        {
            isDone = true;
        }

        return;
    }

    // 체중 확인하기
    private void CheckWeights(int weight)
    {
        if (player.Weight == weight)
        {
            isDone = true;
        }

        return;
    }

    private void CheckClear(int stage)
    {
        //if(PlayerPrefs.GetInt())
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialQuest : MonoBehaviour
{
    private Player player;
    private NPCDialogue dialogue;
    private bool isClear;
    private bool isDone;    // 한 퀘스트 내 세부 퀘스트 성공 여부(순서가 중요할 때)
    private GameObject questClearPanel;

    public GameObject dumb;
    public GameObject food;
    public GameObject drop;

    private delegate void TutorialCheckPoint();
    private TutorialCheckPoint checker;

    private List<TextMeshProUGUI> missions;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        PlayerPrefs.SetInt("Tutorial", 0);
        player = GameObject.Find("Player").GetComponent<Player>();
        dialogue = GameObject.Find("Devil NPC").GetComponent<NPCDialogue>();
        questClearPanel = GameObject.Find("UI").transform.Find("QuestClearPanel").gameObject;
        questClearPanel.SetActive(false);

        missions = new List<TextMeshProUGUI>();
        for (int i = 0; i < 3; i++)
        {
            TextMeshProUGUI mission = GameObject.Find("UI").transform.Find("MissionPanel").transform.Find("Mission Text" + (i + 1) + " (TMP)").GetComponent<TextMeshProUGUI>();
            if (i==0)
                mission.text = "- 악마를 클릭하여 대화하기";
            else
                mission.text = "";
            missions.Add(mission);
        }

        checker = null;
        isClear = false;

        StartCoroutine(Tutorial());
    }

    // Update is called once per frame
    void Update()
    {
        checker?.Invoke();
    }

    IEnumerator Tutorial()
    {
        while (dialogue.index < 5)
            yield return new WaitForSeconds(0.5f);

        #region Quest 1. 움직이기
        yield return new WaitUntil(CheckDialogue);
        missions[0].color = Color.white;
        missions[0].text = "Ctrl 누르기";
        missions[1].color = Color.white;
        missions[1].text = "WASD 조작하기";
        missions[1].color = Color.white;
        missions[2].text = "SPACE를 꾹 눌러 제트팩 사용하기";

        checker = CheckLockingMouse;
        yield return new WaitUntil(IsDone);
        missions[0].color = Color.grey;
        isDone = false;

        checker = CheckMovements;
        yield return new WaitUntil(IsDone);
        missions[1].color = Color.grey;
        isDone = false;

        checker = CheckUsingJetpack;
        yield return new WaitUntil(IsClear);
        missions[2].color = Color.grey;
        dialogue.DialogueBubble.SetActive(true);
        dialogue.IsClear = true;
        #endregion

        #region Quest 2. 감량하기

        while (dialogue.index < 8)
            yield return new WaitForSeconds(0.5f);
        Instantiate(dumb, drop.transform.position, Quaternion.identity);

        while (dialogue.index < 9)
            yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(CheckDialogue);

        missions[0].color = Color.white;
        missions[0].text = "덤벨을 줍고 우클릭을 여러번 눌러 감량하기";
        missions[1].color = Color.white;
        missions[1].text = "";
        missions[2].color = Color.white;
        missions[2].text = "";
        checker = CheckLosingWeight;
        yield return new WaitUntil(IsClear);

        dialogue.IsClear = true;
        missions[0].color = Color.grey;
        dialogue.DialogueBubble.SetActive(true);
        #endregion

        #region Quest 3. 증량하기
        checker = CheckGainingWeight;
        while (dialogue.index < 11)
            yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(CheckDialogue);
        Instantiate(food);
        missions[0].color = Color.white;
        missions[0].text = "솥단지의 빵을 먹기";
        yield return new WaitUntil(IsClear);

        dialogue.IsClear = true;
        missions[0].color = Color.grey;
        dialogue.DialogueBubble.SetActive(true);
        checker = null;
        #endregion

        // 씬 이동 전 당부
        while (dialogue.index < 14)
            yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(CheckDialogue);

        checker = EndTutorial;
        yield return new WaitUntil(IsClear);
        checker = null;

        // 씬 이동
        yield return new WaitForSeconds(3f);
        int idx = SceneManager.GetActiveScene().buildIndex;
        string path = SceneUtility.GetScenePathByBuildIndex(idx + 1);
        string name = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
        SceneLoader.Instance.LoadScene(name);
    }

    public bool IsClear()
    {
        return isClear;
    }

    public bool IsDone()
    {
        return isDone;
    }

    private bool CheckDialogue()
    {
        if (DialogueManager.instance.IsDone)
        {
            dialogue.DialogueBubble.SetActive(false);
            return true;
        }
        else
        {
            dialogue.DialogueBubble.SetActive(true);
            return false;
        }
    }

    private void CheckLockingMouse()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            isDone = true;
    }

    private void CheckMovements()
    {
        if (Input.GetAxis("Horizontal") > 0f || Input.GetAxis("Vertical") > 0f)
            isDone = true;
    }

    private void CheckUsingJetpack()
    {
        if (player.IsJetpackOn)
        {
            isDone = true;
            if (isClear == false)
            {
                isClear = true;
                StartCoroutine(ShowClearUI());
            }
        }
    }

    private void CheckLosingWeight()
    {
        if (player.Weight == 90)
        {
            isDone = true;
            if (isClear == false)
            {
                isClear = true;
                StartCoroutine(ShowClearUI());
            }
        }
        else
            isDone = false;
    }

    private void CheckGainingWeight()
    {
        if (player.Weight == 100)
        {
            isDone = true;
            if (isClear == false)
            {
                isClear = true;
                StartCoroutine(ShowClearUI());
            }
        }
    }

    private void EndTutorial()
    {
        if (dialogue.DialogueBubble.activeSelf == false)
        {
            isDone = true;
            if (isClear == false)
            {
                isClear = true;
                StartCoroutine(ShowClearUI());
            }
            PlayerPrefs.SetInt("Tutorial", 1);
        }
    }

    IEnumerator ShowClearUI()
    {
        // 랜덤으로 칭찬어구 출력
        int compliment = Random.Range(0, 5);
        switch (compliment)
        {
            case 0:
                questClearPanel.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Super!";
                break;
            case 1:
                questClearPanel.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Great!";
                break;
            case 2:
                questClearPanel.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Amazing!";
                break;
            case 3:
                questClearPanel.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Nice!";
                break;
            case 4:
                questClearPanel.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Awesome!";
                break;
            default:
                break;
        }
        questClearPanel.SetActive(true);
        yield return new WaitForSeconds(4f);

        questClearPanel.SetActive(false);
        isClear = false;
        isDone = false;
    }
}

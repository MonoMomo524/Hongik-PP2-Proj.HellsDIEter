using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialQuest : MonoBehaviour
{
    private Player player;
    private NPCDialogue dialogue;
    private bool isClear;
    private bool isDone;

    public GameObject dumb;
    public GameObject food;
    public GameObject drop;

    private delegate void TutorialCheckPoint();
    private TutorialCheckPoint checker;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        dialogue = GameObject.Find("Devil NPC").GetComponent<NPCDialogue>();

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

        // Quest 1. 움직이기
        yield return new WaitUntil(CheckDialogue);
        checker = CheckMovements;
        yield return new WaitUntil(IsDone);
        checker = CheckUsingJetpack;
        yield return new WaitUntil(IsClear);

        Debug.Log("OKAY!");
        dialogue.DialogueBubble.SetActive(true);
        dialogue.IsClear = true;

        // Quest 2. 감량하기
        while (dialogue.index < 8)
            yield return new WaitForSeconds(0.5f);
        Instantiate(dumb, drop.transform.position, Quaternion.identity);

        while (dialogue.index < 9)
            yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(CheckDialogue);
        this.isClear = false;
        checker = CheckLosingWeight;
        yield return new WaitUntil(IsClear);

        Debug.Log("OKAY!");
        dialogue.IsClear = true;
        dialogue.DialogueBubble.SetActive(true);
        this.isClear = false;

        // Quest 3. 증량하기
        checker = CheckGainingWeight;
        while (dialogue.index < 11)
            yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(CheckDialogue);
        Instantiate(food);
        yield return new WaitUntil(IsClear);

        Debug.Log("OKAY!");
        dialogue.IsClear = true;
        dialogue.DialogueBubble.SetActive(true);
        this.isClear = false;

        // 씬 이동 전 당부
        while (dialogue.index < 14)
            yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(CheckDialogue);

        checker = EndTutorial;
        yield return new WaitUntil(IsClear);

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

    private void CheckMovements()
    {
        if (Input.GetAxis("Horizontal") > 0f || Input.GetAxis("Vertical") > 0f)
            isDone = true;
    }

    private void CheckUsingJetpack()
    {
        if (isDone && player.IsJetpackOn)
            isClear = true;
    }

    private void CheckLosingWeight()
    {
        if (player.Weight == 90)
            isClear = true;
        else
            isClear = false;
    }

    private void CheckGainingWeight()
    {
        if (player.Weight == 100)
            isClear = true;
    }

    private void EndTutorial()
    {
        if(dialogue.DialogueBubble.activeSelf == false)
            isClear = true;
    }
}

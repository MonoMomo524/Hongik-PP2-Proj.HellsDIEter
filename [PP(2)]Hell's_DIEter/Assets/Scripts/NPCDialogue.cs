using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue
{
    public int id { get; set; }
    public string sentence { get; set; }
}

public class NPCDialogue : MonoBehaviour
{
    public GameObject DialogueBubble;

    public int id { get; set; }
    public int index { get; set; }
    public int level { get; set; }

    string[] line;
    List<string> sentences = new List<string>();
    List<string> ids = new List<string>();
    private Dictionary<int, string> TableIndexDic = new Dictionary<int, string>();

    private bool isClear = false;   // 이벤트 매니저를 통해 변경되는 변수
    public bool IsClear
    {
        get { return isClear; }
        set { isClear = value; }
    }

    private bool isFirst = true;    // 해달 레벨에서 처음 말을 걸은 것인지

    private void Start()
    {
        id = 1;
        level = 0;
        index = 0;
        SetTable();
        isClear = false;
        DialogueBubble.SetActive(true);
    }

    public void SetTable()
    {
        // csv 호출 및 불러오기
        TextAsset text = Resources.Load<TextAsset>("Dialogues");
        string content = text.text;

        // 행단위 구분
        line = content.Split('\n');

        for (int i = 2; i < line.Length; i++) // 0 ~ 1번 row는 사용하지 않음
        {
            string[] column = line[i].Split(',');
            int idx = 0;
            Dialogue table = new Dialogue();
            table.id = int.Parse(column[idx++]);
            table.sentence = column[idx++];
            
            // 대사 스크립트를 저장
            TableIndexDic.Add(table.id, table.sentence);
        }
    }

    // 클릭 이벤트가 감지되면 헤당 레벨에 맞는 범위의 스크립트를 전송
    private void OnMouseDown()
    {
        // 말하는 중이면 말 끊지 못하게 저지함
        if (DialogueManager.instance.IsTyping)
        {
            return;
        }

        // 1. 퀘스트가 처음인지
        if (DialogueManager.instance.IsDone)
        {
            if (isClear == true)
                isFirst = true;
        }
        else
        {
            DialogueManager.instance.Ondialogue(sentences.ToArray(), ids.ToArray(), level);
        }

        //2. 스크립트 관련
        // 해당 레벨에서 처음 말을 걸은 경우
        if (isFirst)
        {
            level++;
            isFirst = false;
            isClear = false;

            // 레벨에 맞는 스크립트 세팅
            SetScripts();

            // DialogueManager에게 스크립트 전송
            DialogueManager.instance.Ondialogue(sentences.ToArray(), ids.ToArray(), level);
            return;
        }

        // 해당 레벨에 맞는 튜토리얼을 끝마치치 못했다면 다시 말을 건 것이라 생각하고 처리함
        if (isClear == false || !DialogueManager.instance.IsDone)
        {
            
        }
        else
        {
            isFirst = true;
        }
    }

    private void SetScripts()
    {
        // 기존 스크립트 지우기
        sentences.Clear();

        int i = index;

        // 해당 레벨에 맞는 스크립트 넣기
        foreach (var item in TableIndexDic)
        {
            if (i > 0)
            {
                i--;
                continue;
            }

            // 스크립트 넣기
            sentences.Add(item.Value);
            ids.Add(item.Key.ToString());

            // 마지막 스크립트
            if  (index == 4 || index == 8 || index == 10 || index == 14)
            {
                index++;
                break;
            }

            index++;
        }
    }
}

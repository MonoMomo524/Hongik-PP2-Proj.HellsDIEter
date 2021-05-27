using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour, IPointerDownHandler
{
    public Text DialogueText;
    public Sprite[] Window;
    public GameObject NextText;
    public CanvasGroup dialogueGroup;

    private Queue<string> sentences;
    private Queue<string> ids;

    private string currentSentence;
    private int currentIndex;

    private float typingSpeed = 0.025f;

    private bool isTyping;
    public bool IsTyping
    {
        get { return isTyping; }
    }

    private bool isDone;    // 해당 레벨의 대사가 모두 끝났는지
    public bool IsDone
    {
        get { return isDone; }
    }
    private bool isClear = false;
    public bool IsClear
    {
        get { return isClear; }
    }

    int count;
    int level;
    int lastIndex;

    // 싱글톤
    public static DialogueManager instance;
    private void Awake()
    {
        instance = this;
        count = 0;
        level = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        ids = new Queue<string>();
    }

    // 대사 스크립트를 추가
    public void Ondialogue(string[] lines, string[] indexes, int level)
    {
        isDone = false; // 해당 레벨의 대사가 모두 끝났는지 (= 끝나지 않음)
        if (this.level < level)
        {
            this.level = level;
            lastIndex = currentIndex;
            count += lines.Length;
        }

        // 대사 저장
        sentences.Clear();
        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }

        // id 저장
        ids.Clear();
        foreach (string idx in indexes)
        {
            ids.Enqueue(idx);
        }

        dialogueGroup.alpha = 1;
        dialogueGroup.blocksRaycasts = true;    // 마우스 이벤트 감지

        // 첫번째 문장 출력
        NextSentence();
    }

    // 다음 문장을 출력할 스크립트로 설정
    public void NextSentence()
    {
        // 출력할 문장이 남아있다면
        if (sentences.Count != 0)
        {
            currentSentence = sentences.Dequeue();

            if (level == 1)
                currentIndex = int.Parse(ids.Dequeue());
            else
                currentIndex = int.Parse(ids.Dequeue()) + lastIndex;

            // 특정 스크립트에선 대화창 모양 변경
            if (currentIndex == 2 || currentIndex == 10 || currentIndex == 13)
                this.GetComponent<Image>().sprite = Window[1];
            else
                this.GetComponent<Image>().sprite = Window[0];

            // 타이핑 이펙트 코루틴
            StartCoroutine(Typing(currentSentence));
        }
        else
        {
            dialogueGroup.alpha = 0;
            dialogueGroup.blocksRaycasts = false;
            isDone = true;  // 할 말 다 끝남
        }
    }

    // 타이핑 연출 코루틴
    IEnumerator Typing(string line)
    {
        isTyping = true;
        NextText.SetActive(false);

        DialogueText.text = "";
        foreach(char letter in line.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    // 스테이지 클리어는 이벤트 매니저로 처리
    void Update()
    {
        // dialogueText == currentSentece == 대사 한 줄 끝남
        if(DialogueText.text.Equals(currentSentence))
        {
            NextText.SetActive(true);
            isTyping = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isTyping)
            NextSentence();
    }

    // 대화창 연출 메소드
    public void SetWindow(int type)
    {
        if(type == 0)
        {
            // 일반
            this.GetComponent<Image>().sprite = Window[0];
        }
        else
        {
            // 강조
            this.GetComponent<Image>().sprite = Window[1];
        }
    }
}

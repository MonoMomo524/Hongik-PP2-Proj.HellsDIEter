using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour, IPointerDownHandler
{
    public Text DialogueText;
    public GameObject NextText;
    public CanvasGroup dialogueGroup;
    private Queue<string> sentences;
    private string currentSentence;

    private float typingSpeed = 0.05f;
    private bool isTyping;
    public bool IsTyping
    {
        get { return isTyping; }
    }

    // 싱글톤
    public static DialogueManager instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void Ondialogue(string[] lines)
    {
        sentences.Clear();
        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }
        dialogueGroup.alpha = 1;
        dialogueGroup.blocksRaycasts = true;    // 마우스 이벤트 감지

        NextSentence();
    }

    public void NextSentence()
    {
        if (sentences.Count != 0)
        {
            currentSentence = sentences.Dequeue();

            // 코루틴
            isTyping = true;
            NextText.SetActive(false);
            StartCoroutine(Typing(currentSentence));
        }
        else
        {
            dialogueGroup.alpha = 0;
            dialogueGroup.blocksRaycasts = false;
        }
    }

    IEnumerator Typing(string line)
    {
        DialogueText.text = "";
        foreach(char letter in line.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    // Update is called once per frame
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
}

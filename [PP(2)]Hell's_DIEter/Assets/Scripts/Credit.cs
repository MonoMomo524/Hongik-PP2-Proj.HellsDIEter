using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Credit : MonoBehaviour
{
    public Image Background;
    public Image PanelImage;
    public Sprite[] sprite;
    public TextMeshProUGUI[] Text;
    [SerializeField]private bool isCredit;
    Vector3 pos;
    Vector3 start;

    private void Awake()
    {
        isCredit = false;
        start = Text[0].transform.position;
        pos = new Vector3(start.x, -start.y * 10, start.z);
    }

    private void OnEnable()
    {
        Text[0].transform.position = start;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name.Contains("Ending"))
            StartCoroutine(Ending());
        else
            isCredit = true;
    }

    private void Update()
    {
        if(isCredit)
            Text[0].transform.position = Vector3.MoveTowards(Text[0].transform.position, pos, Time.deltaTime*130f);
        if (SceneManager.GetActiveScene().name.Contains("Title"))
            if (Text[0].transform.position.y >= 2365f)
                GameObject.Find("UI").transform.Find("Panel").gameObject.SetActive(false);
    }

    IEnumerator Ending()
    {
        Background.sprite = sprite[0];
        Text[1].text = "여러 시험 끝에 당신은 저승의 문을 열었습니다.";
        yield return new WaitForSeconds(3.5f);

        Background.sprite = sprite[1];
        Text[1].text = "당신은 이승에서 다시 태어나게 될 것입니다.";
        yield return new WaitForSeconds(3.5f);

        Background.color = Color.black;
        PanelImage.color = Color.black;
        Text[1].transform.position = new Vector3(Screen.width / 2, Screen.height / 2);
        Text[1].text = "The End\n감사합니다!";
        yield return new WaitForSeconds(3);
        Text[1].color = Color.grey ;
        isCredit = true;
        yield return new WaitForSeconds(35);
        SceneLoader.Instance.LoadScene("0.Title");
    }


}

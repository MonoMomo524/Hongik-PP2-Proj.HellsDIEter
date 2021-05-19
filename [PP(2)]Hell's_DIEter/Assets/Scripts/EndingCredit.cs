using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingCredit : MonoBehaviour
{
    public Image Background;
    public Image PanelImage;
    public Sprite[] sprite;
    public TextMeshProUGUI[] Text;
    [SerializeField]private bool isCredit;
    Vector3 pos;
    Vector3 start;

    // Start is called before the first frame update
    void Start()
    {
        isCredit = false;
        start = Text[1].transform.position;
        pos = new Vector3(start.x, -start.y*3, start.z);
        StartCoroutine(Ending());
    }

    private void Update()
    {
        if(isCredit)
            Text[1].transform.position = Vector3.MoveTowards(Text[1].transform.position, pos, Time.deltaTime*100f);
    }

    IEnumerator Ending()
    {
        Background.sprite = sprite[0];
        Text[0].text = "여러 시험 끝에 당신은 저승의 문을 열었습니다.";
        yield return new WaitForSeconds(5);

        Background.sprite = sprite[1];
        Text[0].text = "당신은 이승에서 다시 태어나게 될 것 입니다.";
        yield return new WaitForSeconds(5);

        Background.color = Color.black;
        PanelImage.color = Color.black;
        Text[0].transform.position = new Vector3(Screen.width / 2, Screen.height / 2);
        Text[0].text = "The End";
        yield return new WaitForSeconds(3);
        Text[0].color = Color.grey ;
        isCredit = true;
        yield return new WaitForSeconds(10);
        SceneLoader.Instance.LoadScene("0.Title");

    }
}

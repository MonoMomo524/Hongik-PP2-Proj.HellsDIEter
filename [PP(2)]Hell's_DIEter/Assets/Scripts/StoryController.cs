using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Text, Image 등의UI관련 변수 등을 사용할수 있게됩니다
using UnityEngine.SceneManagement;
using TMPro;

public class StoryController : MonoBehaviour
{
    public Image CurrentImage; //기존에 존재하는 이미지
    public Sprite[] Cuts = new Sprite[3]; //바뀌어질 이미지
    private TextMeshProUGUI tmPro;
    private int index = 0;
    private bool done = false;

    private void Start()
    {
        index = 0;
        tmPro = transform.parent.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(Timer());
    }

    public void ChangeImage()
    {
        if (index < 3)
        {
            CurrentImage.sprite = Cuts[index];
        }
        else
        {
            CurrentImage.color = Color.black;
            RectTransform text = GameObject.Find("UI").transform.Find("StoryText").GetComponent<RectTransform>();
            text.position = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        }
    }

    IEnumerator Timer()
    {
        while (index < 7)
        {
            switch (index)
            {
                case 0:
                    tmPro.text = "행성 탐사에 나섰던 당신.";
                    break;
                case 1:
                    tmPro.text = "얼마 지나지 않아 당신은 큰 위험을 마주하게 됩니다.";
                    break;
                case 2:
                    tmPro.text = "바로 의문의 방사능 폭풍이었죠.";
                    break;
                case 3:
                    tmPro.text = "당신은 살아남았지만 심각한 부작용으로\n식욕을 주체할 수 없었습니다.";
                    break;
                case 4:
                    tmPro.text = "결국 모든 식량을 먹어 치운 당신은 머지않아 굶어 죽었고\n" +
                        "사후세계에 도착해서도 식욕을 참지 못해\n" +
                        "사후세계의 음식마저 먹어대기 시작했습니다.";
                    break;
                case 5:
                    tmPro.text = "사후세계의 일원들은 당신을 감당할 수 없었고\n당신을 시험하여 이승으로 돌려보내기로 합니다.";
                    break;
                default:
                    break;
            }
            ChangeImage();

            if(index>3)
            {
                yield return new WaitForSeconds(2.0f);
            }
            yield return new WaitForSeconds(3.0f);

            if(index == 5)
            {
                break;
            }
            index++;
        }

        yield return new WaitForSeconds(3.0f);

        int idx = SceneManager.GetActiveScene().buildIndex;
        string path = SceneUtility.GetScenePathByBuildIndex(idx + 1);
        string name = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
        SceneLoader.Instance.LoadScene(name);
    }
}

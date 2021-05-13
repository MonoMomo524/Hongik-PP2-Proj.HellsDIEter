using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Text, Image 등의UI관련 변수 등을 사용할수 있게됩니다
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour
{
    public Image CurrentImage; //기존에 존재하는 이미지
    public Sprite[] Cuts = new Sprite[3]; //바뀌어질 이미지
    private int index = 0;
    private bool done = false;

    private void Start()
    {
        StartCoroutine(Timer());
    }

    public void ChangeImage()
    {
        CurrentImage.sprite = Cuts[index];
    }

    IEnumerator Timer()
    {
        while (index < 3)
        {
            yield return new WaitForSeconds(3.0f);
            ChangeImage();
            index++;
        }

        yield return new WaitForSeconds(3.0f);

        int idx = SceneManager.GetActiveScene().buildIndex;
        string path = SceneUtility.GetScenePathByBuildIndex(idx + 1);
        string name = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
        SceneLoader.Instance.LoadScene(name);
    }
}

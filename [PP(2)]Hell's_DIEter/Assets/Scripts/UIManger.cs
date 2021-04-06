using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManger : MonoBehaviour
{
    #region Variables
    int stage = 0;

    #region Tutorial & Main Stage
    // 스크립트
    private Player playerScript;

    // 슬라이더(Gauge Bar)
    public Slider WeightSlider;
    public Slider HPSlider;
    public Slider FuelSlider;

    // 텍스트
    private TextMeshProUGUI weightText;
    private TextMeshProUGUI hpText;
    private TextMeshProUGUI fuelText;

    // 플레이어 이미지
    public Sprite[] Avatars;
    private Image avatar;

    // 게이지 값
    private float weight;
    private int hp;
    private float fuel;
    #endregion

    #region PanelPuzzle Stage
    private PanelPuzzleController puzzleController;
    private GameObject[] Lives = new GameObject[3];
    private GameObject[] FlipCounts = new GameObject[3];
    #endregion
    // 게임 결과
    public GameObject result;
    #endregion

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "1.Tutorial" ||
            SceneManager.GetActiveScene().name == "2.Main")
        {
            stage = 0;
            playerScript = GameObject.Find("Player").GetComponent<Player>();              // 플레이어 스크립트
            weightText = WeightSlider.GetComponentInChildren<TextMeshProUGUI>();                     // 체중을 나타낼 텍스트
            hpText = HPSlider.GetComponentInChildren<TextMeshProUGUI>();                                   // HP를 나타낼 텍스트
            fuelText = FuelSlider.GetComponentInChildren<TextMeshProUGUI>();                              // 연료량을 나타낼 텍스트
            avatar = HPSlider.transform.Find("IconImage").GetComponent<Image>();    // 플레이어 아바타 이미지

            // 게이지에 표기할 값 가져오기
            WeightSlider.maxValue = playerScript.Weight;
            HPSlider.maxValue = playerScript.Hp;
            FuelSlider.maxValue = playerScript.MaxFuel;
        }
        else if(SceneManager.GetActiveScene().name == "3.PanelPuzzle")
        {
            stage = 1;
            puzzleController = GameObject.Find("PanelPuzzle").GetComponent<PanelPuzzleController>();

            FlipCounts[0] = this.transform.Find("Flip Counts").Find("Pizza 1").gameObject;
            FlipCounts[1] = this.transform.Find("Flip Counts").Find("Pizza 2").gameObject;
            FlipCounts[2] = this.transform.Find("Flip Counts").Find("Pizza 3").gameObject;

            Lives[0] = this.transform.Find("Hearts").Find("Heart 1").gameObject;
            Lives[1] = this.transform.Find("Hearts").Find("Heart 2").gameObject;
            Lives[2] = this.transform.Find("Hearts").Find("Heart 3").gameObject;

            int count = puzzleController.Chance;
            switch (count)
            {
                case 1:
                    FlipCounts[1].SetActive(false);
                    FlipCounts[2].SetActive(false);
                    break;
                case 2:
                    FlipCounts[2].SetActive(false);
                    break;
            }
        }
        // 게임오버 시 게임오버화면 표시
        result.SetActive(false);
    }

    void Update()
    {
        // 게임오버
        if (result.gameObject.activeSelf == true)
            return;

        if (stage == 0)
        {
            UpdateGauge();
            UpdateAvatar();
            if (result.gameObject.activeSelf == false && playerScript.Hp <= 0)
                result.SetActive(true);
        }
        else if (stage == 1)
        {
            switch (puzzleController.Lives)
            {
                case 2:
                    Lives[2].gameObject.SetActive(false);
                    break;
                case 1:
                    Lives[1].gameObject.SetActive(false);
                    break;
                case 0:
                    Lives[0].gameObject.SetActive(false);
                    break;
                default:
                    break;
            }

            if (result.gameObject.activeSelf == false && puzzleController.Lives <= 0)
            {
                result.SetActive(true);
            }

            if(puzzleController.Clear==true)
            {
                transform.Find("Rocket").gameObject.SetActive(true);
            }
        }
        
    }

    // 게이지를 업데이트 하는 함수
    private void UpdateGauge()
    {
        // 체중 게이지
        weight = playerScript.Weight;
        WeightSlider.value = weight;
        weightText.text = weight.ToString() + "/" + playerScript.MaxWeight.ToString() + "Kg";
        if (WeightSlider.value <= 0)
            WeightSlider.transform.Find("Fill Area").gameObject.SetActive(false);
        else
            WeightSlider.transform.Find("Fill Area").gameObject.SetActive(true);

        // HP 게이지
        hp = (int)playerScript.Hp;
        HPSlider.value = hp;
        hpText.text = hp.ToString();
        if (HPSlider.value <= 0)
            HPSlider.transform.Find("Fill Area").gameObject.SetActive(false);
        else
            HPSlider.transform.Find("Fill Area").gameObject.SetActive(true);

        // 연료 게이지
        fuel = playerScript.Fuel;
        FuelSlider.value = fuel;
        fuel = (int)fuel;
        fuelText.text = fuel.ToString() + "/" + playerScript.MaxFuel.ToString();
        if (FuelSlider.value <= 0)
            FuelSlider.transform.Find("Fill Area").gameObject.SetActive(false);
        else
            FuelSlider.transform.Find("Fill Area").gameObject.SetActive(true);
    }

    private void UpdateAvatar()
    {
        if (hp >= 70)
        {
            avatar.sprite = Avatars[0];
        }
        else if (hp < 30)
        {
            avatar.sprite = Avatars[2];
        }
        else
        {
            avatar.sprite = Avatars[1]; 
        }
    }
}

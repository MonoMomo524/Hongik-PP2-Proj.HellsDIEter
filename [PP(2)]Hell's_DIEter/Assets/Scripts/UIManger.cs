using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManger : MonoBehaviour
{
    #region Variables
    // 플레이어 스크립트
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

    // 게임 결과
    public GameObject result;
    #endregion

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();              // 플레이어 스크립트
        weightText = WeightSlider.GetComponentInChildren<TextMeshProUGUI>();                     // 체중을 나타낼 텍스트
        hpText = HPSlider.GetComponentInChildren<TextMeshProUGUI>();                                   // HP를 나타낼 텍스트
        fuelText = FuelSlider.GetComponentInChildren<TextMeshProUGUI>();                              // 연료량을 나타낼 텍스트
        avatar = HPSlider.transform.Find("IconImage").GetComponent<Image>();    // 플레이어 아바타 이미지

        // 게이지에 표기할 값 가져오기
        WeightSlider.maxValue = playerScript.GetWeight();
        HPSlider.maxValue = playerScript.GetHP();
        FuelSlider.maxValue = playerScript.GetMaxFuel();

        // 게임오버 시 게임오버화면 표시
        result.SetActive(false);
    }

    void Update()
    {
        // 게임오버
        if (result.gameObject.activeSelf == true)
            return;

        UpdateGauge();
        UpdateAvatar();
        if (result.gameObject.activeSelf==false && playerScript.GetHP() <= 0)
        {
            result.SetActive(true);
        }
    }

    // 게이지를 업데이트 하는 함수
    private void UpdateGauge()
    {
        // 체중 게이지
        weight = playerScript.GetWeight();
        WeightSlider.value = weight;
        weightText.text = weight.ToString() + "/" + playerScript.GetMaxWeight().ToString() + "Kg";
        if (WeightSlider.value <= 0)
            WeightSlider.transform.Find("Fill Area").gameObject.SetActive(false);
        else
            WeightSlider.transform.Find("Fill Area").gameObject.SetActive(true);

        // HP 게이지
        hp = playerScript.GetHP();
        HPSlider.value = hp;
        hpText.text = hp.ToString();
        if (HPSlider.value <= 0)
            HPSlider.transform.Find("Fill Area").gameObject.SetActive(false);
        else
            HPSlider.transform.Find("Fill Area").gameObject.SetActive(true);

        // 연료 게이지
        fuel = playerScript.GetFuel();
        FuelSlider.value = fuel;
        fuel = (int)fuel;
        fuelText.text = fuel.ToString() + "/" + playerScript.GetMaxFuel().ToString();
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

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
    GameObject inventory;
    bool isUsing;

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
    private GameObject[] Lives;
    private GameObject[] FlipCounts;
    #endregion

    #region WeightPuzzle Stage
    private WeightPuzzleController weightController;
    private TextMeshProUGUI timerText;
    private GameObject[] Hands;
    #endregion

    // 게임 결과
    public GameObject result;
    #endregion

    void Start()
    {
        ChanageSettings(SceneManager.GetActiveScene().name);

        if (SceneManager.GetActiveScene().buildIndex == 3
            || SceneManager.GetActiveScene().buildIndex == 5)
        {
            isUsing = true;
            if (playerScript == null)
                playerScript = GameObject.Find("Player").GetComponent<Player>();
            inventory = GameObject.Find("UI").transform.Find("Inventory").gameObject;
            SetInventory();
        }

        else
            isUsing = false;
    }

    void Update()
    {
        // 게임오버
        if (result.gameObject.activeSelf == true)
            return;

        if (Input.GetKeyDown(KeyCode.Escape) 
            && (SceneManager.GetActiveScene().buildIndex == 2
                    || SceneManager.GetActiveScene().buildIndex == 3
                    || SceneManager.GetActiveScene().buildIndex == 4
                    || SceneManager.GetActiveScene().buildIndex == 5))
            this.transform.Find("PauseButton").GetComponent<ButtonController>().MenuPopUp();

        if (stage == 1)
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

            int count = puzzleController.Chance;
            switch (count)
            {
                case 0:
                    FlipCounts[0].SetActive(false);
                    FlipCounts[1].SetActive(false);
                    FlipCounts[2].SetActive(false);
                    break;
                case 1:
                    FlipCounts[0].SetActive(true);
                    FlipCounts[1].SetActive(false);
                    FlipCounts[2].SetActive(false);
                    break;
                case 2:
                    FlipCounts[0].SetActive(true);
                    FlipCounts[1].SetActive(true);
                    FlipCounts[2].SetActive(false);
                    break;
                case 3:
                    FlipCounts[0].SetActive(true);
                    FlipCounts[1].SetActive(true);
                    FlipCounts[2].SetActive(true);
                    break;
            }

            if (puzzleController.Clear==true)
            {
                transform.Find("Rocket").gameObject.SetActive(true);
            }

            return;
        }
        else if (stage == 2)
        {
            if (result.gameObject.activeSelf == false && weightController.Timer <= 0)
            {
                result.SetActive(true);
                return;
            }

            UpdateGauge();
            UpdateAvatar();
            if (result.gameObject.activeSelf == false && playerScript.Hp <= 0)
            {
                result.SetActive(true);
                return;
            }

            timerText.text = "남은 시간: " + ((int)weightController.Timer).ToString();

            if(playerScript.IsGrabbing == true)
            {
                Hands[0].SetActive(false);
                Hands[1].SetActive(true);
            }
            else
            {
                Hands[0].SetActive(true);
                Hands[1].SetActive(false);
            }
        }
        else
        {
            UpdateGauge();
            UpdateAvatar();
            if (result.gameObject.activeSelf == false && playerScript.Hp <= 0)
                result.SetActive(true);
        }

        SetInventory();
    }

    public void ChanageSettings(string name)
    {
        if (name == "3.PanelPuzzle")
        {
            stage = 1;
            Lives = new GameObject[3];
            FlipCounts = new GameObject[3];

            puzzleController = GameObject.Find("PanelPuzzle").GetComponent<PanelPuzzleController>();

            // 초기 세팅
            FlipCounts[0] = this.transform.Find("Flip Counts").Find("Pizza 1").gameObject;
            FlipCounts[1] = this.transform.Find("Flip Counts").Find("Pizza 2").gameObject;
            FlipCounts[2] = this.transform.Find("Flip Counts").Find("Pizza 3").gameObject;

            Lives[0] = this.transform.Find("Hearts").Find("Heart 1").gameObject;
            Lives[1] = this.transform.Find("Hearts").Find("Heart 2").gameObject;
            Lives[2] = this.transform.Find("Hearts").Find("Heart 3").gameObject;

            return;
        }
        else if(name == "4.WeightScale")
        {
            if (playerScript == null)
                playerScript = GameObject.Find("Player").GetComponent<Player>();
            stage = 2;
            weightController = GameObject.Find("Weight Puzzle Stage").GetComponent<WeightPuzzleController>();
            timerText = transform.Find("InfoText").Find("Timer").GetComponent<TextMeshProUGUI>();
            Hands = new GameObject[2];
            Hands[0] = GameObject.Find("HandPoint");
            Hands[1] = GameObject.Find("HandGrab");
            Hands[0].SetActive(true);
            Hands[1].SetActive(false);
        }

        if (name != "3.PanelPuzzle")
        {
            if (playerScript == null) 
                playerScript = GameObject.Find("Player").GetComponent<Player>();              // 플레이어 스크립트

            if(weightText == null)
                weightText = WeightSlider.GetComponentInChildren<TextMeshProUGUI>();                     // 체중을 나타낼 텍스트

            if(hpText == null)
                hpText = HPSlider.GetComponentInChildren<TextMeshProUGUI>();                                   // HP를 나타낼 텍스트

            if(fuelText == null)
                fuelText = FuelSlider.GetComponentInChildren<TextMeshProUGUI>();                              // 연료량을 나타낼 텍스트

            if(avatar == null)
                avatar = HPSlider.transform.Find("IconImage").GetComponent<Image>();    // 플레이어 아바타 이미지

            // 게이지에 표기할 값 가져오기
            if(WeightSlider!=null)
                WeightSlider.maxValue = playerScript.Weight;

            if (HPSlider != null)
                HPSlider.maxValue = playerScript.Hp;

            if (FuelSlider != null)
                FuelSlider.maxValue = playerScript.MaxFuel;
        }

        // 게임오버 시 게임오버화면 표시
        result.SetActive(false);
    }

    // 게이지를 업데이트 하는 함수
    private void UpdateGauge()
    {
        // 체중 게이지
        weight = playerScript.Weight;
        WeightSlider.value = (int)weight;
        weightText.text = ((int)weight).ToString() + "/" + playerScript.MaxWeight.ToString() + "Kg";
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
        FuelSlider.maxValue = playerScript.MaxFuel;
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

    public void SetInventory()
    {
        if (isUsing == false)
            return;

        // 코인 개수
        TextMeshProUGUI tmPro = inventory.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        
        tmPro.text = "x" + playerScript.CoinCounts.ToString();

        // 덤벨 획득 시 인벤토리에서도 볼 수 있음
        if (playerScript.DumCounts > 0)
        {
            inventory.transform.GetChild(2).gameObject.SetActive(true);
            tmPro = inventory.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            tmPro.text = "x" + playerScript.DumCounts.ToString();
        }

    }

    public void ShowKeyIcon(bool hasKey)
    {
        // 열쇠 획득 시 인벤토리에서도 볼 수 있음
        if (hasKey== true)
        {
            inventory.transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    public void ShowMapIcon(bool hasMap)
    {
        // 지도 획득 시 인벤토리에서도 볼 수 있음
        if (hasMap == true)
        {
            inventory.transform.GetChild(4).gameObject.SetActive(true);
            GameObject.Find("Player").transform.GetChild(3).gameObject.SetActive(true);
        }
    }
}

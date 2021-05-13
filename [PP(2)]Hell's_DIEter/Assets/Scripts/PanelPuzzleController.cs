using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Panel
{
    public GameObject PanelObject;
    private int number;
    public int Number
    {
        get { return number; }
        set { number = value; }
    }

    private bool state;
    public bool State
    {
        get { return state; }
        set { state = value; }
    }
}

public class PanelPuzzleController : MonoBehaviour
{
    public GameObject Model;
    public static int level = 1;
    public Material[] materials;

    private List<int> inputs;
    private int width = 3;
    private int height = 3;
    private int flip = 1;
    private bool clear = false;
    public bool Clear
    {
        get { return clear; }
    }
    bool flipable = true;
    private int chance;
    public int Chance
    {
        get { return chance; }
    }
    private int lives = 3;
    public int Lives
    {
        get { return lives; }
    }
    private List<Panel> goalPanels;
    private List<Panel> myPanels;

    private bool selected = false;

    // Start is called before the first frame update
    void Start()
    {
        chance = flip;

        // 패널 딕셔너리 생성
        goalPanels = new List<Panel>();
        myPanels = new List<Panel>();
        inputs = new List<int>();
        
        // 패널 수 조정
        switch (level)
        {
            case 2:
                width = 4;
                flip = Random.Range(1, 2);
                break;
            case 3:
                height = 4;
                flip = Random.Range(2, 3);
                break;
            default:
                break;
        }

        // 패널 생성
       CreatePanels();
    }

    private void Update()
    {
        // 맞추지 못했다면 목숨-1
        if(chance == 0 && clear == false)
        {
            flipable = false;
            chance = flip;
            lives--;
            StartCoroutine(ResetPanels());
        }

        // 게임 오버
        if(lives==0)
        {
            Debug.Log("GAME OVER");
            GameObject.Find("Canvas").transform.Find("GameResult").gameObject.SetActive(true);
            StartCoroutine(EndGame());
        }
        // 게임 클리어
        else if (clear == true)
        {
            GameObject.Find("Rocket").SetActive(true);
            level++;
            StartCoroutine(EndGame());
        }

        // 게임 플레이 중
        // 마우스 조작
        if (clear == false && flipable == true && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.transform.CompareTag("Panel"))
            {
                Panel result = myPanels.Find(x => x.PanelObject.name == hit.transform.name);

                FlipPanel(myPanels, result.Number);
                selected = true;
            }
        }
        //키보드 조작
        if (clear == false && flipable == true)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                chance--;
                inputs.Add(6);
                FlipPanel(myPanels, 6);
                selected = true;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                chance--;
                inputs.Add(7);
                FlipPanel(myPanels, 7);
                selected = true;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                chance--;
                inputs.Add(8);
                FlipPanel(myPanels, 8);
                selected = true;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                chance--;
                inputs.Add(3);
                FlipPanel(myPanels, 3);
                selected = true;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                chance--;
                inputs.Add(4);
                FlipPanel(myPanels, 4);
                selected = true;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                chance--;
                inputs.Add(5);
                FlipPanel(myPanels, 5);
                selected = true;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                chance--;
                inputs.Add(0);
                FlipPanel(myPanels, 0);
                selected = true;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                chance--;
                inputs.Add(1);
                FlipPanel(myPanels, 1);
                selected = true;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                chance--;
                inputs.Add(2);
                FlipPanel(myPanels, 2);
                selected = true;
            }
        }
        // 맞는지 확인
        if (selected)
        {
            clear = ComparePanels();
            selected = false;
        }
    }

    // 패널을 생성하는 메소드
    private void CreatePanels()
    {
        // 목표 패널과 플레이어 패널 그룹을 찾음
        Transform GoalParent = GameObject.Find("GoalPanel").GetComponent<Transform>();
        Transform MyParent = GameObject.Find("MyPanel").GetComponent<Transform>();

        // 패널 생성을 위한 반복문
        for (int i = 0; i < width*height; i++)
        {
            // 목표 패널의 앞뒷면 설정
            Panel newPanel = new Panel();
            newPanel.PanelObject = Model;
            bool randBool = (Random.value > 0.5f);
            newPanel.State = randBool;

            // 목표 패널과 플레이어 패널의 자식 객체로 인스턴스 생성
            GameObject instance = (GameObject)Instantiate(newPanel.PanelObject, GoalParent);
            newPanel.PanelObject = instance;
            goalPanels.Add(newPanel);    // 목표 패널에 자식 객체로 추가

            //  패널의 위치와 회전 조정
            int yPos = (i / height) * -6;
            int xPos = (i % width) * -6;
            newPanel.PanelObject.transform.localPosition = new Vector3(xPos, yPos, 0.0f);   // 패널 위치 설정

            // 패널의 번호 설정과 색상 적용
            newPanel.Number = i;    // 번호 설정
            SetColor(newPanel);     // 색상 적용

            // 플레이어 패널의 앞뒷면 설정
            Panel mPanel = new Panel();
            mPanel.PanelObject = Model;
            mPanel.State = randBool;

            //  패널의 위치와 회전 조정
            mPanel.PanelObject.transform.localPosition = new Vector3(xPos, yPos, 0.0f);   // 패널 위치 설정
            mPanel.PanelObject.transform.name = "Panel(" + xPos.ToString() + "," + yPos.ToString() + ")";

            // 패널의 번호 설정과 색상 적용
            mPanel.Number = i;    // 번호 설정
            SetColor(mPanel);     // 색상 적용

            instance = (GameObject)Instantiate(mPanel.PanelObject, MyParent);
            mPanel.PanelObject = instance;
            myPanels.Add(mPanel);      // 플레이어 패널에 자식 개체로 추가
        }

        // 플레이어의 패널을 뒤집어 놓기
        SetPanels(myPanels);
    }

    private void SetColor(Panel panel)
    {
        // 패널 앞뒷면 색상 설정
        if (panel.State)
        {
            // WHITE  
            panel.PanelObject.transform.GetComponent<MeshRenderer>().sharedMaterial = materials[0];
            panel.PanelObject.transform.Rotate(new Vector3(0f, 0f, 0f));
        }
        else
        {
            // SKY BLUE
            panel.PanelObject.transform.GetComponent<MeshRenderer>().sharedMaterial = materials[1];
            panel.PanelObject.transform.Rotate(new Vector3(0f, 180f, 0f));
        }

        return;
    }

    // 퍼즐 시작 전 플레이어 패널 세팅 메소드
    public void SetPanels(List<Panel> myPanels)
    {
        int[] numbers = new int[flip];

        // 무작위로 뒤집을 패널 선택(중복불허)
        int i = 0;
        while(i < flip)
        {
            // 0 ~ 8
            int num = Random.Range(0, width * height);
            if (numbers.Contains<int>(num)==false)
            {
                numbers[i] = num;
                i++;
            }
        }

        // 뒤집기
        for (i = 0; i < flip; i++)
        {
            FlipPanel(myPanels, numbers[i]);
        }

        return;
    }

    // 뒤집기 메소드
    public void FlipPanel(List<Panel> panels, int selected)
    {
        // 퍼즐 세팅
        int i = 0, col = 0, row = 0;
        int[,] puzzle = new int[width, height];
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                puzzle[h, w] = i;
                if(i==selected)
                {
                    col = h;
                    row = w;
                }
                i++;
            }
        }

        for (int c = -1; c < 2; c++)
        {
            if (col + c < 0 || col + c > height-1)
                continue;

            for (int r = -1; r < 2; r++)
            {
                if (row + r < 0 || row + r > width-1)
                    continue;

                int index = puzzle[col + c, row + r];
                panels[index].State = !panels[index].State;
                SetColor(panels[index]);
            }
        }
        return;
    }

    public bool ComparePanels()
    {
        for(int i=0; i<width*height; i++)
        {
            if (myPanels[i].State != goalPanels[i].State)
                return false;
        }

        return true;
    }

    IEnumerator ResetPanels()
    {
        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < flip; i++)
        {
            FlipPanel(myPanels, inputs[i]);
        }
        inputs.Clear();
        flipable = true;
    }

    IEnumerator EndGame()
    {
        int i = 0;
        while(i<8)
        {
            yield return new WaitForSeconds(1.0f);
            i++;
        }

        Debug.Log("END");
        SceneManager.UnloadSceneAsync(5);
    }
}

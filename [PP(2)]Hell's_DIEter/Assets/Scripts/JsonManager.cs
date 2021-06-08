using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;

[Serializable]
public class MyJsonContainer
{
    // 사운드 데이터
    public int SoundState;

    // 룸 오픈 여부
    public int isOpen1;    //1-2
    public int isOpen2;    //2-4
    public int isOpen3;    //1-8
    public int isOpen4;    //2-8
    public int roomNum;

    // 플레이어 관련
    public int SavePoint;

    // 아이템 소지 여부
    public int Count;
    public int DumCounts;
    public int MinWeight;
    public int Weight;
    public int Fuel;
    public int MaxFuel;
    public int CoinCounts;
    public int HasMap;
    public int HasKey;
}

public class JsonManager : MonoBehaviour
{
    public MyJsonContainer jsonContainer;
    public string DataFileName = "HellsDIEter_save.json";
    // Singletone
    static GameObject container;
    static GameObject Container
    {
        get
        {
            return container;
        }
    }
    static JsonManager instance;
    public static JsonManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "JsonManager";
                instance = container.AddComponent(typeof(JsonManager)) as JsonManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    private void Start()
    {
        Load();
    }

    public void Save()
    {
        if (jsonContainer == null)
        {
            jsonContainer = new MyJsonContainer();
            GetData();
        }

        string json = JsonUtility.ToJson(jsonContainer, true);

        MyJsonContainer myJson = JsonUtility.FromJson<MyJsonContainer>(json);

        SaveDataText(jsonContainer, DataFileName);
    }

    public void Load()
    {
        jsonContainer = LoadDataText<MyJsonContainer>(DataFileName);
    }

    public void GetData()
    {
        if(PlayerPrefs.HasKey("Sound"))
            jsonContainer.SoundState = PlayerPrefs.GetInt("Sound");

        if (PlayerPrefs.HasKey("Count"))
        {
            switch (PlayerPrefs.GetInt("Count"))
            {
                case 0:
                    jsonContainer.isOpen1 = 0;
                    jsonContainer.isOpen2 = 0;
                    jsonContainer.isOpen3 = 0;
                    jsonContainer.isOpen4 = 0;
                    break;
                case 1:
                    jsonContainer.isOpen1 = 1;
                    jsonContainer.isOpen2 = 0;
                    jsonContainer.isOpen3 = 0;
                    jsonContainer.isOpen4 = 0;
                    break;
                case 2:
                    jsonContainer.isOpen1 = 1;
                    jsonContainer.isOpen2 = 1;
                    jsonContainer.isOpen3 = 0;
                    jsonContainer.isOpen4 = 0;
                    break;
                case 3:
                    jsonContainer.isOpen1 = 1;
                    jsonContainer.isOpen2 = 1;
                    jsonContainer.isOpen3 = 1;
                    jsonContainer.isOpen4 = 0;
                    break;
                case 4:
                    jsonContainer.isOpen1 = 1;
                    jsonContainer.isOpen2 = 1;
                    jsonContainer.isOpen3 = 1;
                    jsonContainer.isOpen4 = 1;
                    break;
                default:
                    jsonContainer.isOpen1 = 0;
                    jsonContainer.isOpen2 = 0;
                    jsonContainer.isOpen3 = 0;
                    jsonContainer.isOpen4 = 0;
                    break;
            }
            jsonContainer.isOpen1 = PlayerPrefs.GetInt("Puzzle1");
        }

        if (PlayerPrefs.HasKey("Min"))
            jsonContainer.MinWeight = PlayerPrefs.GetInt("Min");

        if (PlayerPrefs.HasKey("Weight"))
            jsonContainer.Weight = PlayerPrefs.GetInt("Weight");

        if (PlayerPrefs.HasKey("Map"))
            jsonContainer.HasMap = PlayerPrefs.GetInt("Map");

        if (PlayerPrefs.HasKey("Key"))
            jsonContainer.HasKey = PlayerPrefs.GetInt("Key");

        if (PlayerPrefs.HasKey("SavePoint"))
            jsonContainer.SavePoint = PlayerPrefs.GetInt("SavePoint");

        if (PlayerPrefs.HasKey("Coin"))
            jsonContainer.CoinCounts = PlayerPrefs.GetInt("Coin");

        if (PlayerPrefs.HasKey("Dumb"))
            jsonContainer.DumCounts = PlayerPrefs.GetInt("Dumb");

        if (PlayerPrefs.HasKey("Fuel"))
            jsonContainer.MaxFuel = PlayerPrefs.GetInt("Fuel");

        if (PlayerPrefs.HasKey("Count"))
            jsonContainer.Count = PlayerPrefs.GetInt("Count");
    }

    public void SaveDataText<T>(T data, string _fileName)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);

            if (json.Equals("{}"))
            {
                Debug.Log("json null");
                return;
            }
            string path = Application.persistentDataPath + "/" + _fileName;
            File.WriteAllText(path, json);

            Debug.Log(json);
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("The file was not found:" + e.Message);

            GetData();
            Save();
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("The directory was not found: " + e.Message);

            GetData();
            Save();
        }
        catch (IOException e)
        {
            Debug.Log("The file could not be opened:" + e.Message);

            GetData();
            Save();
        }
    }

    // 게임 종료 시 자동저장
    private void OnApplicationQuit()
    {
        Debug.Log("SAVE GAME");
        GetData();
        Save();
    }

    public T LoadDataText<T>(string _fileName)
    {
        try
        {
            string path = Application.persistentDataPath + "/" + _fileName;
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Debug.Log(json);
                T t = JsonUtility.FromJson<T>(json);
                return t;
            }
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("The file was not found:" + e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("The directory was not found: " + e.Message);
        }
        catch (IOException e)
        {
            Debug.Log("The file could not be opened:" + e.Message);
        }
        return default;
    }
}
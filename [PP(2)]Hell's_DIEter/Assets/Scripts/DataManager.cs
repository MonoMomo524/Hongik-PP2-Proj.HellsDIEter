using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    // 폴더 내 저장파일 확인 및 존재확인
using System;

public class DataManager : MonoBehaviour
{
    // Singletone
    static GameObject container;
    static GameObject Container
    {
        get
        {
            return container;
        }
    }

    static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if(!instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    // 데이터 파일 이름 설정
    public const string DataFileName = "HellsDIEter_save.json";

    public GameData gameData;
    public GameData GameData
    {
        get
        {
            if(gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return gameData;
        }
    }

    private void Start()
    {
        LoadGameData();
        SaveGameData();
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + DataFileName;

        // 저장된 게임이 있으면 불러오기
        if(File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        // 새 저장파일 생성
        else
        {
            gameData = new GameData();
        }
    }

    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + DataFileName;

        // 이미 있다면 덮어쓰기
        File.WriteAllText(filePath, ToJsonData);
    }

    // 게임 종료 시 자동저장
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}

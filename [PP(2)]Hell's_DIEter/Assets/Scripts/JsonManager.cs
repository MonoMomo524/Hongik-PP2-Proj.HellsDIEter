using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class GameData
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
    public int Dumbell;
    public int MinWeight;
    public int Weight;
    public int Fuel;
    public int Coin;
    public int HasMap;
    public int HasKey;
}

public class JsonManager : MonoBehaviour
{
    public GameData data = new GameData();

    private void Start()
    {
        GetData();
    }

    public void Save()
    {
        JsonData dataJson = JsonMapper.ToJson(data);

        File.WriteAllText(Application.dataPath + "/Resources/SaveData.json", dataJson.ToString());
    }

    public void Load()
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Resources/SaveData.json");

        JsonData jsonData = JsonMapper.ToObject(JsonString);
    }

    public void GetData()
    {
        data.SoundState = PlayerPrefs.GetInt("Sound");
        data.isOpen1 = PlayerPrefs.GetInt("Puzzle1");
        data.isOpen2 = PlayerPrefs.GetInt("Weight1");
        data.isOpen3 = PlayerPrefs.GetInt("Puzzle2");
        data.isOpen4 = PlayerPrefs.GetInt("Weight2");
        data.Dumbell = PlayerPrefs.GetInt("Dumb");
        data.MinWeight = PlayerPrefs.GetInt("Min");
        data.Weight = PlayerPrefs.GetInt("Weight");
        data.Fuel = PlayerPrefs.GetInt("Fuel");
        data.Coin = PlayerPrefs.GetInt("Coin");
        data.HasMap = PlayerPrefs.GetInt("Map");
        data.HasKey = PlayerPrefs.GetInt("Key");
        data.SavePoint = PlayerPrefs.GetInt("SavePoint");
    }

    private void SetData()
    {

    }
}
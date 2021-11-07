using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    // 폴더 내 저장파일 확인 및 존재확인
using LitJson;

public class DataManager : MonoBehaviour
{
    bool agree = false;
    int sound;

    public void DeleteData()
    {
        sound = PlayerPrefs.GetInt("Sound");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Sount", sound);
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class GameData
{
    // 사운드 데이터
    private bool bgmSound;
    public bool BGMSound
    {
        get { return BGMSound; }
        set { bgmSound = value; }
    }

    // 룸 오픈 여부
    public bool isOpen1;
    public bool isOpen2;
    public bool isOpen3;
    public bool isOpen4;
    public bool isOpen5;
}
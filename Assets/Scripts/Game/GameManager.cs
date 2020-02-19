﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //做成单例
    public static GameManager Instance;

    //游戏是否开始
    public bool IsGameStarted { get; set; }

    //游戏是否结束
    public bool IsGameOver { get; set; }

    private void Awake()
    {
        Instance = this;
    }
}

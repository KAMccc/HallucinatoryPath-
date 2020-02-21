using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    //做成单例
    public static GameManager Instance;

    //游戏是否开始
    public bool IsGameStarted { get; set; }

    //游戏是否结束
    public bool IsGameOver { get; set; }
    public bool IsPause { get; set; }
    //玩家是否开始移动
    public bool PlayerIsMove { get; set; }

    //当局游戏成绩
    private int gameScore;
    //吃到钻石
    private int gameDiamond;

    private GameData data;

    private bool isFirstGame;
    private bool isMusicOn;

    private int[] bestScoreArr;
    private int selectSkin;
    private bool[] skinUnlocked;
    private int diamondCount;

    private void Awake()
    {
        Instance = this;
        data = new GameData();

        EventCenter.AddListener(EventDefine.AddScore, AddGameScore);
        EventCenter.AddListener(EventDefine.PlayerMove,PlayerMove);
        EventCenter.AddListener(EventDefine.AddDiamond,AddGameDiaond);


        if (GameData.IsAgainGame)
        {
            IsGameStarted = true;
        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddScore, AddGameScore);
        EventCenter.RemoveListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.RemoveListener(EventDefine.AddDiamond, AddGameDiaond);

    }

    /// <summary>
    /// 玩家移动调用到
    /// </summary>
    private void PlayerMove()
    {
        PlayerIsMove = true;
    }

    /// <summary>
    /// 增加游戏成绩
    /// </summary>
    private void AddGameScore()
    {
        if (IsGameStarted == false || IsGameOver || IsPause)
            return;

        gameScore++;

        EventCenter.Broadcast(EventDefine.UpdateScoreText,gameScore);
    }

    /// <summary>
    /// 获取当局游戏成绩
    /// </summary>
    /// <returns></returns>
    public int GetGameScore()
    {
        return gameScore;
    }

    /// <summary>
    /// 获得吃到的钻石数
    /// </summary>
    /// <returns></returns>
    public int GetGameDiamond()
    {
        return gameDiamond;
    }

    /// <summary>
    /// 
    /// </summary>
    private void AddGameDiaond()
    {
        gameDiamond++;
        EventCenter.Broadcast(EventDefine.UpdateDiamondText,gameDiamond);
    }

    /// <summary>
    /// 储存数据
    /// </summary>
    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data"))
            {
                data.SetBestScoreArr(bestScoreArr);
                data.SetDiamondCount(diamondCount);
                data.SetIsFirstGame(isFirstGame);
                data.SetisMusicOn(isMusicOn);
                data.SetSelectSkin(selectSkin);
                data.SetSkinUnlocked(skinUnlocked);
                bf.Serialize(fs, data);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    /// <summary>
    /// 读取数据
    /// </summary>
    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data",FileMode.Open))
            {
                data = (GameData)bf.Deserialize(fs);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

}

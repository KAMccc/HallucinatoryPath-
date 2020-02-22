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

    private ManagerVars vars;

    private void Awake()
    {
        Instance = this;
        //不需要New了，下面调用了初始化的方法
        //data = new GameData();
        vars = ManagerVars.GetManagerVars();

        EventCenter.AddListener(EventDefine.AddScore, AddGameScore);
        EventCenter.AddListener(EventDefine.PlayerMove,PlayerMove);
        EventCenter.AddListener(EventDefine.AddDiamond,AddGameDiaond);


        if (GameData.IsAgainGame)
        {
            IsGameStarted = true;
        }

        InitGameData();
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
    /// 获取当前皮肤是否解锁
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetSkinUnlocked(int index)
    {
        return skinUnlocked[index];
    }

    /// <summary>
    /// 设置解锁皮肤
    /// </summary>
    /// <param name="index"></param>
    public void SetSkinUnlocked(int index)
    {
        skinUnlocked[index] = true;
        Save();
    }

    /// <summary>
    /// 设置当前选择皮肤
    /// </summary>
    /// <param name="index"></param>
    public void SetSelectedSkin(int index) 
    {
        selectSkin = index;
        Save();
    }

    /// <summary>
    /// 获得当前选择的皮肤
    /// </summary>
    /// <returns></returns>
    public int GetCurrentSelectSkin()
    {
        return selectSkin;
    }

    /// <summary>
    /// 获取所有钻石数量
    /// </summary>
    /// <returns></returns>
    public int GetAllDiamond()
    {
        return diamondCount;
    }

    /// <summary>
    /// 更新总钻石数量
    /// </summary>
    /// <param name="value">新增用+，减少用-</param>
    public void UpdateAllDiamond(int value)
    {
        diamondCount += value;
        Save();
    }

    /// <summary>
    /// 初始化游戏数据
    /// </summary>
    private void InitGameData()
    {
        Read();
        if(data != null)
        {
            isFirstGame = data.GetIsFirstGame();

        }
        else
        {
            isFirstGame = true;
        }

        //如果第一次开始游戏
        if (isFirstGame)
        {
            isFirstGame = false;
            isMusicOn = true;
            bestScoreArr = new int[3];
            selectSkin = 0;
            skinUnlocked = new bool[vars.skinSpriteList.Count];
            skinUnlocked[0] = true;//第一个皮肤默认解锁
            diamondCount = 10;

            data = new GameData();
            Save();
        }
        else
        {
            isMusicOn = data.GetisMusicOn();
            bestScoreArr = data.GetBestScoreArr();
            selectSkin = data.GetSelectSkin();
            skinUnlocked = data.GetSkinUnlocked();
            diamondCount = data.GetDiamondCount();
        }
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
                data =(GameData)bf.Deserialize(fs);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

}

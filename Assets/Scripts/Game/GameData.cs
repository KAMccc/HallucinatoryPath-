using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    //是否再来一局游戏
    public static bool IsAgainGame = false;

    private bool isFirstGame;
    private bool isMusicOn;

    private int[] bestScoreArr;
    private int selectSkin;
    private bool[] skinUnlocked;
    private int diamondCount;

    public void SetIsFirstGame(bool isFirstGame)
    {
        this.isFirstGame = isFirstGame;
    }

    public void SetisMusicOn(bool isMusicOn)
    {
        this.isMusicOn = isMusicOn;
    }

    public void SetBestScoreArr(int[] bestScoreArr)
    {
        this.bestScoreArr = bestScoreArr;
    }

    public void SetSelectSkin(int selectSkin)
    {
        this.selectSkin = selectSkin;
    }
    
    public void SetSkinUnlocked(bool[] skinUnlocked)
    {
        this.skinUnlocked = skinUnlocked;
    }

    public void SetDiamondCount(int diamondCount)
    {
        this.diamondCount = diamondCount;
    }

    public bool GetIsFirstGame()
    {
        return isFirstGame;
    }
    public bool GetisMusicOn()
    {
        return isMusicOn;
    }

    public int[] GetBestScoreArr()
    {
        return bestScoreArr;
    }

    public int GetSelectSkin()
    {
        return  selectSkin;
    }

    public bool[] GetSkinUnlocked()
    {
        return skinUnlocked;
    }

    public int GetDiamondCount()
    {
        return diamondCount;
    }

}

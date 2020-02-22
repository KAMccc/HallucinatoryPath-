using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "CreatManagerVarsContainer")]
public class ManagerVars : ScriptableObject
{
    public static ManagerVars GetManagerVars()
    {
        return Resources.Load<ManagerVars>("ManagerVarsContainter");
    }
    public List<Sprite> bgThemeSpriteList = new List<Sprite>();
    public List<Sprite> platformThemeSpriteLis = new List<Sprite>();
    [Header("商城皮肤图片-正面")]
    public List<Sprite> skinSpriteList = new List<Sprite>();
    [Header("游戏中皮肤图片-背面")]
    public List<Sprite> characterSkinSpriteList = new List<Sprite>();

    public GameObject characterPre;
    public GameObject skinChooseItemPre;
    [Header("皮肤名字")]
    public List<string> skinNameList = new List<string>();
    [Header("皮肤价格")]
    public List<int> skinPrice = new List<int>();

    public GameObject normalPlatformPre;
    public List<GameObject> commonPlatformGroup = new List<GameObject>();
    public List<GameObject> grassPlatformGroup = new List<GameObject>();
    public List<GameObject> winterPlatformGroup = new List<GameObject>();
    public GameObject spikePlatformLeft;
    public GameObject spikePlatformRight;
    public GameObject deathEffect;
    public GameObject diamondPre;

    public float nextXPos = 0.554f, nextYPos = 0.645f;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    private Button btn_Start;
    private Button btn_Shop;
    private Button btn_Rank;
    private Button btn_Sound;
    private Button btn_Reset;

    private ManagerVars vars;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        Init();
        EventCenter.AddListener(EventDefine.ShowMainPanel, ShowMainPanel);
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
    }

    private void Start()
    {
        //在Awake的时候广播，注册的还没有加载好，时序问题。需要在start的时候广播
        if (GameData.IsAgainGame)
        {
            EventCenter.Broadcast(EventDefine.ShopGamePanel);
            gameObject.SetActive(false);
        }
        //主界面设置当前皮肤图片
        ChangeSkin(GameManager.Instance.GetCurrentSelectSkin());
        Sound();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowMainPanel, ShowMainPanel);
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);

    }

    private void Init()
    {
        btn_Start = transform.Find("btn_Start").GetComponent<Button>();
        btn_Start.onClick.AddListener(OnStartButtonClick);

        btn_Shop = transform.Find("Btns/btn_Shop").GetComponent<Button>();
        btn_Shop.onClick.AddListener(OnShopButtonClick);

        btn_Rank = transform.Find("Btns/btn_Rank").GetComponent<Button>();
        btn_Rank.onClick.AddListener(OnRankButtonClick);

        btn_Sound = transform.Find("Btns/btn_Sound").GetComponent<Button>();
        btn_Sound.onClick.AddListener(OnSoundButtonClick);

        btn_Reset = transform.Find("Btns/btn_Reset").GetComponent<Button>();
        btn_Reset.onClick.AddListener(OnResetButtonClick);
    }

    /// <summary>
    /// 开始按钮点击后调用此方法
    /// </summary>
    private void OnStartButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        GameManager.Instance.IsGameStarted = true;
        EventCenter.Broadcast(EventDefine.ShopGamePanel);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 商店按钮单击
    /// </summary>
    private void OnShopButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);

        EventCenter.Broadcast(EventDefine.ShowShopPanel);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 排行榜按钮点击
    /// </summary>
    private void OnRankButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);

        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }

    /// <summary>
    /// 声音按钮点击
    /// </summary>
    private void OnSoundButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        GameManager.Instance.SetIsMusicOn(!GameManager.Instance.GetIsMusicOn());
        Sound();

    }

    /// <summary>
    /// 重置点击
    /// </summary>
    private void OnResetButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);

        EventCenter.Broadcast(EventDefine.ShowResetPanel);
    }

    private void ShowMainPanel()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 刷新UI皮肤图片
    /// </summary>
    /// <param name="skinIndex"></param>
    private void ChangeSkin(int skinIndex)
    {
        btn_Shop.transform.GetChild(0).GetComponent<Image>().sprite = vars.skinSpriteList[skinIndex];

    }

    private void Sound()
    {
        if (GameManager.Instance.GetIsMusicOn())
        {
            btn_Sound.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOn;
        }
        else
        {
            btn_Sound.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOff;
        }

        EventCenter.Broadcast(EventDefine.IsMusicOn, GameManager.Instance.GetIsMusicOn());
    }
}

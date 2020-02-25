using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    private Text txt_Score;
    private Text txt_BestScore;
    private Text txt_AddDiamondCount;

    private Button btn_Restart;
    private Button btn_Home;
    private Button btn_Rank;

    private Image img_New;

    private void Awake()
    {
        InitTextAndBtns();
        gameObject.SetActive(false);
        EventCenter.AddListener(EventDefine.ShopGameOverPanel,Show);

    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShopGameOverPanel,Show);

    }

    private void Show()
    {
        //成绩显示
        txt_Score.text = GameManager.Instance.GetGameScore().ToString();
        //钻石显示
        txt_AddDiamondCount.text ="+"+ GameManager.Instance.GetGameDiamond().ToString();
        //更新总的钻石数量
        GameManager.Instance.UpdateAllDiamond(GameManager.Instance.GetGameDiamond());
        //最高分显示 当局成绩大于最高分
        if (GameManager.Instance.GetGameScore() > GameManager.Instance.GetBestScore())
        {
            
            img_New.gameObject.SetActive(true);
            txt_BestScore.text = "最高分  " + GameManager.Instance.GetGameScore();
        }
        else
        {
            img_New.gameObject.SetActive(false);
            txt_BestScore.text = "最高分  " + GameManager.Instance.GetBestScore();
        }
        GameManager.Instance.SaveScore(GameManager.Instance.GetGameScore());

        gameObject.SetActive(true);
    }

    /// <summary>
    /// 文本和按钮是绑定
    /// </summary>
    private void InitTextAndBtns()
    {
        img_New = transform.Find("img_New").GetComponent<Image>();

        txt_Score = transform.Find("txt_Socre").GetComponent<Text>();
        txt_BestScore = transform.Find("txt_BestScore").GetComponent<Text>();
        txt_AddDiamondCount = transform.Find("Diamond/txt_AddDiamondCount").GetComponent<Text>();

        btn_Restart = transform.Find("btn_Restart").GetComponent<Button>();
        btn_Home = transform.Find("btn_Home").GetComponent<Button>();
        btn_Rank = transform.Find("btn_Rank").GetComponent<Button>();

        btn_Restart.onClick.AddListener(OnRestartButtonClick);
        btn_Home.onClick.AddListener(OnHomeButtonClick);
        btn_Rank.onClick.AddListener(OnRankButtonClick);
    }

    /// <summary>
    /// 再试一次按钮点击
    /// </summary>
    private void OnRestartButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = true;
    }

    /// <summary>
    /// 主菜单按钮点击
    /// </summary>
    private void OnHomeButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);

        //回到主菜单，即重新加载游戏场景即可
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = false;
    }

    /// <summary>
    /// 排行榜按钮点击
    /// </summary>
    private void OnRankButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);

        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GamePanel : MonoBehaviour
{
    private Button btn_Pause;
    private Button btn_Play;
    private Text txt_Score;
    private Text txt_DiamondCount;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShopGamePanel, Shop);

        Init();


    }

    private void Init()
    {
        btn_Pause = transform.Find("btn_Pause").GetComponent<Button>();
        btn_Pause.onClick.AddListener(OnPasueButtonClick);
        btn_Play = transform.Find("btn_Play").GetComponent<Button>();
        btn_Play.onClick.AddListener(OnPlayButtonClick);

        txt_Score = transform.Find("txt_Score").GetComponent<Text>();
        txt_DiamondCount = transform.Find("Diamond/txt_DiamondCount").GetComponent<Text>();

        btn_Play.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    private void Shop()
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShopGamePanel, Shop);
    }


    /// <summary>
    /// 暂停按钮单击
    /// </summary>
    private void OnPasueButtonClick()
    {
        btn_Play.gameObject.SetActive(true);
        btn_Pause.gameObject.SetActive(false);

        // Todo 游戏暂停
    }    
    
    /// <summary>
    /// 开始按钮单击
    /// </summary>
    private void OnPlayButtonClick()
    {
        btn_Play.gameObject.SetActive(false);
        btn_Pause.gameObject.SetActive(true);

        //Todo 继续游戏
    }

    
}

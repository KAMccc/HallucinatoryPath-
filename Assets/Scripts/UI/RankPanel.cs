using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RankPanel : MonoBehaviour
{
    private Button btn_Close;
    private GameObject go_SocoreList;
    private Text[] txt_Scores = new Text[3];

    private void Awake()
    {
        InitBtnAndText();
        btn_Close.GetComponent<Image>().color = new Color(btn_Close.GetComponent<Image>().color.r, btn_Close.GetComponent<Image>().color.g,
            btn_Close.GetComponent<Image>().color.b, 0);
        go_SocoreList.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
        EventCenter.AddListener(EventDefine.ShowRankPanel, Show);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRankPanel, Show);
    }

    private void InitBtnAndText()
    {
        btn_Close = transform.Find("btn_Close").GetComponent<Button>();
        btn_Close.onClick.AddListener(OnCloseButtonClick);
        go_SocoreList = transform.Find("ScoreList").gameObject;

        for (int i = 0; i < txt_Scores.Length; i++)
        {
            txt_Scores[i] = transform.Find("ScoreList").GetChild(i).GetComponentInChildren<Text>();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        btn_Close.GetComponent<Image>().DOColor(new Color(btn_Close.GetComponent<Image>().color.r, btn_Close.GetComponent<Image>().color.g,
            btn_Close.GetComponent<Image>().color.b, 0.8f),0.3f);
        go_SocoreList.transform.DOScale(Vector3.one,0.3f);

         int[] arr =GameManager.Instance.GetScoreArr();
        for (int i = 0; i < arr.Length; i++)
        {
            txt_Scores[i].text = arr[i].ToString();
        }
    }

    private void OnCloseButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);

        btn_Close.GetComponent<Image>().DOColor(new Color(btn_Close.GetComponent<Image>().color.r, btn_Close.GetComponent<Image>().color.g,
            btn_Close.GetComponent<Image>().color.b, 0), 0.3f);
        go_SocoreList.transform.DOScale(Vector3.zero, 0.3f).OnComplete(()=> { 
            gameObject.SetActive(false);
        });
    }
}

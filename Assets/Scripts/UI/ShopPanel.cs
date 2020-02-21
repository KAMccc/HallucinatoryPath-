using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopPanel : MonoBehaviour
{
    private ManagerVars vars;
    //子皮肤生成节点
    private Transform parent;
    private Text txt_Name;

    private Button btn_Back;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        parent = transform.Find("ScrollRect/Parent");
        txt_Name = transform.Find("txt_Name").GetComponent<Text>();
        btn_Back = transform.Find("btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(OnBackButtonClick);
        ScrollRectInit();

        EventCenter.AddListener(EventDefine.ShowShopPanel, ShowShopPanel);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        int selectIndex = (int)Mathf.Round(parent.transform.localPosition.x / -160f);
        //Debug.Log(currentIndex);
        if (Input.GetMouseButtonUp(0))
        {
            parent.transform.DOLocalMoveX(selectIndex * -160,0.2f);
            //parent.transform.localPosition = new Vector3(currentIndex * -160, 0, 0);
        }
        SetItemSize(selectIndex);
        RefreshUI(selectIndex);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowShopPanel, ShowShopPanel);
    }

    private void ScrollRectInit()
    {
        // 设置皮肤可滑动区域的宽度=》(皮肤数+2)* 160
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2((vars.skinSpriteList.Count + 2) * 160, 300);

        for (int i = 0; i < vars.skinSpriteList.Count; i++)
        {
            GameObject go = Instantiate(vars.skinChooseItemPre, parent);
            go.GetComponentInChildren<Image>().sprite = vars.skinSpriteList[i];
            go.transform.localPosition = new Vector3((i + 1) * 160, 0, 0);
        }
    }

    private void SetItemSize(int selectIndex)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if(selectIndex == i)
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
            }
            else
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);

            }
        }
    }

    private void RefreshUI(int selectIndex)
    {
        txt_Name.text = vars.skinNameList[selectIndex];
    }

    /// <summary>
    /// 商店界面放回按钮点击
    /// </summary>
    private void OnBackButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }

    private void ShowShopPanel()
    {
        gameObject.SetActive(true);
    }
}

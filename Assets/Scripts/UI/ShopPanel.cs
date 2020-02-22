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
    private Text txt_Diamond;
    private Button btn_Back;
    private Button btn_Select;
    private Button btn_Buy;

    private int selectIndex;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        parent = transform.Find("ScrollRect/Parent");
        txt_Name = transform.Find("txt_Name").GetComponent<Text>();
        txt_Diamond = transform.Find("Diamond/txt_Diamond").GetComponent<Text>();

        btn_Back = transform.Find("btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(OnBackButtonClick);

        btn_Select = transform.Find("btn_Select").GetComponent<Button>();
        btn_Select.onClick.AddListener(OnSelectButtonClick);

        btn_Buy = transform.Find("btn_Buy").GetComponent<Button>();
        btn_Buy.onClick.AddListener(OnBuyButtonClick);

        EventCenter.AddListener(EventDefine.ShowShopPanel, ShowShopPanel);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        ScrollRectInit();

    }

    private void Update()
    {
        selectIndex = (int)Mathf.Round(parent.transform.localPosition.x / -160f);
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
            //判断皮肤是否已经解锁，如果没有解锁就设置成灰色
            if (!GameManager.Instance.GetSkinUnlocked(i))
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
            else //解锁了
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }

            go.GetComponentInChildren<Image>().sprite = vars.skinSpriteList[i];
            go.transform.localPosition = new Vector3((i + 1) * 160, 0, 0);
        }
        //打开页面直接定位到当前选中的皮肤
        parent.transform.localPosition = new Vector3(GameManager.Instance.GetCurrentSelectSkin() * -160, 0);
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
        txt_Diamond.text = GameManager.Instance.GetAllDiamond().ToString();
        //如果皮肤未解锁
        if (!GameManager.Instance.GetSkinUnlocked(selectIndex))
        {
            btn_Select.gameObject.SetActive(false);
            btn_Buy.gameObject.SetActive(true);
            btn_Buy.GetComponentInChildren<Text>().text = vars.skinPrice[selectIndex].ToString();
        }
        else
        {
            btn_Select.gameObject.SetActive(true);
            btn_Buy.gameObject.SetActive(false);

        }
    }

    /// <summary>
    /// 商店界面放回按钮点击
    /// </summary>
    private void OnBackButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 购买按钮点击
    /// </summary>
    private void OnBuyButtonClick()
    {
        int price =int.Parse(btn_Buy.GetComponentInChildren<Text>().text);
        if(price > GameManager.Instance.GetAllDiamond())
        {
            EventCenter.Broadcast(EventDefine.Hint,"你 无 钱");
            Debug.LogErrorFormat("钻石不足，无法购买皮肤；当前钻石{0}，需要钻石{1}",GameManager.Instance.GetAllDiamond(),price);
            return;
        }
        GameManager.Instance.UpdateAllDiamond(-price);
        GameManager.Instance.SetSkinUnlocked(selectIndex);
        parent.GetChild(selectIndex).GetChild(0).GetComponent<Image>().color = Color.white;
    }

    //选择按钮点击
    private void OnSelectButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ChangeSkin,selectIndex);
        GameManager.Instance.SetSelectedSkin(selectIndex);
        //设置皮肤后隐藏按钮
        gameObject.SetActive(false);
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
    }

    private void ShowShopPanel()
    {
        gameObject.SetActive(true);
    }

}

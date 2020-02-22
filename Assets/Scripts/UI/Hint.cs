using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hint : MonoBehaviour
{
    private Image img_Bg;
    private Text txt_Hit;

    private void Awake()
    {
        img_Bg = GetComponent<Image>();
        txt_Hit = GetComponentInChildren<Text>();

        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        txt_Hit.color = new Color(txt_Hit.color.r, txt_Hit.color.g, txt_Hit.color.b,0);

        EventCenter.AddListener<string>(EventDefine.Hint, Show);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventDefine.Hint, Show);
    }

    private void Show(string text)
    {
        StopCoroutine("Delay");
        transform.localPosition = new Vector3(0, -70,0);
        transform.DOLocalMoveY(0, 0.3f).OnComplete(() =>
        {
            StartCoroutine("Delay");
        });
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.4f),0.1f);
        txt_Hit.DOColor(new Color(txt_Hit.color.r, txt_Hit.color.g, txt_Hit.color.b, 0.8f), 0.1f);
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        transform.DOLocalMoveY(70, 0.3f);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0), 0.1f);
        txt_Hit.DOColor(new Color(txt_Hit.color.r, txt_Hit.color.g, txt_Hit.color.b, 0), 0.1f);
    }
}

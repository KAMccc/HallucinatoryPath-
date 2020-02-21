using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderers;
    public GameObject obstacle;

    private bool startTimer = false;
    private float fallTime;
    private Rigidbody2D my_Body;

    private void Awake()
    {
        my_Body = GetComponent<Rigidbody2D>();
    }

    public void Init(Sprite sprite,float fallTime,int obstacleDirisLeft)
    {
        my_Body.bodyType = RigidbodyType2D.Static;

        startTimer = true;
        this.fallTime = fallTime;

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = sprite;
        }

        if(obstacleDirisLeft == 0)//朝右
        {
            if (obstacle != null)
            {
                obstacle.transform.localPosition = new Vector3(-obstacle.transform.localPosition.x,
                    obstacle.transform.localPosition.y, 0);
            }
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted || !GameManager.Instance.PlayerIsMove) return;
        if (startTimer)
        {
            fallTime -= Time.deltaTime;
            if(fallTime <0)//倒计时结束
            {
                //掉落
                startTimer = false;
                if(my_Body.bodyType != RigidbodyType2D.Dynamic)
                {
                    my_Body.bodyType = RigidbodyType2D.Dynamic;
                    StartCoroutine(DealyHide());
                }
            }
        }

        //平台在摄像机下面，也隐藏
        if(transform.position.y - Camera.main.transform.position.y < -6f)
        {
            StartCoroutine(DealyHide());
        }
    }

    private IEnumerator DealyHide()
    {
        yield return new WaitForSeconds(1f);
        my_Body.bodyType = RigidbodyType2D.Static;
        gameObject.SetActive(false);
    }
}

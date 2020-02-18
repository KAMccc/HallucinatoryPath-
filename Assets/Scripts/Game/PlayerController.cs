using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    //是否向左移动，反之向右
    private bool isMoveLeft = false;
    //是否正在跳跃
    private bool isJumping = false;
    private Vector3 nextPlatformLeft;
    private Vector3 nextPlatformRight;

    private ManagerVars vars;


    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isJumping == false)
        {
            EventCenter.Broadcast(EventDefine.DecidePath);

            isJumping = true;

            Vector3 mousePos = Input.mousePosition;

            //单击左边屏幕
            if(mousePos.x <= Screen.width / 2)
            {
                isMoveLeft = true;
                transform.DOMoveX(nextPlatformLeft.x, 0.2f);
                transform.DOMoveY(nextPlatformLeft.y+0.8f, 0.15f);
            }
            else if(mousePos.x > Screen.width / 2)//单击右边屏幕
            {
                isMoveLeft = false;
                transform.DOMoveX(nextPlatformRight.x, 0.2f);
                transform.DOMoveY(nextPlatformRight.y+0.8f, 0.15f);

            }

            Jump();
        }
    }

    private void Jump()
    {
        if (isMoveLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);


        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Platform"))
        {
            isJumping = false;

            Vector3 currentPlatformPos = collision.gameObject.transform.position;
            nextPlatformLeft = new Vector3(currentPlatformPos.x - vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
            nextPlatformRight = new Vector3(currentPlatformPos.x + vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
        }
        
    }
}

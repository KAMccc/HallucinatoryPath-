using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Transform rayDown;

    public LayerMask platformLayer;

    //是否向左移动，反之向右
    private bool isMoveLeft = false;
    //是否正在跳跃
    private bool isJumping = false;
    private Vector3 nextPlatformLeft;
    private Vector3 nextPlatformRight;

    private ManagerVars vars;

    private Rigidbody2D my_Body;
    private SpriteRenderer spriteRenderer;
    


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        vars = ManagerVars.GetManagerVars();
        my_Body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameOver)
            return;
         
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

        //游戏结束
        if (my_Body.velocity.y < 0 && IsRayPlatform()==false && GameManager.Instance.IsGameOver == false)
        {
            spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.IsGameOver = true;
            //Todo 调用游戏结束面板
        }
    }

    private bool IsRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
        if(hit.collider!= null)
        {
            if(hit.collider.CompareTag("Platform"))
                return true;
        }
        return false;
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

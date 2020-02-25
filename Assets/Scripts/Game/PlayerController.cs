using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Transform rayDown;
    public Transform rayLeft;
    public Transform rayRight;

    public LayerMask platformLayer;
    public LayerMask obstacleLayer;

    //是否向左移动，反之向右
    private bool isMoveLeft = false;
    //是否正在跳跃
    private bool isJumping = false;
    private Vector3 nextPlatformLeft;
    private Vector3 nextPlatformRight;

    private ManagerVars vars;

    private Rigidbody2D my_Body;
    private SpriteRenderer spriteRenderer;

    private bool isMove = false;
    private AudioSource m_AudioSource;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        vars = ManagerVars.GetManagerVars();
        my_Body = GetComponent<Rigidbody2D>();
        m_AudioSource = GetComponent<AudioSource>();

        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
    }

    private void Start()
    {
        ChangeSkin(GameManager.Instance.GetCurrentSelectSkin());

    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);

    }

    private void Update()
    {
        Debug.DrawRay(rayDown.position,Vector2.down * 1f,Color.red);
        Debug.DrawRay(rayLeft.position,Vector2.left * 0.15f, Color.red);
        Debug.DrawRay(rayRight.position,Vector2.right * 0.15f, Color.red);

        //点击UI，不跳跃
        if (IsPointerOverGameObject(Input.mousePosition)) return;

        //区分平台
        //if (Application.platform == RuntimePlatform.WindowsEditor ||
        //    Application.platform == RuntimePlatform.WindowsPlayer)
        //{
        //    if (EventSystem.current.IsPointerOverGameObject()) return;
        //}
        //else
        //{
        //    int fingerId = Input.GetTouch(0).fingerId;
        //    if (EventSystem.current.IsPointerOverGameObject(fingerId)) return;
        //}

        if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameOver || GameManager.Instance.IsPause)
            return;

        if (Input.GetMouseButtonDown(0) && isJumping == false && nextPlatformLeft != Vector3.zero)
        {
            if (!isMove) 
            {
                EventCenter.Broadcast(EventDefine.PlayerMove);
                isMove = true;
            }

            m_AudioSource.PlayOneShot(vars.jumpClip);

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


        //第一种游戏结束方式，跳错了边
        if (my_Body.velocity.y < 0 && IsRayPlatform()==false && GameManager.Instance.IsGameOver == false)
        {
            m_AudioSource.PlayOneShot(vars.fallClip);

            spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.IsGameOver = true;
            //Todo 调用游戏结束面板
            Debug.LogError("【1】游戏结束，跳不到平台");
            StartCoroutine(DealyShowGameOverPanel());
        }

        //第二种游戏结束方式，碰到了障碍物
        if (isJumping && IsRayObstacle() && GameManager.Instance.IsGameOver == false)
        {
            m_AudioSource.PlayOneShot(vars.hitClip);

            GameObject go = ObjectPool.Instance.GetDeathEffect();
            go.SetActive(true);
            go.transform.position = transform.position;
            GameManager.Instance.IsGameOver = true;
            spriteRenderer.enabled = false;
            //Todo 调用游戏结束面板
            Debug.LogError("【2】游戏结束，撞到障碍物");
            StartCoroutine(DealyShowGameOverPanel());
        }

        if (transform.position.y - Camera.main.transform.position.y < -6f
            && !GameManager.Instance.IsGameOver)
        {
            m_AudioSource.PlayOneShot(vars.fallClip);

            GameManager.Instance.IsGameOver = true;
            //Todo 调用游戏结束面板
            Debug.LogError("【3】游戏结束，平台掉下");
            StartCoroutine(DealyShowGameOverPanel());
        }
    }

    IEnumerator DealyShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.5f);
        EventCenter.Broadcast(EventDefine.ShopGameOverPanel);

    }

    private GameObject lastHitGo = null;

    /// <summary>
    /// 是否检测到平台
    /// </summary>
    /// <returns></returns>
    private bool IsRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
        if(hit.collider!= null)
        {
            if (hit.collider.CompareTag("Platform")) {
                //一个平台只发送一次广播，不会每帧都加
                if(lastHitGo != hit.collider.gameObject)
                {
                    if(lastHitGo == null)
                    {
                        lastHitGo = hit.collider.gameObject;
                        return true;
                    }
                    EventCenter.Broadcast(EventDefine.AddScore);
                    lastHitGo = hit.collider.gameObject;
                }
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 是否检测到障碍物
    /// </summary>
    private bool IsRayObstacle()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);

        if(leftHit.collider != null)
        {
            if (leftHit.collider.CompareTag("Obstacle"))
            {
                return true;
            }
        }
        
        if(rightHit.collider != null)
        {
            if (rightHit.collider.CompareTag("Obstacle"))
            {
                return true;
            }
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

    /// <summary>
    /// 更换皮肤
    /// </summary>
    /// <param name="skinIndex"></param>
    private void ChangeSkin(int skinIndex)
    {
        spriteRenderer.sprite = vars.characterSkinSpriteList[skinIndex];
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //吃到钻石
        if (collision.collider.CompareTag("Pickup"))
        {
            m_AudioSource.PlayOneShot(vars.diamondClip);

            EventCenter.Broadcast(EventDefine.AddDiamond);
            collision.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 音效是否开启
    /// </summary>
    /// <param name="isMusicOn"></param>
    private void IsMusicOn(bool isMusicOn)
    {
        m_AudioSource.mute = !isMusicOn;
    }

    /// <summary>
    //// 创建一个点击事件判断是否点击到UI
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <returns></returns>
    private bool IsPointerOverGameObject(Vector2 mousePosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        //向点击位置发射射线
        EventSystem.current.RaycastAll(eventData, raycastResults);

        return raycastResults.Count > 0;
    }
}

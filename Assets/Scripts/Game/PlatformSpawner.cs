using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    //平台初始生成位置
    public Vector3 startSpawnPos;
    //生成平台数量
    private int spawnPlatformCount;
    private ManagerVars vars;
    //平台生成位置
    private Vector3 platformSpawnPosition;
    //是否朝左生成，反之朝右
    private bool isLeftSpawn = false;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);   
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);   
    }

    private void Start()
    {
        platformSpawnPosition = startSpawnPos;
        vars = ManagerVars.GetManagerVars();

        for (int i = 0; i < 5; i++)
        {
            spawnPlatformCount = 5;
            DecidePath();
        }

        //生成人物
        GameObject go =Instantiate(vars.characterPre);
        go.transform.position = new Vector3(0, -1.8f, 0);
    }
    /// <summary>
    /// 确定路径
    /// </summary>
    private void DecidePath()
    {
        if(spawnPlatformCount > 0)
        {
            spawnPlatformCount--;
            SpawnPlatform();
        }
        else
        {
            //反转生成方向
            isLeftSpawn = !isLeftSpawn;
            spawnPlatformCount = Random.Range(1, 4);
            SpawnPlatform();
        }
    }

    /// <summary>
    /// 生成平台
    /// </summary>
    private void SpawnPlatform()
    {
        SpawnNormalPlatform();

        if (isLeftSpawn)//向左生成
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, 0);
        }
        else//先右
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, 0);
        }

    }

    /// <summary>
    /// 生成普通平台(单个)
    /// </summary>
    private void SpawnNormalPlatform()
    {
        GameObject go = Instantiate(vars.normalPlatformPre, transform);
        go.transform.position = platformSpawnPosition;
    }
}

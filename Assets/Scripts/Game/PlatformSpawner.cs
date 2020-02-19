using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformGroupType
{
    Grass,
    Winter,
}

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
    //选择的平台主题
    private Sprite selectPlatformSprite;
    //组合平台的类型
    private PlatformGroupType groupType;
    //钉子组合平台是否生成在左边
    private bool spikeSpawnLeft = false;
    //钉子方向平台位置
    private Vector3 spikeDirPlatformPos;
    //生成钉子平台后需要在钉子方向生成的平台数量
    private int afterSpawnSpikeSpawnCount;

    private bool isSpawnSpike;
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();

        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);   
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);   
    }

    private void Start()
    {
        
        //调用随机生成主题
        RandomPlatformThheme();

        platformSpawnPosition = startSpawnPos;

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
    /// 随机平台主题
    /// </summary>
    private void RandomPlatformThheme()
    {
        int ran = Random.Range(0, vars.platformThemeSpriteLis.Count);
        selectPlatformSprite = vars.platformThemeSpriteLis[ran];

        if(ran == 2)
        {
            groupType = PlatformGroupType.Winter;
        }
        else
        {
            groupType = PlatformGroupType.Grass;
        }
    }

    /// <summary>
    /// 确定路径
    /// </summary>
    private void DecidePath()
    {
        if (isSpawnSpike)
        {
            AfterSpawnSpike();
            return;
        }

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
        //确定障碍物平台生成的方向
        int ranObstacleDir = Random.Range(0, 2);

        //生成单个平台
        if(spawnPlatformCount >=1)
        {
            SpawnNormalPlatform(ranObstacleDir);
        }
        //生成组合平台
        else if(spawnPlatformCount == 0)
        {
            int ran = Random.Range(0, 3);
            //生成通用组合平台
            if(ran == 0)
            {
                SpawnCommonPlatformGroup(ranObstacleDir);
            }
            //生成主题平台
            else if(ran == 1)
            {
                switch (groupType)
                {
                    case PlatformGroupType.Grass:
                        SpawnGrassPlatformGroup(ranObstacleDir);
                        break;
                    case PlatformGroupType.Winter:
                        SpawnWinterPlatformGroup(ranObstacleDir);
                        break;
                    default:
                        break;
                }
            }
            //生成钉子组合平台
            else
            {
                int value = -1;
                if (isLeftSpawn)
                {
                    value = 0;//生成右边方向的钉子
                }
                else
                {
                    value = 1;//生成左边的钉子
                }
                SpawnSpikePlatform(value);

                afterSpawnSpikeSpawnCount = 4;

                isSpawnSpike = true;

                //为钉子平台多加几个普通平台
                if (spikeSpawnLeft)
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x - 1.65f, platformSpawnPosition.y + vars.nextYPos, 0);
                }
                else
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x + 1.65f, platformSpawnPosition.y + vars.nextYPos, 0);
                }
            }
        }


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
    private void SpawnNormalPlatform(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetNormalPlatform();
        go.transform.position = platformSpawnPosition;
        //给PlatformScript传图
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, ranObstacleDir);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成通用组合平台
    /// </summary>
    private void SpawnCommonPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetCommonPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, ranObstacleDir);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成草地组合平台
    /// </summary>
    private void SpawnGrassPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetGrassPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, ranObstacleDir);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成冬季组合平台
    /// </summary>
    private void SpawnWinterPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetWinterPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, ranObstacleDir);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成钉子组合平台
    /// </summary>
    /// <param name="dir"></param>
    private void SpawnSpikePlatform(int dir)
    {
        GameObject temp;
        if(dir == 0)//生成右边钉子
        {
            spikeSpawnLeft = false;
            temp = ObjectPool.Instance.GetRightSpikePlatform();
        }
        else
        {
            spikeSpawnLeft = true;
            temp = ObjectPool.Instance.GetLeftSpikePlatform();
        }
        temp.transform.position = platformSpawnPosition;
        temp.GetComponent<PlatformScript>().Init(selectPlatformSprite, dir);
        temp.SetActive(true);
    }

    /// <summary>
    /// 生成钉子平台后需要生成的平台
    /// 包括钉子方向和原来方向
    /// </summary>
    private void AfterSpawnSpike()
    {
        if (afterSpawnSpikeSpawnCount > 0)
        {
            afterSpawnSpikeSpawnCount--;
            for (int i = 0; i < 2; i++)
            {
                GameObject temp = ObjectPool.Instance.GetNormalPlatform() ;

                if (i == 0)//生成原来方向的平台
                {
                    temp.transform.position = platformSpawnPosition;
                    //钉子生成在左边 原先路径在右边
                    if (spikeSpawnLeft)
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos,
                            platformSpawnPosition.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos,
                            platformSpawnPosition.y + vars.nextYPos, 0);
                    }
                }
                else//生成钉子方向的平台
                {
                    temp.transform.position = spikeDirPlatformPos;
                    if (spikeSpawnLeft)
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x - vars.nextXPos,
                            spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x + vars.nextXPos,
                            spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                }

                temp.GetComponent<PlatformScript>().Init(selectPlatformSprite,1);
                temp.SetActive(true);
            }
        }
        else
        {
            isSpawnSpike = false;
            DecidePath();
        }
    }
}

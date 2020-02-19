using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public int initSwpanCount = 5;

    private List<GameObject> normalPlatformList = new List<GameObject>();
    private List<GameObject> commonPlatformList = new List<GameObject>();
    private List<GameObject> grassPlatformList = new List<GameObject>();
    private List<GameObject> winterPlatformList = new List<GameObject>();
    private List<GameObject> spikePlatformLeftList = new List<GameObject>();
    private List<GameObject> spikePlatformRightList = new List<GameObject>();

    private ManagerVars vars;

    private void Awake()
    {
        Instance = this;
        vars = ManagerVars.GetManagerVars();
        Init();
    }

    private void Init()
    {
        //对象池初始化

        for (int i = 0; i < initSwpanCount; i++)
        {
            InstantiateObject(vars.normalPlatformPre, ref normalPlatformList);
        }

        for (int i = 0; i < initSwpanCount; i++)
        {
            for (int j = 0; j < vars.commonPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.commonPlatformGroup[j], ref commonPlatformList);
            }
        }

        for (int i = 0; i < initSwpanCount; i++)
        {
            for (int j = 0; j < vars.grassPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.grassPlatformGroup[j], ref grassPlatformList);
            }
        }

        for (int i = 0; i < initSwpanCount; i++)
        {
            for (int j = 0; j < vars.winterPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.winterPlatformGroup[j], ref winterPlatformList);
            }
        }

        for (int i = 0; i < initSwpanCount; i++)
        {
            InstantiateObject(vars.spikePlatformLeft, ref spikePlatformLeftList);
        }

        for (int i = 0; i < initSwpanCount; i++)
        {
            InstantiateObject(vars.spikePlatformRight, ref spikePlatformRightList);
        }

    }

    private GameObject InstantiateObject(GameObject prefab,ref List<GameObject> addlist)
    {
        GameObject go = Instantiate(prefab, transform);
        go.SetActive(false);
        addlist.Add(go);
        return go;
    }

    /// <summary>
    /// 获得对象池中的普通平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetNormalPlatform()
    {
        for (int i = 0; i < normalPlatformList.Count; i++)
        {
            if(normalPlatformList[i].activeInHierarchy == false)
            {
                return normalPlatformList[i];
            }
        }

        return InstantiateObject(vars.normalPlatformPre, ref normalPlatformList); 
    }

    /// <summary>
    /// 获得对象池中的通用平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetCommonPlatformGroup()
    {
        for (int i = 0; i < commonPlatformList.Count; i++)
        {
            if (commonPlatformList[i].activeInHierarchy == false)
            {
                return commonPlatformList[i];
            }
        }

        int ran = Random.Range(0, vars.commonPlatformGroup.Count);

        return InstantiateObject(vars.commonPlatformGroup[ran], ref commonPlatformList);
    }

    /// <summary>
    /// 获得对象池中的草地平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetGrassPlatformGroup()
    {
        for (int i = 0; i < grassPlatformList.Count; i++)
        {
            if (grassPlatformList[i].activeInHierarchy == false)
            {
                return grassPlatformList[i];
            }
        }

        int ran = Random.Range(0, vars.grassPlatformGroup.Count);

        return InstantiateObject(vars.grassPlatformGroup[ran], ref grassPlatformList);
    }

    /// <summary>
    /// 获得对象池中的冬季平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetWinterPlatformGroup()
    {
        for (int i = 0; i < winterPlatformList.Count; i++)
        {
            if (winterPlatformList[i].activeInHierarchy == false)
            {
                return winterPlatformList[i];
            }
        }

        int ran = Random.Range(0, vars.winterPlatformGroup.Count);

        return InstantiateObject(vars.winterPlatformGroup[ran], ref winterPlatformList);
    }

    /// <summary>
    /// 获得对象池中的左边钉子
    /// </summary>
    /// <returns></returns>
    public GameObject GetLeftSpikePlatform()
    {
        for (int i = 0; i < spikePlatformLeftList.Count; i++)
        {
            if (spikePlatformLeftList[i].activeInHierarchy == false)
            {
                return spikePlatformLeftList[i];
            }
        }

        return InstantiateObject(vars.spikePlatformLeft, ref spikePlatformLeftList);
    }

    /// <summary>
    /// 获得对象池中的右边钉子
    /// </summary>
    /// <returns></returns>
    public GameObject GetRightSpikePlatform()
    {
        for (int i = 0; i < spikePlatformRightList.Count; i++)
        {
            if (spikePlatformRightList[i].activeInHierarchy == false)
            {
                return spikePlatformRightList[i];
            }
        }

        return InstantiateObject(vars.spikePlatformRight, ref spikePlatformRightList);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 对象池
/// </summary>
public class PoolData
{
    public GameObject fatherObj;
    //对象的容器
    public List<GameObject> poolList;

    public PoolData(GameObject obj, GameObject poolObj)
    {
        //创建一个父对象 并且把他作为我们对象的子物体
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>() {};
        PushObj(obj);
    }

    /// <summary>
    /// 往池子里面放东西
    /// </summary>
    /// <param name="obj"></param>
    public void PushObj(GameObject obj)
    {
        //失活 让其隐藏
        obj.SetActive(false);
        //存起来
        poolList.Add(obj);
        //设置父对象
        obj.transform.parent = fatherObj.transform;
    }

    /// <summary>
    /// 往池子里面取东西
    /// </summary>
    /// <returns></returns>
    public GameObject GetObj()
    {
        GameObject obj = null;
        //取出第一个
        obj = poolList[0];
        poolList.RemoveAt(0);
        //激活 让其显示
        obj.SetActive(true);
        //断开了父子关系
        obj.transform.parent = null;

        return obj;
    }
}

/// <summary>
/// 缓存池模块
/// 1.Dictionary List
/// 2.GameObject 和 Resources 两个公共类中的 API 
/// </summary>
public class PoolManager : SingletonBase<PoolManager>
{
    //缓存池容器
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    private GameObject poolObj;

    /// <summary>
    /// 获得
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public void Get(string name, UnityAction<GameObject> callBack)
    {
        //有池子 并且池子里有东西
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            callBack(poolDic[name].GetObj());
        }
        else
        {
            //通过异步加载资源 创建对象给外部用
            ResManager.Instance.LoadAsync<GameObject>(name, (o) =>
            {
                o.name = name;
                callBack(o);
            });
        }
    }

    /// <summary>
    /// 回收
    /// </summary>
    public void Push(string name, GameObject obj)
    {
        if (poolObj == null)
            poolObj = new GameObject("Pool");

        //里面有池子
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        //里面没有池子
        else
        {
            poolDic.Add(name, new PoolData(obj, poolObj));
        }
    }


    /// <summary>
    /// 清空缓存池的方法 
    /// 主要用在 场景切换时
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectUtil
{
    private static Dictionary<RecycleGameobject, ObjectPool> pools = new Dictionary<RecycleGameobject, ObjectPool>();
    public static GameObject Instantiate(GameObject prefab , Vector3 pos)
    {
        GameObject instance = null;
        var recycleComponent = prefab.GetComponent<RecycleGameobject>();
        if (recycleComponent != null)
        {
            //放入对象池
            var pool = GetObjectPool(recycleComponent);
            instance = pool.NextObject(pos).gameObject;
        }
        else
        {
            instance = GameObject.Instantiate(prefab);
            instance.transform.position = pos;
        }
        return instance;
    }

    public static void Destroy(GameObject gameObject)
    {
        var recycleGameObject = gameObject.GetComponent<RecycleGameobject>();
        //判断是否具有可复用功能
        if (recycleGameObject != null)
        {
            recycleGameObject.Shutdown();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private static ObjectPool GetObjectPool(RecycleGameobject reference)
    {
        ObjectPool pool = null;
        if (pools.ContainsKey(reference))
        {
            pool = pools[reference];
        }
        else
        {
            //如果所查询的对象池不存在则创建一个新的
            var poolContainer = new GameObject(reference.gameObject.name + "ObjectPool");
            pool = poolContainer.AddComponent<ObjectPool>();
            pool.prefab = reference;
            pools.Add(reference, pool);
        }
        return pool;
    }

}

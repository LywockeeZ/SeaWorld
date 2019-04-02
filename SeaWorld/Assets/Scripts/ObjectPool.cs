using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public RecycleGameobject prefab;
    public List<RecycleGameobject> poolInstances = new List<RecycleGameobject>();

    private RecycleGameobject CreateInstance(Vector3 pos)
    {
        var clone = GameObject.Instantiate(prefab);
        clone.transform.position = pos;
        clone.transform.parent = transform;
        poolInstances.Add(clone);
        return clone;
    }

    public RecycleGameobject NextObject(Vector3 pos)
    {
 
        RecycleGameobject instance = null;
        //遍历查看是否有可用的未激活的对象
        foreach (var go in poolInstances)
        {
            if (go.gameObject.activeSelf != true)
            {
                instance = go;
                instance.transform.position = pos;
            }
        }
        //如果没有就创建一个
        if (instance == null)
        {
            instance = CreateInstance(pos);
        }
        instance.Restart();
        return instance; 
    }
}

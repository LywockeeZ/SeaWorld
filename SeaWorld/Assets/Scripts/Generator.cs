using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public float delay = 1f;
    public bool active = false;
    public GameObject[] prefabs;
    public int[] activeCounts;        //激活状态下的个数
    public float GenerateRadius = 8f;

    private void Awake()
    {
        SpawnerManager.Instance.generators.Add(this);
    }

    void Start()
    {

        StartCoroutine(MyGenerator());
    }
     
    IEnumerator MyGenerator()
    {
        yield return new WaitForSeconds(delay);
        if (SpawnerManager.Instance.active == true)
        {
            if (active)
            {
                switch (GameManager.Instance.gameLevel)
                {
                    case 1:
                        InstantiatePrefabs(0, 2);
                        break;
                    case 2:
                        InstantiatePrefabs(0, 3);
                        break;
                    case 3:
                        InstantiatePrefabs(0, 4);
                        break;
                    default:
                        InstantiatePrefabs(0, 1);
                        break;
                }

            }
        }
        

        StartCoroutine(MyGenerator());
    }

    //maxIndex用来控制实例化prefabs数组的范围
    public void InstantiatePrefabs(int initIndex , int maxIndex)
    {
        var newTransfrom = transform;
        for (int index = initIndex; index <= maxIndex; index++)
        {
            ObjectPool targetPool = null;
            if (GameObjectUtil.poolList.Count != 0)
            {
                //获得相应的对象池
                foreach (var item in GameObjectUtil.poolList)
                {
                    if (item.gameObject.name == prefabs[index].name + "ObjectPool")
                    {
                        targetPool = item;
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                    
                }
                //如果没有，则新建一个对象池然后结束
                if (targetPool == null)
                {
                    if (activeCounts[index] != 0)
                    {
                        GameObjectUtil.Instantiate(prefabs[index], newTransfrom.position + (Vector3)Random.insideUnitCircle * GenerateRadius);
                    }
                    continue;
                }
                //如果有对象池，则检查对象池中激活的实例个数
                int activedCount = 0;
                foreach (var item in targetPool.poolInstances)
                {
                    if (item.gameObject.activeInHierarchy == true)
                    {
                        activedCount++;
                    }
                }
                //如果当前激活个数小于目标个数，则激活或者新建
                if (activedCount < activeCounts[index])
                {
                    
                    GameObjectUtil.Instantiate(prefabs[index], newTransfrom.position + (Vector3)Random.insideUnitCircle * GenerateRadius);//通过游戏对象管理器实例化对象，使对象生成可控
                }

            }
            else
            {
                GameObjectUtil.Instantiate(prefabs[index], newTransfrom.position + (Vector3)Random.insideUnitCircle * GenerateRadius);
            }
            
            
        }
        
        
    }


    private void OnDrawGizmos()
    {
        if (active == false)
        {
            Gizmos.color = Color.grey;
        }
        else Gizmos.color = Color.green;
        
        Gizmos.DrawWireSphere(transform.position, GenerateRadius);
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public float delay = 1f;
    public bool active = false;
    public GameObject[] prefabs;
    int count = 0;
    [Range(10, 500)]
    public int startingCount = 250;
    const float AgentDensity = 0.08f;//密度
    [Range(0, 500)]
    public int maxCount = 20;
    // Start is called before the first frame update
    void Start()
    {
        //for (count = 0; count < startingCount; count++)
        //{
        //    GameObjectUtil.Instantiate(prefabs[Random.Range(0, prefabs.Length)],
        //        Random.insideUnitCircle * startingCount * AgentDensity);

        //}
        StartCoroutine(MyGenerator());
    }
     
    IEnumerator MyGenerator()
    {
        yield return new WaitForSeconds(delay);
        if (active)
        {
            if (count < maxCount)
            {
                var newTransfrom = transform;
                //通过游戏对象管理器实例化对象，使对象生成可控
                GameObjectUtil.Instantiate(prefabs[Random.Range(0, prefabs.Length)], newTransfrom.position );
                count++;
            }
            
        }

        StartCoroutine(MyGenerator());
    }
}

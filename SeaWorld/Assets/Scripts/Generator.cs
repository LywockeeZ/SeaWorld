﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public float delay = 1f;
    public bool active = false;
    public GameObject[] prefabs;
    //[Range(0, 500)]
    //public int maxCount = 20;
    //int count = 0;                  //生成的总数
    //public int Count { get { return count; } set { count = value; } }
    public float GenerateRadius = 8f;


    protected static Generator _instance;
    public static Generator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Generator>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<Generator>();
                }
            }
            return _instance;
        }
    }
    void Start()
    {

        StartCoroutine(MyGenerator());
    }
     
    IEnumerator MyGenerator()
    {
        yield return new WaitForSeconds(delay);
        if (active)
        {
                var newTransfrom = transform;
            //通过游戏对象管理器实例化对象，使对象生成可控
            if (GameManager.Instance.IncreaseSpeed <= 2)
            {
                GameObjectUtil.Instantiate(prefabs[Random.Range(0, 2)], newTransfrom.position + (Vector3)Random.insideUnitCircle * GenerateRadius);
            }
            else
            if ((GameManager.Instance.IncreaseSpeed > 2)&&(GameManager.Instance.IncreaseSpeed <= 4))
            {
                GameObjectUtil.Instantiate(prefabs[Random.Range(0, 3)], newTransfrom.position + (Vector3)Random.insideUnitCircle * GenerateRadius);
            }
            else
            {
                GameObjectUtil.Instantiate(prefabs[Random.Range(0, 4)], newTransfrom.position + (Vector3)Random.insideUnitCircle * GenerateRadius);
            }
                ; 
            
        }

        StartCoroutine(MyGenerator());
    }

}

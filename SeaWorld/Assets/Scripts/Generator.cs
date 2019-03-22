using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public float delay = 1f;
    public bool active = false;
    public GameObject[] prefabs;
    // Start is called before the first frame update
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
            GameObjectUtil.Instantiate(prefabs[Random.Range(0,prefabs.Length)], newTransfrom.position);
        }

        StartCoroutine(MyGenerator());
    }
}

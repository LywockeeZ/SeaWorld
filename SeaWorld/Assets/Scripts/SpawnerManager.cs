using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public List<Generator> generators = new List<Generator>();
    public GameObject[] basicGameobject; 
    public bool active = false;


    protected static SpawnerManager _instance;
    public static SpawnerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SpawnerManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<SpawnerManager>();
                }
            }
            return _instance;
        }
    }

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }


    public void TraverseAllAndReplace(string name , GameObject before , GameObject after)
    {
        for (int i = 0; i < generators.Count; i++)
        {
            for (int j = 0; j < generators[i].prefabs.Length; j++)
            {
                if (generators[i].prefabs[j] == FlockManager.Instance.FlockPrefab)
                {
                    generators[i].prefabs[j] = before;
                }

                if (generators[i].prefabs[j].name == name)
                {
                    generators[i].prefabs[j] = after;
                }
                
     
            }
        }
    }


    public void TraverseAllAndReplaceBack(GameObject gameObject)
    {
        for (int i = 0; i < generators.Count; i++)
        {
            for (int j = 0; j < generators[i].prefabs.Length; j++)
            {
                if (generators[i].prefabs[j] == FlockManager.Instance.FlockPrefab)
                {
                    generators[i].prefabs[j] = gameObject;
                }
            }
        }
    }


    public GameObject FindOriginal(string name)
    {
        for (int i = 0; i < basicGameobject.Length; i++)
        {
            if (basicGameobject[i].name == name)
            {
                return basicGameobject[i];
            }
        }
        return null;
    }
}

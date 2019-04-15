using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoUnlockZ : MonoBehaviour
{
    private EnemyAI myEnemyAI;
    // Start is called before the first frame update
    void Start()
    {
        myEnemyAI = GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.Instance.gameLevel >= 4)
        //{
        //    myEnemyAI.canFollow = true;
           
        //}
    }



}

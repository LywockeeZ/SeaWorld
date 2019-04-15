using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public int PlayerId = 0;
    public int Damage = 5;
    public int DamageWithSqueeze = 10;
    public int MaxHealt = 100;
    public int CurrentHealt = 50;
    public int Weight = 20;
    public GameObject[] meatObjArray;

    
    public void AddHealt(int meatPoint)
    {
        int currentHealt = CurrentHealt + meatPoint;
        CurrentHealt = Mathf.Clamp(currentHealt, 0, MaxHealt);
    }

    public void ReduceHealt(int damage)
    {
        CurrentHealt -= damage;
        if (CurrentHealt <= 0)
        {
            CurrentHealt = 0;
            if (meatObjArray != null)
            {
                foreach (GameObject item in meatObjArray)
                {
                    if (item != null)
                    {
                        float xOffset = UnityEngine.Random.Range(1f, 2f);
                        float yOffset = UnityEngine.Random.Range(1f, 3f);
                        float zOffset = UnityEngine.Random.Range(1f, 3f);
                        Vector3 offset = new Vector3(xOffset, yOffset, zOffset);
                        Vector3 newMeatPos = this.transform.position + offset;
                        Instantiate(item, newMeatPos, Quaternion.identity);
                    }
                }
            }
            Destroy(gameObject);
        }
    }

    //private void FixedUpdate()
    //{
    //    if (stickToBiteFlag)
    //    {
    //        MoveToBite();
    //    }
    //}
}

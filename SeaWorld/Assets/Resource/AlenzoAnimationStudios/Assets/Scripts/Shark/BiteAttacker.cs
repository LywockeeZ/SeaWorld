using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteAttacker : MonoBehaviour
{
    public int Damage = 5;
    public List<GameObject> preyList;

    void Start()
    {
        preyList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Prey")
        {
            preyList.Add(other.gameObject);
        }
    }

    public void ClearPreyList()
    {
        preyList.Clear();
    }
}

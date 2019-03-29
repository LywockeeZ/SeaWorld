using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteDectector : MonoBehaviour
{
    public bool Baiting = false;
    public bool BaitSqueezing = false;
    private PlayerStats stats;

    void Start()
    {
        stats = transform.root.GetComponent<PlayerStats>();
    }

    public int GetDamage()
    {
        return transform.root.GetComponent<PlayerStats>().Damage;
    }

    public int GetDamageWithSqueeze()
    {
        return transform.root.GetComponent<PlayerStats>().DamageWithSqueeze;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meat"))
        {
            if (other.gameObject.GetComponent<Meat>()) //eat meat gain points life!!
            {
                Meat meat = other.gameObject.GetComponent<Meat>();
                stats.AddHealt(meat.meatPoint);
                Debug.Log("One peace of meath detected");
            }
        }
    }
}

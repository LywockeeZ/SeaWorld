using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtDetectorL : MonoBehaviour
{
    private Animator animator;

    private static int hurtLAnimHash = Animator.StringToHash("isHurtL");

    private void Start()
    {
        animator = transform.root.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BiteDetector"))
        {
            animator.SetBool(hurtLAnimHash, true);
            Invoke("RestoreBaseLayer", 1);
        }
    }

    private void RestoreBaseLayer()
    {
        animator.SetBool(hurtLAnimHash, false);
    }
}

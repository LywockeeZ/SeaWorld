using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtDetectorR : MonoBehaviour
{
    private Animator animator;

    private static int hurtRAnimHash = Animator.StringToHash("isHurtR");

    private void Start()
    {
        animator = transform.root.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BiteDetector"))
        {
            animator.SetBool(hurtRAnimHash, true);
            Invoke("RestoreBaseLayer", 2);
        }
    }

    private void RestoreBaseLayer()
    {
        animator.SetBool(hurtRAnimHash, false);
    }
}

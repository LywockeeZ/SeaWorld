using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAnimControl : MonoBehaviour
{
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        
    }


    public void BeginMainMenu()
    {
        UIManager.Instance.Open("MainMenu");
        animator.enabled = false;
        

    }


    public void BeginShopMenu()
    {
        UIManager.Instance.Open("ShopMenu");
    }

    public void StartMoveToShop()
    {
        UIManager.Instance.Close("MainMenu");
        animator.enabled = true;
        animator.Play("Cam_MoveToShop");
    }

    public void StartBackToMainFromShop()
    {
        UIManager.Instance.Close("ShopMenu");
        animator.Play("Cam_FromShopToMain");
    }


}

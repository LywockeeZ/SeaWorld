using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas MainMenu;
    public Canvas ShopMenu;
    public Canvas SetupMenu;
    public Canvas PauseMenu;
    public Canvas MakersMenu;
    public Canvas GameOverMenu;


    protected static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<UIManager>();
                }
            }
            return _instance;
        }
    }

    private void Start()
    {
        Close("ShopMenu");
    }


    void Update()
    {
        
    }


    public void Open(string menuName)
    {
        var menu = GetMenu(menuName);
        menu.gameObject.SetActive(true);
    }


    public void Close(string menuName)
    {
        var menu = GetMenu(menuName);
        menu.gameObject.SetActive(false);
    }


    public Canvas GetMenu(string menuName)
    {
        switch (menuName)
        {
            case "MainMenu":return MainMenu;
            case "ShopMenu": return ShopMenu;
            case "SetupMenu": return SetupMenu;
            case "PauseMenu": return PauseMenu;
            case "MakersMenu": return MakersMenu;
            case "GameOverMenu":return GameOverMenu;
            default: return null;
        }
    }

    public void GameOver()
    {
        Open("GameOverMenu");
    }
    
}

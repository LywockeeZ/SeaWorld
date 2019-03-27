using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int scores;
    public Generator generator;
    public Text scoreText;

    protected static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scores = FlockManager.Instance.Flocks.Count * 30;
        scoreText.text = scores.ToString();
        if (FlockManager.Instance.Flocks.Count == 0)
        {
            GameOver();
        }
    }


    public void GameStart()
    {
        generator.active = true;
    }

    public void GameOver()
    {
        Application.Quit();
    }
}

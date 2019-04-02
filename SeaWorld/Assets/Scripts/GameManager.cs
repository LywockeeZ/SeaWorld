using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int scores;
    public Generator generator;
    public Text scoreText;
    public int gameLevel = 1;
    float increaseSpeed = 0f;
    public float IncreaseSpeed { get { return increaseSpeed; } set { increaseSpeed = value; } }

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
        
        scoreText.text = scores.ToString();
        if (FlockManager.Instance.Flocks.Count == 0)
        {
            GameOver();
        }
        if (increaseSpeed >4.5)
        {
            StopAllCoroutines();
        }
    }


    public void GameStart()
    {
        generator.active = true;
        StartCoroutine(ChangeSpeed());
    }

    public void GameOver()
    {
        Application.Quit();
    }


    IEnumerator ChangeSpeed()
    {
        increaseSpeed += 0.05f;
        Debug.Log(increaseSpeed);
        yield return new WaitForSeconds(1f);
        StartCoroutine(ChangeSpeed());
    }

}

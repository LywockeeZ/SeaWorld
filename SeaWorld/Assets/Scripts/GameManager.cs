using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int scores;
    public Generator generator;
    public Text scoreText;
    public int gameLevel = 1;
    float increaseSpeed = 0f;
    public float IncreaseSpeed { get { return increaseSpeed; } set { increaseSpeed = value; } }
    private bool isGameOver = false;

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
 

    // Update is called once per frame
    void Update()
    {
        
        scoreText.text = scores.ToString();
        if (FlockManager.Instance.Flocks.Count == 0)
        {
            if (!isGameOver)
            {
                //GameOver();
            }
            
        }
        if (increaseSpeed >4.5)
        {
            StopAllCoroutines();
        }
    }


    public void GameStart()
    {
        SpawnerManager.Instance.active = true;
        FlockManager.Instance.FlockPrefab.GetComponent<FlockAI>().CanMove = true;
        FlockManager.Instance._flockPrefab.GetComponent<FlockAI>().CanMove = true;
        FlockManager.Instance.FlockPrefab.GetComponent<DestroyOffscreen>().canShutDown = true;
        StartCoroutine(ChangeSpeed());
    }

    public void GameOver()
    {
        isGameOver = true;
        UIManager.Instance.GameOver();
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(0);
    }


    IEnumerator ChangeSpeed()
    {
        increaseSpeed += 0.05f;
        Debug.Log(increaseSpeed);
        yield return new WaitForSeconds(1f);
        StartCoroutine(ChangeSpeed());
    }

}

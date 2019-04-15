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
    public AudioSource ScoredSound;
    public AudioSource HitSound;
    public AudioSource PlayerHit;
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

    private void Awake()
    {
        ScoredSound = GameObject.Find("Scored").GetComponent<AudioSource>();
        HitSound = GameObject.Find("Hit").GetComponent<AudioSource>();
        PlayerHit = GameObject.Find("PlayerHit").GetComponent<AudioSource>();
    }

    void Update()
    {
        ChangeGameLevel();
        scoreText.text = scores.ToString();
        if (FlockManager.Instance.Flocks.Count == 0)
        {
            GameOver();
            if ( Input.GetMouseButtonDown(0))
            {
                
                GameRestart();
                
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameRestart();
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
        //var rank = (int)FlockManager.Instance._flockPrefab.GetComponent<FlockAI>().Rank[4] - 48;
        //CameraController.Instance.CamDistance = -15 - rank;
        //CameraController.Instance.offset = new Vector3(CameraController.Instance.offset.x, CameraController.Instance.offset.y + (float)rank / 2 +2 , CameraController.Instance.offset.z);
        var fishParamater = FlockManager.Instance._flockPrefab.GetComponent<FishParameter>();
        CameraController.Instance.CamDistance = fishParamater.camDistance;
        CameraController.Instance.offset = new Vector3(CameraController.Instance.offset.x, fishParamater.camYoffset , CameraController.Instance.offset.z);

    }

    public void GameOver()
    {
        UIManager.Instance.GameOver();
    }

    public void GameRestart()
    {
        DontDestroyOnLoad(GameObject.Find("Sounds"));
        SceneChanger.Instance.FadeToScene(1);
        //SceneManager.LoadScene(1);
    }

    public void ChangeGameLevel()
    {
        if (increaseSpeed >= 1 && increaseSpeed < 2)
        {
            gameLevel = 1;
        }

        if (increaseSpeed >= 2 && increaseSpeed < 3)
        {
            gameLevel = 2;
        }

        if (increaseSpeed >= 3 && increaseSpeed < 4.5)
        {
            gameLevel = 3;
        }

        if (increaseSpeed >= 4.5)
        {
            gameLevel = 4;
        }
    }

    IEnumerator ChangeSpeed()
    {
        increaseSpeed += 0.05f;
        Debug.Log(increaseSpeed);
        yield return new WaitForSeconds(1f);
        StartCoroutine(ChangeSpeed());
    }

}

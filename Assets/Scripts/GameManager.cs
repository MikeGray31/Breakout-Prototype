using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Camera mainCamera;
    public Camera MainCamera { get { return mainCamera; } }

    private PlayerController player;
    public PlayerController Player { get { return player; } }

    private BallScript ball;
    public BallScript Ball { get { return ball; } }

    private List<BrickScript> bricks;

    [SerializeField] private float StartingLives;
    private float Lives;
    private bool IsGameOver;

    [SerializeField] private TMP_Text livesText;

    /*[SerializeField] private float topYLimit;
    public float TopYLimit { get { return topYLimit; } }
    
    [SerializeField] private float bottomYLimit;
    public float BottomYLimit { get { return bottomYLimit; } }*/

    [SerializeField] private float maxDistanceFromCenter;
    public float MaxDistanceFromCenter { get { return maxDistanceFromCenter; } }

    public int score;
    [SerializeField] private TMP_Text scoreText;

    private bool gamePaused;
    [SerializeField] private GameObject ControlsExplanation;
    [SerializeField] private GameObject YouWinScreen;
    [SerializeField] private GameObject GameOverScreen;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        bricks = new List<BrickScript>();

        gamePaused = false;
    }

    private void Update()
    {
        CheckForPause();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UnsubsribeFromPlayerEvents();
        player = FindFirstObjectByType<PlayerController>();
        SubsribeToPlayerEvents();

        UnsubsribeFromBallEvents();
        ball = FindFirstObjectByType<BallScript>();
        SubsribeToBallEvents();

        mainCamera = FindFirstObjectByType<Camera>();
        
        OnLevelStart();
    }

    public void SubsribeToPlayerEvents()
    {
        if (player != null)
        {
            
        }
        /*else
        {
            Debug.LogWarning("GameManager.player is null!");
        }*/
    }

    public void UnsubsribeFromPlayerEvents()
    {
        if (player != null)
        {
            
        }
        /*else
        {
            Debug.LogWarning("GameManager.player is null!");
        }*/
    }

    public void SubsribeToBallEvents()
    {
        if (ball != null)
        {
            ball.OnReset += OnBallReset;
        }
        /*else
        {
            Debug.LogWarning("GameManager.player is null!");
        }*/
    }

    public void UnsubsribeFromBallEvents()
    {
        if (ball != null)
        {
            ball.OnReset -= OnBallReset;
        }
        /*else
        {
            Debug.LogWarning("GameManager.player is null!");
        }*/
    }

    public void OnLevelStart()
    {
        Lives = StartingLives;
        IsGameOver = false;

        livesText.text = ("Lives: " + Lives);


        YouWinScreen.SetActive(false);
        GameOverScreen.SetActive(false);

        score = 0;
        scoreText.text = ("score: " + score);
        //Debug.Log("OnLevelStart called!");
        Pause();
        GetBricks();
        CenterPlayer();
        BallSetup();
    }

    public void CenterPlayer()
    {
        if (player != null)
        {
            //player.transform.position = new Vector3(0, -6f, 0);
        }
    }

    public void GetBricks()
    {
        if(bricks.Count > 0)
        {
            foreach(BrickScript b in bricks)
            {
                b.BallHit -= BrickHit;
                b.BrickDestroyed -= BrickDestroyed;
            }
        }
        bricks.Clear();
        bricks.AddRange(FindObjectsByType<BrickScript>(FindObjectsSortMode.None));
        foreach(BrickScript b in bricks)
        {
            Debug.Log("how many times is this called? => ");
            b.BallHit += BrickHit;
            b.BrickDestroyed += BrickDestroyed;
        }
    }

    public void BrickHit(BrickScript brick)
    {
        IncreaseScore(5);
    }

    public void BrickDestroyed(BrickScript brick)
    {
        bricks.Remove(brick);
        IncreaseScore(10);
        Destroy(brick.gameObject);
        if(bricks.Count <= 0)
        {
            Debug.Log("All bricks are gone!");
            WinGame();
        }
    }

    public void IncreaseScore(int increase)
    {
        score += increase;
        scoreText.text = ("score: " + score);
        //Debug.Log("Score = " + score);
    }

    public void BallSetup()
    {
        //ball.BallInitialMove();
    }

    public void PullBall(Transform source, float force, float dampingFactor)
    {
        if (ball != null)
        { 
            ball.Pulled(source, force , dampingFactor); 
        }
    }

    public void PushBall(Transform source, float force, float dampingFactor)
    {
        if (ball != null)
        {
            ball.Pushed(source, force, dampingFactor);
        }
    }

    public void OnBallReset()
    {
        Lives--;
        livesText.text = ("Lives: " + Lives);

        if (Lives <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        GameOverScreen.SetActive(true);
        IsGameOver = true;
    }


    public void CheckForPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IsGameOver)
        {
            if (!gamePaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        ControlsExplanation.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void Resume()
    {
        ControlsExplanation.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }


    public void WinGame()
    {
        YouWinScreen.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }
}

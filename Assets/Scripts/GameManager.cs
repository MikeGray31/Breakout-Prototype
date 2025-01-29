using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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

    [SerializeField] private float topYLimit;
    public float TopYLimit { get { return topYLimit; } }
    
    [SerializeField] private float bottomYLimit;
    public float BottomYLimit { get { return bottomYLimit; } }


    public int score;

    private bool gamePaused;
    [SerializeField] private GameObject ControlsExplanation;
    [SerializeField] private GameObject YouWinScreen;

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

        ball = FindFirstObjectByType<BallScript>();
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

    public void OnLevelStart()
    {

        YouWinScreen.SetActive(false);

        score = 0;
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
            player.transform.position = new Vector3(0, -4.5f, 0);
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
        Destroy(brick.gameObject, 0.01f);
        if(bricks.Count <= 0)
        {
            Debug.Log("All bricks are gone!");
            WinGame();
        }
    }

    public void IncreaseScore(int increase)
    {
        score += increase;
        //Debug.Log("Score = " + score);
    }

    public void BallSetup()
    {
        ball.BallInitialMove();
    }

    public void PullBall(Transform source, float force)
    {
        if (ball != null)
        { 
            ball.Pulled(source, force); 
        }
    }

    public void PushBall(Transform source, float force)
    {
        if (ball != null)
        {
            ball.Pushed(source, force);
        }
    }


    public void CheckForPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

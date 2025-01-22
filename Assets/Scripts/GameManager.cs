using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlayerController player;
    private Camera mainCamera;

    private BallScript ball;

    private List<BrickScript> bricks;

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
        //Debug.Log("OnLevelStart called!");
        GetBricks();
        CenterPlayer();
        BallSetup();
    }

    public Camera GetMainCamera()
    {
        return mainCamera;
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
                b.BallHit -= BrickDestroyed;
            }
        }
        bricks.Clear();
        bricks.AddRange(FindObjectsByType<BrickScript>(FindObjectsSortMode.None));
        foreach(BrickScript b in bricks)
        {
            Debug.Log("how many times is this called? => ");
            b.BallHit += BrickDestroyed;
        }
    }

    public void BrickDestroyed(BrickScript brick)
    {
        bricks.Remove(brick);
        Destroy(brick.gameObject, 0.01f);
        if(bricks.Count <= 0)
        {
            Debug.Log("All bricks are gone!");
        }
    }

    public void BallSetup()
    {
        ball.BallInitialMove();
    }

    public void BallReset()
    {

    }
}

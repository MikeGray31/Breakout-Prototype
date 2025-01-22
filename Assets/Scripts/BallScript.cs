using System;
using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
    private Rigidbody2D rb;

    public event Action<BallScript> ballDropped;

    public float ballResetTimer;
    public Vector2 InitialPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        InitialPosition = new Vector2(transform.position.x, transform.position.y);
        ballResetTimer = 0;
    }

    private void Update()
    {
        CheckForReset();
    }

    public void BallInitialMove()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if(rb != null)
        {
            Vector2 forceVector = new Vector2(0.25f, -0.5f);
            forceVector = forceVector.normalized;
            forceVector = forceVector * 240f;
            rb.AddForce(forceVector);
        }
    }

    public void CheckForReset()
    {
        if(transform.position.y < -5f)
        {
            ballResetTimer += Time.deltaTime;
        }
        else
        {
            ballResetTimer = 0;
        }

        if(ballResetTimer >= 1f)
        {
            StartCoroutine(BallReset());
        }
    }

    IEnumerator BallReset()
    {
        transform.position = InitialPosition;
        rb.linearVelocity = new Vector2(0f, 0f);
        ballResetTimer = 0;
        yield return new WaitForSeconds(0.5f);
        BallInitialMove();
    }
    
}

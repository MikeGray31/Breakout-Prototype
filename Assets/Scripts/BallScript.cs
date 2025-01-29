using System;
using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
    private Rigidbody2D rb;

    public event Action<BallScript> ballDropped;

    private float ballResetTimer;
    public Vector2 InitialPosition;

    [SerializeField] private GameObject magnetLine;

    [SerializeField] private float MaxVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        InitialPosition = new Vector2(transform.position.x, transform.position.y);
        ballResetTimer = 0;
    }

    private void Update()
    {
        CheckForBorderReset();
        LimitSpeed();
        UpdateVisuals();
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

    public void CheckForBorderReset()
    {
        if(transform.position.y < GameManager.Instance.BottomYLimit || transform.position.y > GameManager.Instance.TopYLimit)
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
        //BallInitialMove();
    }
    
    public void Pulled(Transform source, float force)
    {
        Vector2 direction = (source.position - transform.position).normalized;
        rb.AddForce(direction * force, ForceMode2D.Force);
    }

    public void Pushed(Transform source, float force)
    {
        Vector2 direction = (transform.position - source.position).normalized;
        rb.AddForce(direction * force, ForceMode2D.Force);
    }

    public void LimitSpeed()
    {
        if(rb.linearVelocity.magnitude > MaxVelocity)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * MaxVelocity;
        }
    }

    public void UpdateVisuals()
    {
        Vector2 lookDir = transform.position - GameManager.Instance.Player.transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        magnetLine.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Pit")
        {
            Debug.Log("Ball hit a pit!");
            StartCoroutine(BallReset());
        }
    }
}

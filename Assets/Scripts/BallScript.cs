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

    [SerializeField] private GameObject PushLine;
    private bool PushLineActive;
    [SerializeField] private GameObject PullLine;
    private bool PullLineActive;

    public event Action OnReset;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        InitialPosition = new Vector2(transform.position.x, transform.position.y);
        ballResetTimer = 0;

        PushLineActive = false;
        PullLineActive = false;
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
        float DistanceFromCenter = transform.position.magnitude;
        if(DistanceFromCenter > 8f)
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
        OnReset?.Invoke();
        yield return new WaitForSeconds(0.5f);
        //BallInitialMove();
    }
    
    public void Pulled(Transform source, float force, float dampingFactor)
    {
        //Vector2 direction = (source.position - transform.position).normalized;
        Vector2 direction = -source.position.normalized;

        rb.AddForce(direction * force, ForceMode2D.Force);
        DampenPerpendicularSpeed(direction, dampingFactor);

        PullLineActive = true;
    }

    public void StopPulling()
    {
        PullLineActive = false;
    }

    public void Pushed(Transform source, float force, float dampingFactor)
    {
        //Vector2 direction = (source.position - transform.position).normalized;
        Vector2 direction = source.position.normalized;
        Debug.Log("Direction vector = " + direction);   

        rb.AddForce(direction * force, ForceMode2D.Force);
        DampenPerpendicularSpeed(direction, dampingFactor);

        PushLineActive = true;
    }

    public void StopPushing()
    {
        PushLineActive = false;
    }

    public void DampenPerpendicularSpeed(Vector2 direction, float dampingFactor)
    {
        // Project velocity onto the reference vector (parallel component)
        Vector2 parallelComponent = Vector2.Dot(rb.linearVelocity, direction) * direction;

        // Get the perpendicular component
        Vector2 perpendicularComponent = rb.linearVelocity - parallelComponent;

        // Apply damping to the perpendicular component
        perpendicularComponent *= dampingFactor;

        // Set the new velocity
        rb.linearVelocity = parallelComponent + perpendicularComponent;
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
        Vector2 lookDir = GameManager.Instance.Player.transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        magnetLine.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        if (PushLineActive)
        {
            PushLine.SetActive(true);
        }
        else
        {
            PushLine.SetActive(false);
        }

        if (PullLineActive)
        {
            PullLine.SetActive(true);
        }
        else
        {
            PullLine.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pit")
        {
            Debug.Log("Ball hit a pit!");
            StartCoroutine(BallReset());
        }
    }
}

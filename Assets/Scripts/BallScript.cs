using System;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private Rigidbody2D rb;

    public event Action<BallScript> ballDropped;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
}

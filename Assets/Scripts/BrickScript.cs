using System;
using UnityEngine;


public class BrickScript : MonoBehaviour
{

    public event Action<BrickScript> BallHit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BallScript ball = collision.gameObject.GetComponent<BallScript>();
        if (ball != null)
        {
            OnBallHit();
        }
    }

    public void OnBallHit()
    {
        Debug.Log("OnBallHit() called!");
        BallHit?.Invoke(this);
    }
}

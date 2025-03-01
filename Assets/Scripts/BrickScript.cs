using System;
using UnityEngine;


public class BrickScript : MonoBehaviour
{

    public event Action<BrickScript> BallHit;
    public event Action<BrickScript> BrickDestroyed;
    
    public float BrickHealth;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        BrickHealth = 10f;
    }

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
        BrickHealth = Mathf.Clamp(BrickHealth - 5f, 0, 10f);
        if(BrickHealth <= 0)
        {
            animator.Play("BrickDestroyedAnimation");
        }
        else
        {
            animator.Play("BrickHitAnimation");
            BallHit?.Invoke(this);
        }
    }

    public void DestroyBrick()
    {
        Debug.Log("DestroyBrick() called!");
        BrickDestroyed?.Invoke(this);
    }

}

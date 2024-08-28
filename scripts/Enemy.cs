using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 1;
    public int currentHealth;
    public Rigidbody2D rigidbody2d;
    public Animator animator;
    [SerializeField] public SimpleFlash flashEffect;
    void Start()
    {
        currentHealth = maxHealth;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public virtual void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if(amount < 0){
            flashEffect.Flash();
        }
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        SwordsmanController player = other.gameObject.GetComponent<SwordsmanController>();
        if (player != null)
        {
            Rigidbody2D playerBody = other.gameObject.GetComponent<Rigidbody2D>();
            Vector2 playerFromEnemy = playerBody.position - rigidbody2d.position;
            if(!player.isInvincible)
            {
                player.ChangeHealth(-1);
                player.ResetVelocity();
                playerBody.AddForce(playerFromEnemy.normalized * 15, ForceMode2D.Impulse);
            }
        }
    }
}

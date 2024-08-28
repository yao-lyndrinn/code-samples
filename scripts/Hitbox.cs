using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // Start is called before the first frame update

    void OnTriggerEnter2D(Collider2D other)
    {
        SwordsmanController player = other.gameObject.GetComponent<SwordsmanController>();
        if (player != null)
        {
            Rigidbody2D playerBody = other.gameObject.GetComponent<Rigidbody2D>();
            Vector2 position = transform.position;
            Vector2 playerFromEnemy = playerBody.position - position;
            if(!player.isInvincible)
            {
                player.ChangeHealth(-1);
                playerBody.AddForce(playerFromEnemy.normalized * 10, ForceMode2D.Impulse);
            }
        }
    }
}

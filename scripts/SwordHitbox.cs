using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy hitEnemy = other.gameObject.GetComponent<Enemy>();
        SwordsmanController player;
        Rigidbody2D rb;
        if (hitEnemy != null)
        {
            hitEnemy.ChangeHealth(-1);
            player = transform.parent.gameObject.GetComponent<SwordsmanController>();
            rb = player.GetComponent<Rigidbody2D>();
            Vector2 position = player.transform.position;
            Vector2 enemyPosition = hitEnemy.transform.position;
            Vector2 playerFromEnemy = position - enemyPosition;
            Vector2 force = new Vector2(playerFromEnemy.x, 0).normalized;
            rb.AddForce(force * 15.0f, ForceMode2D.Impulse);
            player.ChangeMana(5);
        }
    }
    /*public void enableCollider(){
        swordCollider = GetComponent<Collider2D>();
        swordCollider.enabled = true;
    }*/
    /*void DisableCollider(){
        swordCollider.enabled = false;
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy : Enemy
{
    bool tracking = false;
    Rigidbody2D playerBody;
    Vector2 playerFromEnemy;
    SwordsmanController player;
    
    void Update()
    {
        if (tracking)
        {
        }
        else
        {
            Collider2D result = Physics2D.OverlapCircle(rigidbody2d.position, 7.5f, 8);
            if (result != null)
            {
                player = result.gameObject.GetComponent<SwordsmanController>();
                if (player != null)
                {
                    playerBody = result.gameObject.GetComponent<Rigidbody2D>();
                    tracking = true;
                }
            }
        }
    }
    void FixedUpdate()
    {
        playerFromEnemy = playerBody.position - rigidbody2d.position;
        rigidbody2d.AddForce(playerFromEnemy.normalized * 2.0f);
    }
}

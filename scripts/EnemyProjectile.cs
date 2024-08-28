using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float timeFlying = 10.0f;

    float flightTimer;
    bool reflected = false;

    Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        flightTimer = timeFlying;
    }

    void Update()
    {
        flightTimer -= Time.deltaTime;
        if (flightTimer<0){
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(reflected == true){
            Enemy e = other.collider.GetComponent<Enemy>();
            if (e != null)
            {
                e.ChangeHealth(-1);
            }
        }
        else{
            SwordsmanController p = other.collider.GetComponent<SwordsmanController>();
            if (p != null)
            {
                p.ChangeHealth(-1);
                Rigidbody2D playerBody = other.gameObject.GetComponent<Rigidbody2D>();
                Vector2 playerFromEnemy = playerBody.position - rigidbody2d.position;
                playerBody.AddForce(playerFromEnemy.normalized * 10, ForceMode2D.Impulse);
            }
        }
        Destroy(gameObject);
    }
    public void Reflect(){
        gameObject.layer = 3;
        reflected = true;
        flightTimer = timeFlying;
        Vector3 newScale = gameObject.transform.localScale;
        newScale.x *= -1;
        newScale.y *= -1;
        gameObject.transform.localScale = newScale;
        Vector3 newVelocity = rigidbody2d.velocity;
        newVelocity.x *= -1;
        newVelocity.y *= -1;
        rigidbody2d.velocity = newVelocity;
        //rigidbody2d.velocity.x = -rigidbody2d.velocity.x;
        //rigidbody2d.velocity.y = -rigidbody2d.velocity.y;
        //gameObject.transform.localScale.x *= -1;
        //gameObject.transform.localScale.y *= -1;
    }
}

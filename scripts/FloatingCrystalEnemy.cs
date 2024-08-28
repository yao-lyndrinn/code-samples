using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCrystalEnemy : Enemy
{
    public GameObject crystalProjectilePrefab;
    public float speed = 1.5f;
    public int direction = -1;
    public float changeTime = 2.0f;
    Rigidbody2D playerBody;
    SwordsmanController player;
    float timer = 2.0f;
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.y = position.y + Time.deltaTime * speed * direction;
        rigidbody2d.MovePosition(position);
    }
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            if(speed == 1.5f){
                speed = 0.0f;
                Shoot();
            }
            else{
                direction = -direction;
                speed = 1.5f;
            }
            timer = changeTime;
        }
    }
    void Shoot()
    {
        Collider2D result = Physics2D.OverlapCircle(rigidbody2d.position, 10.0f, 8);
            if (result != null)
            {
                player = result.gameObject.GetComponent<SwordsmanController>();
                if (player != null)
                {
                    playerBody = result.gameObject.GetComponent<Rigidbody2D>();
                    Vector2 v2 = transform.position;
                    Vector2 playerFromShoot = playerBody.position - v2;
                    GameObject projectileObject = Instantiate(crystalProjectilePrefab, transform.position, Quaternion.FromToRotation(Vector3.right, playerFromShoot));
                    EnemyProjectile projectile = projectileObject.GetComponent<EnemyProjectile>();
                    projectile.Launch(playerFromShoot.normalized, 400);
                }
            }
    }
}

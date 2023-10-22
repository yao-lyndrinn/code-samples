using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalDoorEnemy : Enemy
{
    public GameObject crystalProjectilePrefab;
    public float changeTime = 3.0f;
    Rigidbody2D playerBody;
    SwordsmanController player;
    float timer = 3.0f;
    bool countered = false;
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            Shoot();
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
                    GameObject projectileObject = Instantiate(crystalProjectilePrefab, transform.position, Quaternion.identity);
                    EnemyProjectile projectile = projectileObject.GetComponent<EnemyProjectile>();
                    projectile.Launch(Vector2.left, 400);
                }
            }
    }
    public override void ChangeHealth(int amount){
        if(countered){
            currentHealth += amount;
            if(amount < 0){
                flashEffect.Flash();
            }
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
        else{
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyProjectile proj = other.gameObject.GetComponent<EnemyProjectile>();
        if (proj != null)
        {
            countered = true;
            ChangeHealth(-1);
        }
        else{
            countered = false;
        }
    }
    
}

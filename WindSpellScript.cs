using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpellScript : MonoBehaviour
{
    void End()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyProjectile projectile = other.GetComponent<EnemyProjectile>();
        if (projectile != null)
        {
            projectile.Reflect();
        }
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null){
            enemy.ChangeHealth(-1);
        }
    }
}

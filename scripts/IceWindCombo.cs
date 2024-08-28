using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWindCombo : MonoBehaviour
{
    void End()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null){
            enemy.ChangeHealth(-2);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpellScript : MonoBehaviour
{
    void End()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        WaterTile water = other.GetComponent<WaterTile>();
        if (water != null)
        {
            Debug.Log("FREEZE!");
            water.Freeze();
        }
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null){
            enemy.ChangeHealth(-1);
        }
    }
}

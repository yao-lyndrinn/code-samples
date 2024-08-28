using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float timeFlying = 5.0f;

    float flightTimer;

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
        Enemy e = other.collider.GetComponent<Enemy>();
        if (e != null)
        {
            e.ChangeHealth(-1);
        }
        Destroy(gameObject);
    }
}

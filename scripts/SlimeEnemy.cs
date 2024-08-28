using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : Enemy
{
    public float speed = 3.5f;
    public int direction = 1;
    public float changeTime = 2.0f;
    float timer = 2.0f;
    // Update is called once per frame

    //moveposition for this rigidbody is what's stopping the other things ont he rigidbody like velocity and forces
    //from working btw.
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + Time.deltaTime * speed * direction;
        rigidbody2d.MovePosition(position);
    }
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            if(speed == 3.5f){
                speed = 1.0f;
                direction = -direction;
                Vector3 newScale = gameObject.transform.localScale;
                newScale.x *= -1;
                gameObject.transform.localScale = newScale;
            }
            else{
                speed = 3.5f;
            }
            timer = changeTime;
        }
    }
}

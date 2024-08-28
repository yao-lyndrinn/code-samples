using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionEnemy : Enemy
{
    public float speed = 2.0f;
    public int direction = -1;
    public float changeTime = 3.0f;
    public bool aggressive = false;
    float timer = 3.0f;
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
            direction = -direction;
            timer = changeTime;
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x *= -1;
            gameObject.transform.localScale = newScale;
        }
        if (aggressive == true)
        {
            Vector2 castDirection = new Vector2(direction, 0);
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position, castDirection, 1.5f, LayerMask.GetMask("Player"));
            if (hit.collider != null)
            {
                SwordsmanController character = hit.collider.GetComponent<SwordsmanController>();
                if (character != null)
                {
                    speed = 0f;
                    Invoke("Attack", 1.0f);
                    timer += 1f;
                    aggressive = false;
                }
            }
        }
    }
    void Attack()
    {
        speed = 3.0f;
        animator.SetTrigger("Attack");
        aggressive = true;
    }
    void AttackMove()
    {
        transform.Translate(Vector3.right * direction * 0.75f);
        //rigidbody2d.velocity = new Vector3(100 * direction, 0, 0);
    }
}

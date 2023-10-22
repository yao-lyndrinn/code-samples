using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zeya : Enemy
{
    private Coroutine attackRoutine;
    public GameObject lightningPrefab;
    Rigidbody2D playerBody;
    Vector2 playerFromEnemy;
    SwordsmanController player;
    float timer = 3.0f;
    public Transform castPoint;
    public Transform castPoint2;
    public Transform castPoint3;
    public Transform castPoint4;
    public Transform castPoint5;
    public Transform castPoint6;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            //either do a rushing attack or shoot fire every 3 seconds
            timer -= Time.deltaTime;
            if (playerBody == null){
            Collider2D result = Physics2D.OverlapCircle(rigidbody2d.position, 30.0f, 8);
            if (result != null)
            {
                player = result.gameObject.GetComponent<SwordsmanController>();
                if (player != null)
                {
                    playerBody = result.gameObject.GetComponent<Rigidbody2D>();
                }
            }
            }
            playerFromEnemy = playerBody.position - rigidbody2d.position;
            if (playerFromEnemy.x < 0){
                Vector3 newScale = gameObject.transform.localScale;
                newScale.x = 1;
                gameObject.transform.localScale = newScale;
            }
            else{
                Vector3 newScale = gameObject.transform.localScale;
                newScale.x = -1;
                gameObject.transform.localScale = newScale;
            }
            if(timer < 0){
                if (playerFromEnemy.magnitude > 15.0f){
                    rigidbody2d.AddForce(new Vector2(playerFromEnemy.x, 0).normalized * 10.0f, ForceMode2D.Impulse);
                    timer = 1.0f;
                }
            }
            if (timer < 0){
                int rand = Random.Range(0, 3);
                Debug.Log(rand.ToString());
                if (rand == 1){
                    Cast();
                }
                else if (rand == 2){
                    Attack();
                }
                else{
                    AttackJump();
                }
                timer = 3.0f;
            }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        rigidbody2d.AddForce(new Vector2(playerFromEnemy.x, 0).normalized * 20, ForceMode2D.Impulse);
    }

    public void AttackJump(){
        animator.SetTrigger("Attack");
        rigidbody2d.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        rigidbody2d.AddForce(new Vector2(playerFromEnemy.x, 0).normalized * 20, ForceMode2D.Impulse);
    }
    public void Cast()
    {
        //fire animation, shoot fireball from mouth toward playerlocation
        animator.SetTrigger("Cast");
        GameObject lightningObject = Instantiate(lightningPrefab, castPoint.position, Quaternion.identity);
        GameObject lightningObject2 = Instantiate(lightningPrefab, castPoint2.position, Quaternion.identity);
        GameObject lightningObject3 = Instantiate(lightningPrefab, castPoint3.position, Quaternion.identity);
        GameObject lightningObject4 = Instantiate(lightningPrefab, castPoint4.position, Quaternion.identity);
        GameObject lightningObject5 = Instantiate(lightningPrefab, castPoint5.position, Quaternion.identity);
        GameObject lightningObject6 = Instantiate(lightningPrefab, castPoint6.position, Quaternion.identity);
    }

    public override void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if(amount < 0){
            flashEffect.Flash();
        }
        if (currentHealth <= 0)
        {
            GlobalVars.clearedZeya = true;
            //Debug.Log("True");
            Destroy(gameObject);
        }
    }
}

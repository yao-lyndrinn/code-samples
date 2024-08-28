using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingedBeast : Enemy
{
    private Coroutine attackRoutine;
    public GameObject fireballPrefab;
    public bool attacking = false;
    Rigidbody2D playerBody;
    Vector2 playerFromEnemy;
    SwordsmanController player;
    float timer = 3.0f;
    public Transform shootPoint;
    private Vector2 lastPosition;
    // Start is called before the first frame update
    void Awake()
    {
        lastPosition = transform.position;
        InvokeRepeating("Unstick", 3, 3.0f);
        if(GlobalVars.clearedBeast){
            Destroy(gameObject);
        }
    }

    void FixedUpdate(){
        playerFromEnemy = playerBody.position - rigidbody2d.position;
        if (playerFromEnemy.magnitude > 7.5f)
        {
                rigidbody2d.AddForce(playerFromEnemy.normalized);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(attacking)
        {
            rigidbody2d.AddForce(playerFromEnemy.normalized * 5.0f);
        }
        else
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
            if (playerFromEnemy.x < 0){
                Vector3 newScale = gameObject.transform.localScale;
                newScale.x = 10;
                gameObject.transform.localScale = newScale;
            }
            else{
                Vector3 newScale = gameObject.transform.localScale;
                newScale.x = -10;
                gameObject.transform.localScale = newScale;
            }
            if (timer < 0){
                int rand = Random.Range(0, 2);
                //Debug.Log(rand.ToString());
                if (rand == 1){
                    Shoot();
                }
                else{
                    Attack();
                }
                timer = 3.0f;
            }
            
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        if (attackRoutine != null)
            {
                // In this case, we should stop it first.
                // Multiple FlashRoutines the same time would cause bugs.
                StopCoroutine(attackRoutine);
            }

            // Start the Coroutine, and store the reference for it.
            attackRoutine = StartCoroutine(AttackRoutine());
    }

    public void Shoot()
    {
        //fire animation, shoot fireball from mouth toward playerlocation
        animator.SetTrigger("Shoot");
        Vector2 v2 = shootPoint.position;
        Vector2 playerFromShoot = playerBody.position - v2;
        GameObject projectileObject = Instantiate(fireballPrefab, shootPoint.position, Quaternion.FromToRotation(Vector3.right, playerFromEnemy));
        EnemyProjectile projectile = projectileObject.GetComponent<EnemyProjectile>();
        projectile.Launch(playerFromShoot.normalized, 350);
    }

    private IEnumerator AttackRoutine()
    {
        playerFromEnemy = playerBody.position - rigidbody2d.position;
        attacking = true;
        //Debug.Log("Attacking");
        yield return new WaitForSeconds(3.0f);
        attacking = false;
        attackRoutine = null;
    }
    public override void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if(amount < 0){
            flashEffect.Flash();
        }
        if (currentHealth <= 0)
        {
            GlobalVars.clearedBeast = true;
            //Debug.Log("True");
            Destroy(gameObject);
        }
    }

    private void Unstick(){
        if(rigidbody2d.position == lastPosition){
            rigidbody2d.position += playerFromEnemy.normalized;
        }
        lastPosition = transform.position;
    }
}

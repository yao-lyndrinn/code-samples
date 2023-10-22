using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwordsmanController : MonoBehaviour
{
    public static bool wind;
    public static bool ice;
    public static bool earth;
    public static bool fire;
    Rigidbody2D rigidbody2d;
    public int maxHealth = 5;
    int currentHealth;
    public int maxMana = 50;
    int currentMana;
    public bool isInvincible;
    float invincibleTimer;
    public float timeInvincible = 1.5f;
    float horizontal;
    float vertical;
    public float accel = 10;
    public float decel = 20;
    public float moveSpeed = 6.0f;
    Animator animator;
    // Start is called before the first frame update
    float lookDirection = 1.0f;
    public float maxRunSpeed = 50.0f;
    public float jumpPower = 15.0f;
    public float standingContactDistance = 0.1f;
    Collider2D playerCollider;
    public float attackRange = 0.8f;
    public Transform attackPoint;
    public Transform castPointUp;
    public Transform castPointDiagUp;
    public Transform castPointDiagDown;
    public Transform castPointDown;
    public GameObject iciclePrefab;
    public GameObject dashPrefab;
    public GameObject windSpellPrefab;
    public GameObject iceSpellPrefab;
    public GameObject iceWindComboPrefab;
    public GameObject powerMenu;
    public LayerMask enemyLayers;
    public int attackDamage = 1;
    public float attackRate = 1.5f;
    float nextAttackTime = 0f;
    float nextDashTime = 0f;
    public float dashRate = 1f;
    int dashes = 1;
    bool jump = false;
    bool jumpCut = false;
    bool wallJump = false;
    public bool obtainedDash = false;
    public bool obtainedCast = false;
    public bool obtainedIceSpell = false;
    public bool obtainedWindSpell = false;
    public bool obtainedPickaxe = false;
    private Coroutine accelRoutine;
    [SerializeField] private SimpleFlash flashEffect;
    public static int currentElement; //0 wind, 1 ice, 2 earth, 3 fire
    public int lastSpellUsed; //4 is combos, set to 5 to prevent one
    public static bool[] elements;
    void Start()
    {
        gameObject.SetActive(true);
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        animator.SetFloat("Look X", 1.0f);
        currentHealth = maxHealth;
        currentMana = 50;
        InvokeRepeating("RegenMana", 0, 1.0f);
        wind = false;
        ice = false;
        earth = false;
        fire = false;
        currentElement = 0;
        lastSpellUsed = 4;
        elements = new bool[4]{wind, ice, earth, fire};
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        //change element
        if (Input.GetKeyDown(KeyCode.A)){
            if(!powerMenu.activeSelf){
                for (int i = 0; i < 4; i++){
                if (currentElement != i && elements[i] == true){
                    currentElement = i;
                    //Debug.Log(currentElement);
                    break;
                    }
                }
            }
        }
        //set elements
        if (Input.GetKeyDown(KeyCode.Q)){
            if(powerMenu.activeSelf){
                Time.timeScale = 1.0f;
                powerMenu.SetActive(false);
                UpdateElements();
                for (int i = 0; i < 4; i++){
                if (elements[i] == true){//set current element to first true one
                    currentElement = i;
                    //Debug.Log(currentElement);
                    break;
                    }
                }
            }
            else{
                Time.timeScale = 0f;
                powerMenu.SetActive(true);
            }
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (IsOnTopOfCollider() && Mathf.Abs(rigidbody2d.velocity.y) < 0.01f)
            {
                    jump = true;
            }
            else if(obtainedPickaxe && IsNextToWall()){
                wallJump = true;
            }
        }
        //jumpcut
        if (Input.GetKeyUp(KeyCode.X)){
            if(rigidbody2d.velocity.y > 0 && jump == false){
                jumpCut = true;
            }
        }
        
        if(!Mathf.Approximately(horizontal, 0.0f))
        {
            if (horizontal > 0){
                lookDirection = 1.0f;
            }
            
            else if (horizontal < 0){
                lookDirection = -1.0f;
            }
        }
        animator.SetFloat("Look X", lookDirection);
        //animator.SetFloat("Y", vertical);
        //animator.SetFloat("X", horizontal);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }


        if(Time.time >= nextAttackTime)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                Slash();
                nextAttackTime = Time.time + 1f / attackRate;
            }
            if(Input.GetKeyDown(KeyCode.C))
            {
                if (obtainedCast)
                {
                    if (currentMana >= 10 && (currentElement == 1))
                    {
                        Cast();
                        currentMana -= 10;
                        nextAttackTime = Time.time + 1f / attackRate;
                    }
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Space)){
                if (currentMana >= 10){
                    CastSpell();
                    //Debug.Log("hey");
                }
            }
        if(Time.time >= nextDashTime && dashes > 0)
        {
            if (obtainedDash)
            {
                if(Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        if(currentElement == 0){
                            Dash();
                            dashes--;
                            nextDashTime = Time.time + 1f / dashRate;
                        }
                    }
            }
        }
        if (dashes < 1)
        {
            if (IsOnTopOfCollider() && Mathf.Abs(rigidbody2d.velocity.y) < 0.1f)
            {
                dashes = 1;
            }
        }
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        UIManaBar.instance.SetValue(currentMana / (float)maxMana);
        UIHealthBar.instance.SetValue(currentHealth/(float)maxHealth);
        
    }

    void FixedUpdate()
    {
        //Debug.Log(rigidbody2d.velocity.x);
        float xInput = horizontal;

        // Left and Right
        float targetSpeed = xInput * moveSpeed;
        float speedDif = targetSpeed - rigidbody2d.velocity.x;
        float yForce = 0;
        float accelRate = (Mathf.Abs(targetSpeed)>0.01f) ? accel : decel;
        float xForce =  Mathf.Abs(speedDif) * accelRate * Mathf.Sign(speedDif);

        if(jump)
        {
            jump = false;
            yForce = jumpPower;
            rigidbody2d.AddForce(yForce * Vector2.up, ForceMode2D.Impulse);
        }
        if(wallJump)
        {
            wallJump = false;
            if(accelRoutine != null){
                StopCoroutine(accelRoutine);
            }
            accelRoutine = StartCoroutine(ResetAccels(0.3f));
            ResetVelocity();
            Vector2 directionWallJump = Vector2.left * lookDirection;
            directionWallJump = directionWallJump + Vector2.up;
            directionWallJump = directionWallJump.normalized;
            rigidbody2d.AddForce(jumpPower * directionWallJump * 2, ForceMode2D.Impulse);
            lookDirection *= -1;
        }
        //jumpcut
        if(jumpCut){
            jumpCut = false;
            rigidbody2d.AddForce(Vector2.down * rigidbody2d.velocity.y * 0.5f, ForceMode2D.Impulse);
        }
        //fallgravity
        if(accelRoutine == null){
            if(jump == false && rigidbody2d.velocity.y < 0){
            rigidbody2d.gravityScale = 3;
        }
        else{
            rigidbody2d.gravityScale = 2;
        }
        }
        //running
        rigidbody2d.AddForce(xForce * Vector2.right);
        //friction, gets modified during dashes
        if(accelRoutine != null){
            float amountX = rigidbody2d.velocity.x * -0.05f;
            float amountY = rigidbody2d.velocity.y * -0.075f;
            Vector2 dashingFriction = new Vector2(amountX, amountY);
            rigidbody2d.AddForce(dashingFriction, ForceMode2D.Impulse);
        }
        else{
            if (Mathf.Approximately(horizontal, 0)){
            float amount = Mathf.Min(Mathf.Abs(rigidbody2d.velocity.x), 0.4f);
            amount *= Mathf.Sign(rigidbody2d.velocity.x);
            rigidbody2d.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        if (rigidbody2d.velocity.y > 12.5f){
            rigidbody2d.AddForce(Vector2.down * 1.0f, ForceMode2D.Impulse);
        }
        }
       // Debug.Log(rigidbody2d.velocity.x);
        CapVelocity();
    }

    public void CapVelocity()
    {
        float cappedXVelocity = Mathf.Min(Mathf.Abs(rigidbody2d.velocity.x), maxRunSpeed) * Mathf.Sign(rigidbody2d.velocity.x);
        float cappedYVelocity = Mathf.Min(Mathf.Abs(rigidbody2d.velocity.y), 25f) * Mathf.Sign(rigidbody2d.velocity.y);

        rigidbody2d.velocity = new Vector3(cappedXVelocity, cappedYVelocity);
    }
    void Slash()
    {
        animator.SetTrigger("Slash");
        /*Collider2D[] hitEnemies;
        if(lookDirection > 0)
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        }
        else
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position + Vector3.left * 1.2f, attackRange, enemyLayers);
        }
        foreach(Collider2D hit in hitEnemies){
            rigidbody2d.AddForce(Vector2.left * 15.0f * lookDirection, ForceMode2D.Impulse);

            Enemy hitEnemy = hit.gameObject.GetComponent<Enemy>();
            if(hitEnemy != null)
            {
            hitEnemy.ChangeHealth(-1);
            currentMana += 5;
            }
        }*/
    }
    void Cast()
    {
        animator.SetTrigger("Cast");
        if(lookDirection > 0)
        {
            if(!Mathf.Approximately(vertical, 0.0f))
            {
                if(vertical > 0)
                {
                    //cast up
                    if(!Input.GetKey(KeyCode.RightArrow))
                    {
                        GameObject projectileObject = Instantiate(iciclePrefab, castPointUp.position, castPointUp.rotation);
                        Projectile projectile = projectileObject.GetComponent<Projectile>();
                        projectile.Launch(Vector2.up, 300);
                    }
                    else
                    //cast diag up
                    {
                        GameObject projectileObject = Instantiate(iciclePrefab, castPointDiagUp.position, castPointDiagUp.rotation);
                        Projectile projectile = projectileObject.GetComponent<Projectile>();
                        projectile.Launch((Vector2.up + Vector2.right).normalized, 300);
                    }
                }
                else
                {
                    //cast down
                    if(!Input.GetKey(KeyCode.RightArrow))
                    {
                        GameObject projectileObject = Instantiate(iciclePrefab, castPointDown.position, castPointDown.rotation);
                        Projectile projectile = projectileObject.GetComponent<Projectile>();
                        projectile.Launch(Vector2.down, 300);
                    }
                    else
                    //cast diag down
                    {
                        GameObject projectileObject = Instantiate(iciclePrefab, castPointDiagDown.position, castPointDiagDown.rotation);
                        Projectile projectile = projectileObject.GetComponent<Projectile>();
                        projectile.Launch((Vector2.down + Vector2.right).normalized, 300);
                    }
                }
            }
            else//cast forward
            {
                GameObject projectileObject = Instantiate(iciclePrefab, attackPoint.position, Quaternion.identity);
                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.Launch(Vector2.right, 300);
            }
        }
        else//left casts
        {
            if(!Mathf.Approximately(vertical, 0.0f))
            {
                if(vertical > 0)
                {
                    //cast up
                    if(!Input.GetKey(KeyCode.LeftArrow))
                    {
                        GameObject projectileObject = Instantiate(iciclePrefab, castPointUp.position, Quaternion.Euler(0, 180, 90));
                        Projectile projectile = projectileObject.GetComponent<Projectile>();
                        projectile.Launch(Vector2.up, 300);
                    }
                    else
                    //cast diag up
                    {
                        GameObject projectileObject = Instantiate(iciclePrefab, castPointDiagUp.position + Vector3.left * 0.8f, Quaternion.Euler(0, 180, 45));
                        Projectile projectile = projectileObject.GetComponent<Projectile>();
                        projectile.Launch((Vector2.up + Vector2.left).normalized, 300);
                    }
                }
                else
                {
                    //cast down
                    if(!Input.GetKey(KeyCode.LeftArrow))
                    {
                        GameObject projectileObject = Instantiate(iciclePrefab, castPointDown.position, Quaternion.Euler(0, 180, -90));
                        Projectile projectile = projectileObject.GetComponent<Projectile>();
                        projectile.Launch(Vector2.down, 300);
                    }
                    else
                    //cast diag down
                    {
                        GameObject projectileObject = Instantiate(iciclePrefab, castPointDiagDown.position + Vector3.left * 0.8f, Quaternion.Euler(0, 180, -45));
                        Projectile projectile = projectileObject.GetComponent<Projectile>();
                        projectile.Launch((Vector2.down + Vector2.left).normalized, 300);
                    }
                }
            }
            else//cast forward
            {
                GameObject projectileObject = Instantiate(iciclePrefab, rigidbody2d.position + Vector2.left * 0.6f + Vector2.up * 0.1f, Quaternion.Euler(0, 180, 0));
                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.Launch(Vector2.left, 300);
            }
        }
        
    }
    void Dash()
    {

        rigidbody2d.gravityScale = 0;
        if(accelRoutine != null){
            StopCoroutine(accelRoutine);
        }
        accelRoutine = StartCoroutine(ResetAccels(0.3f));
        if(lookDirection > 0)
        {
            if(!Mathf.Approximately(vertical, 0.0f))
            {
                if(vertical > 0)
                {
                    //dash up
                    if(!Input.GetKey(KeyCode.RightArrow))
                    {
                        GameObject dashEffect = Instantiate(dashPrefab, rigidbody2d.position + Vector2.down, Quaternion.Euler(0,0,90));
                        rigidbody2d.velocity = new Vector3(0, 20, 0);
                    }
                    else
                    //dash diag up
                    {
                        GameObject dashEffect = Instantiate(dashPrefab, rigidbody2d.position + Vector2.down * 0.75f + Vector2.left * 0.75f, Quaternion.Euler(0,0,45));
                        rigidbody2d.velocity = new Vector3(Mathf.Sqrt(200f), Mathf.Sqrt(200f), 0);
                    }
                }
                else
                {
                    //dash down
                    if(!Input.GetKey(KeyCode.RightArrow))
                    {
                        GameObject dashEffect = Instantiate(dashPrefab, rigidbody2d.position + Vector2.up, Quaternion.Euler(0,0,-90));
                        rigidbody2d.velocity = new Vector3(0, -20, 0);
                    }
                    else
                    //dash diag down
                    {
                        GameObject dashEffect = Instantiate(dashPrefab, rigidbody2d.position + Vector2.up * 0.75f + Vector2.left * 0.75f, Quaternion.Euler(0,0,-45));
                        rigidbody2d.velocity = new Vector3(Mathf.Sqrt(200), -Mathf.Sqrt(200), 0);
                    }
                }
            }
            else//dash forward
            {
                GameObject dashEffect = Instantiate(dashPrefab, rigidbody2d.position + Vector2.left, Quaternion.identity);
                rigidbody2d.velocity = Vector3.right * 20f; 
            }
        }
        else//left dashes
        {

            if(!Mathf.Approximately(vertical, 0.0f))
            {
                if(vertical > 0)
                {
                    //dash up
                    if(!Input.GetKey(KeyCode.LeftArrow))
                    {
                        GameObject dashEffect = Instantiate(dashPrefab, rigidbody2d.position + Vector2.down, Quaternion.Euler(0,0,90));
                        rigidbody2d.velocity = new Vector3(0, 20, 0);
                    }
                    else
                    //dash diag up
                    {
                        GameObject dashEffect = Instantiate(dashPrefab, rigidbody2d.position + Vector2.down * 0.75f + Vector2.right * 0.75f, Quaternion.Euler(0,0,135));
                        rigidbody2d.velocity = new Vector3(-Mathf.Sqrt(200f), Mathf.Sqrt(200f), 0);
                    }
                }
                else
                {
                    //dash down
                    if(!Input.GetKey(KeyCode.LeftArrow))
                    {
                        GameObject dashEffect = Instantiate(dashPrefab, rigidbody2d.position + Vector2.up, Quaternion.Euler(0,0,-90));
                        rigidbody2d.velocity = new Vector3(0, -20, 0);
                    }
                    else
                    //dash diag down
                    {
                        GameObject dashEffect = Instantiate(dashPrefab, rigidbody2d.position + Vector2.up * 0.75f + Vector2.right * 0.75f, Quaternion.Euler(0,0,-135));
                        rigidbody2d.velocity = new Vector3(-Mathf.Sqrt(200f), -Mathf.Sqrt(200f), 0);
                    }
                }
            }
            else//dash forward
            {
                GameObject dashEffect = Instantiate(dashPrefab, rigidbody2d.position + Vector2.right, Quaternion.Euler(0,0,180));
                rigidbody2d.velocity = Vector3.left * 20f; 
            }
        }
    }

    void CastSpell(){
        if(Time.time >= nextAttackTime){     
            animator.SetTrigger("Cast");  
            if (currentElement == 0 && obtainedWindSpell){
                GameObject windSpell = Instantiate(windSpellPrefab, this.transform);
            }
            if (currentElement == 1 && obtainedIceSpell){
                GameObject iceSpell = Instantiate(iceSpellPrefab, this.transform);
            }
            currentMana -= 10;
            nextAttackTime = Time.time + 1.5f / attackRate;
            lastSpellUsed = currentElement;
        }
        else{
            if(lastSpellUsed == 1 && currentElement == 0){
                animator.SetTrigger("Cast");  
                if(obtainedWindSpell && obtainedIceSpell){
                    GameObject iceWindCombo = Instantiate(iceWindComboPrefab, this.transform);
                    currentMana -= 10;
                    nextAttackTime = Time.time + 1.5f / attackRate;
                    lastSpellUsed = 4;
                }
            }
            else{

            }
        }
    }
    void OnDrawGizmosSelected(){
        if (attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public bool IsOnTopOfCollider()
    {
        // Check colliders
        if (playerCollider)
        {
            //Debug.Log("hey");
            ContactFilter2D filter2D = new ContactFilter2D();
            filter2D.useTriggers = false;

            RaycastHit2D[] results = new RaycastHit2D[10];

            // playerCollider.OverlapCollider(filter2D, results);
            playerCollider.Cast(new Vector2(0, -1), filter2D, results, 0.2f);
            //Vector2 boxSize = new Vector2(0.8f, 0.3f);
            //Physics2D.OverlapBox(rigidbody2d.position + Vector2.down * 2.25f, boxSize, 0f, filter2D, results);

            foreach (RaycastHit2D hit in results)
            {
                // Check if character is on top of collider
                if (hit.collider && hit.collider.transform.position.z == playerCollider.transform.position.z)
                {
                    // Colliding
                    //Debug.Log(rigidbody2d.velocity.y);
                    //don't allow jumping or dash refreshing on enemy colliders
                    if(hit.collider.gameObject.layer != LayerMask.NameToLayer("Enemies"))
                    {
                        //Debug.Log("true");
                        return true;
                    }
                }
            }
        }
        //Debug.Log("false");
        return false;
    }

    public bool IsNextToWall()//for wall jumping
    {
        // Check colliders
        if (playerCollider)
        {

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * lookDirection, 0.5f, 3);


                // Check if character is on top of collider
                if (hit.collider && hit.collider.transform.position.z == playerCollider.transform.position.z)
                {
                    // Colliding
                    //Debug.Log(rigidbody2d.velocity.y);
                    //don't allow jumping or dash refreshing on enemy colliders
                    if(hit.collider.gameObject.layer != LayerMask.NameToLayer("Enemies"))
                    {
                        //Debug.Log("true");
                        return true;
                    }
                }
        }
        //Debug.Log("false");
        return false;
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            isInvincible = true;
            invincibleTimer = timeInvincible;
            flashEffect.Flash();
            accelRoutine = StartCoroutine(ResetAccels(0.2f));
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (currentHealth < 1)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("DeathScreen");
            GlobalVars.obtainedWind = false;
            GlobalVars.obtainedCast = false;
            GlobalVars.resetMana();
            GlobalVars.resetVit();
        }
    }
    public void ChangeMana(int amount){
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
    }
    void RegenMana()
    {
        currentMana = Mathf.Clamp(currentMana + 1, 0, maxMana);
    }

    public void FillMana(){
        currentMana = maxMana;
    }
    public float getLookDirection(){
        return lookDirection;
    }

    public void ResetVelocity(){
        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.totalForce = Vector2.zero;
    }
    private IEnumerator ResetAccels(float seconds){
        accel = 0;
        decel = 0;
        yield return new WaitForSeconds(seconds);
        accelRoutine = null;
        accel = 15;
        decel = 20;
        rigidbody2d.gravityScale = 2;
    }
    void UpdateElements(){
        elements[0] = wind;
        elements[1] = ice;
        elements[2] = earth;
        elements[3] = fire;
    }
    /*void EnableSword(){
        transform.GetChild(0).GetComponent<SwordHitbox>().enableCollider();
    }*/
}
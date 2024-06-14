using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using static PlayerState;

public class PlayerRb : MonoBehaviour
{
   

    [SerializeField] private float speed;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float velocity;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator AControler;

    //[SerializeField] private CFXR_Effect lg;

    //movimienot
    [SerializeField] private float rotationSpeed;
    private float turnSmoothVelocity;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private Transform cam;
    private Vector3 direction;
   // [SerializeField] private bool canMove;



    //piso
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool isGrounded;

    //salto
    [SerializeField] private float jumpForce;
    private bool justJumped;
    private bool canDoubleJump;
    private bool doubleJump;

    //move
    private bool JustMove;
    private Transform Direction;
    private Vector3 movDir;
    private float angle;

    //Temporizador
    private float timetoCheckGround;

    //weapon
    [SerializeField] private float attackDamage;
    [SerializeField]  LayerMask attackLayer;
    private int countAttack;
    private float damageAttack;
    [SerializeField] Collider weponCollider;
    [SerializeField] private float maxShield;
    //[SerializeField] private float shield=100f;
    [SerializeField] private float shieldRegeneration=1.5f;
    private float timeshield=0f;
    private float shieldRegenerationRate = 1f;

    //stamina
    [SerializeField] private float maxStamina, staminaRegeneration=2f, staminaConsumption=10;
    private float timeStamina = 0f, timeStaminaConsumption=0f;
    private float staminaConsumptionTime = 0.5f;
    [SerializeField] float specialAttackStamina = 25;

    // ESTADOS PARA MOVIMIENTOS
    [SerializeField]private bool canMove;

    [SerializeField]private playerState State;

    private float attackTimeRemaning;
    private float chargeAttack;
    [SerializeField]private float TimerCanMove=0;

    private float targeAngle;

    private bool onPlataform;

    private float currentHealth=100f;
    private float currentShield=50f;
    private float currentStamina=115f;

    private float timeAnimateHit=0.03f;
    private float timeHit=0.03f;

    // Damage
    private float timeDamage = 0;

    //eventos  
    public Action onDeath;
    public Action<float,float> onHealthchange;
    public Action<float, float> onShieldChange;
    public Action<float, float> onStaminaChange;


    
    
    // Start is called before the first frame update

    public void Awake()
    {
        /*
         HUDManager.Instance.maxStamina = maxStamina;      
         HUDManager.Instance.maxShield = maxShield;        
         HUDManager.Instance.maxHealth = maxHealth;

         HUDManager.Instance.currentShield = shield;
         HUDManager.Instance.currentStamina = stamina;
         HUDManager.Instance.currentHealth = health;
        */
        PlayerManager.Instance.PlayerCreated(this);
        GameMangarer.Instance.PlayerCreated(this);
        PlayertUiManager.Instance.PlayerCreated(this);
        maxHealth = currentHealth;
        maxShield= currentShield;
        maxStamina= currentStamina;
        

    }
    void Start()
    {

       /* onShieldChange?.Invoke(currentShield, maxShield);
        onHealthchange?.Invoke(currentHealth, maxHealth);
        onStaminaChange?.Invoke(currentStamina, maxStamina);
       */
    }


    // Update is called once per frame
    void Update()
    {

        switch (State)
        {
            case playerState.Idle:
                AControler.SetFloat("isMoving", 0);
                break;
            case playerState.Walking:
                targeAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targeAngle, ref turnSmoothVelocity, turnSmoothTime);
                movDir = Quaternion.Euler(0f, targeAngle, 0f) * Vector3.forward;
                velocity = speed;
                JustMove = true;
                AControler.SetFloat("isMoving", 0.5f);// de 0 a 1
                break;
            case playerState.Sprint:
                targeAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targeAngle, ref turnSmoothVelocity, turnSmoothTime);
                movDir = Quaternion.Euler(0f, targeAngle, 0f) * Vector3.forward;
                velocity = speed *1.5f;
                JustMove = true;
                AControler.SetFloat("isMoving", 1);
                break;
            case playerState.Jumping:
                canMove = false;
                TimerCanMove = 1f;
                //AControler.SetFloat("isMoving", 0);
                timetoCheckGround = 0.2f;
                AControler.SetBool("isGround", false);
                AControler.SetBool("isJumping", true);
                isGrounded = false;
                justJumped = true;
                canDoubleJump = true;
                break;
            case playerState.FullJumping:
                justJumped = true;
                canDoubleJump = false;
                AControler.SetBool("isJumping", false);
                AControler.SetBool("isJumpFull", true);
                break;
            case playerState.Attacking:
                canMove = false;
                TimerCanMove = 0.5f;
                AControler.SetTrigger("isAttack");
                ActDesactivateCollaider();
                damageAttack = attackDamage*1f;
                break;
            case playerState.Attacking2:
                canMove = false;
                TimerCanMove =1f;
                AControler.SetTrigger("isAttack2");
                ActDesactivateCollaider();
                damageAttack = attackDamage*1.5f;
                break;
            case playerState.SpinAttack:                
                ActDesactivateCollaider();
                AControler.SetTrigger("isSpinAttack");
                damageAttack = 5;
                break;
            case playerState.DefenceOn:
                canMove = false;        
                AControler.SetBool("isDefence", !canMove);
                break;
            case playerState.DefenceOff:        
                canMove = true;
                AControler.SetBool("isDefence", !canMove);
                break;
            case playerState.Dead:
                canMove = false;
                AControler.SetBool("isDead", true);
                break;
            case playerState.Victory: 
                break;

        }

        chekGround();
        Movement();
        CanMove();
        movementWhitoutPhysics();

    }

    private void FixedUpdate()
    {
        PhysicsMovement();
        physicsJump();

    }

    private void CanMove()
    {
        if (TimerCanMove <= 0 && isGrounded && State!=playerState.DefenceOn)
        {
            canMove = true;
        }
        AControler.SetBool("canMove", canMove);
        TimerCanMove -= Time.deltaTime;     
        attackTimeRemaning -= Time.deltaTime;
        timeDamage -= Time.deltaTime;

    }

    private void Movement()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), 0f, +Input.GetAxis("Vertical"));
        //
        if (currentHealth <= 0)
        {
            State=playerState.Dead;
        }
        else if (Input.GetButtonDown("Jump") && isGrounded)
        {
            State = playerState.Jumping;
        }
        else if (Input.GetButtonDown("Jump") && canDoubleJump)
        {
            State = playerState.FullJumping;
        }
        else if (Input.GetMouseButton(1) && isGrounded && currentShield > 0)
        {
            State = playerState.DefenceOn;
        }
        else if (Input.GetMouseButtonUp(1) && isGrounded || currentShield<=0)
        {
            State = playerState.DefenceOff;
        }
        else if (Input.GetKeyDown(KeyCode.F) && isGrounded && currentStamina - specialAttackStamina>0)
        {
            currentStamina -= specialAttackStamina;
            onStaminaChange?.Invoke(currentStamina, maxStamina);
            State = playerState.SpinAttack;
            timeDamage = 0.8f;
        }
        else if (Input.GetMouseButtonDown(0) && isGrounded)
        {
            canMove = false;
            chargeAttack = Time.time;
        }
        else if (Input.GetMouseButtonUp(0) && isGrounded && attackTimeRemaning <= 0f)
        {
            if (Time.time - chargeAttack <= 0.2)
            {
                State = playerState.Attacking;
                attackTimeRemaning = 0.5f;
                timeDamage = 0.5f;
            }
            else
            {
                State = playerState.Attacking2;
                attackTimeRemaning = 1f;
                timeDamage = 1.2f;
            }
        }
        else if (direction.magnitude >= 0.1f && canMove && Input.GetKey(KeyCode.LeftShift) && currentStamina>0)
        {
            StaminaConsumption();
            State = playerState.Sprint;
        }
        else if (direction.magnitude >= 0.1f && canMove)
        {
            State = playerState.Walking;
        }
        else if (canMove)
        {
            State = playerState.Idle;
        }
        else
            State = playerState.Waiting;

        // regeneracion escudo

        shieldReg();

        StaminaRegeneration();
        // piso
       if (isGrounded)
        {
            AControler.SetBool("isGround", true);
            AControler.SetBool("isJumping", false);
            AControler.SetBool("isJumpFull", false);
        }    
    }

    //Movimiento
    private void PhysicsMovement()
    {
        if (JustMove && !onPlataform)
        {
            _rigidbody.MovePosition(transform.position + movDir * velocity * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            JustMove = false;
        }
    }
    private void movementWhitoutPhysics()
    {
        if (JustMove && onPlataform)
        {
            //_rigidbody.MovePosition(transform.position + movDir * velocity * Time.fixedDeltaTime);
            transform.position += movDir * (speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            JustMove = false;
        }
    }

    //Salto 
    private void physicsJump()
    {
        if (justJumped && doubleJump)
        {

            _rigidbody.AddForce(Vector3.up * Mathf.Sqrt(-2f * Physics.gravity.y * jumpForce) + movDir * velocity, ForceMode.Impulse);
            justJumped = false;
            doubleJump = false;
        }
        else if (justJumped)
        {
            _rigidbody.AddForce(Vector3.up * Mathf.Sqrt(-2f * Physics.gravity.y * jumpForce) + movDir *velocity/2, ForceMode.Impulse);           
            justJumped = false;
        }

    }
    // plataforma
 
    public void OnLanding()
    {
        onPlataform = true;
    }
    public void OnLeave()
    {
        onPlataform= false;
    }

    // chekeo piso
    private void chekGround()
    {
        timetoCheckGround-=Time.deltaTime;
        if (timetoCheckGround<=0)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
    }

    //Regeneracion de escudo
    private void shieldReg()
    {       
        if (currentShield < maxShield && State != playerState.DefenceOn)
        {
            timeshield += Time.deltaTime;
            if (timeshield >= shieldRegenerationRate)
            {
                currentShield += shieldRegeneration;
                timeshield = 0f;
                onShieldChange?.Invoke(currentShield, maxShield);
            }


        }
        else if (currentShield > maxShield)
        {
           currentShield = maxShield;
           onShieldChange?.Invoke(currentShield, maxShield);
        }
    }
    


    // Regeneracion Stamina
    private void StaminaRegeneration()
    {
        if (currentStamina < maxStamina && State != playerState.Sprint)
        {
            timeStamina += Time.deltaTime;
            if (timeStamina >= staminaRegeneration)
            {
               currentStamina += maxStamina/10;
                timeStamina = 0f;
                onStaminaChange?.Invoke(currentStamina, maxStamina);
            }


        }
        else if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
            onStaminaChange?.Invoke(currentStamina, maxStamina);
        }
    }
    private void StaminaConsumption()
    {
        timeStaminaConsumption += Time.deltaTime;
        if (timeStaminaConsumption >= staminaConsumptionTime)
        {
            currentStamina -= staminaConsumption;
            timeStaminaConsumption = 0f;
            onStaminaChange?.Invoke(currentStamina, maxStamina);
        }
    }

    //
    public void onLanding()
    {
        onPlataform = true;
    }
    public void levePlataform()
    {
        onPlataform = false;
    }


    // da�o 
    private void ActDesactivateCollaider()
    {
        weponCollider.enabled = false;
        weponCollider.enabled = true;

    }

    private void OnTriggerEnter(Collider other)
        {          
            // L�gica para aplicar da�o al enemigo
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && timeDamage>0)
            {
            enemy.damage(damageAttack);
            }
            Boss boss = other.GetComponent<Boss>();
            if (boss != null && timeDamage > 0)
            {
            
            boss.damage(damageAttack);
            }

    }
    public void Damage(float damage)
    {
        timeAnimateHit -= Time.deltaTime;
        if (State == playerState.DefenceOn)
        {            
            if ((currentShield >= damage))
            {
                currentShield -= damage;
                AnimateHit("isDefenceHit");
                // AControler.SetTrigger("isDefenceHit");
                onShieldChange?.Invoke(currentShield, maxShield);
            }
            else
            {
                damage -= currentShield;
                currentShield = 0f;
                currentHealth -= damage;
                AnimateHit("isHit");
                //AControler.SetTrigger("isHit");
                onShieldChange?.Invoke(currentShield, maxShield);
                onHealthchange?.Invoke(currentHealth,maxHealth);
            }
        }
        else
        {
            currentHealth -= damage;
            AnimateHit("isHit");
            //AControler.SetTrigger("isHit");
            onHealthchange?.Invoke(currentHealth, maxHealth);
        }
        if (currentHealth <= 0)
        { 
            AControler.SetBool("isDead", true);
            onDeath?.Invoke();
        }
    }

    public void heal(float heal)
    {
        currentHealth += heal;
        onHealthchange?.Invoke(currentHealth, maxHealth);
    }

    private void AnimateHit(string hit)
    {
        timeHit += Time.deltaTime;
        if (timeAnimateHit<= timeHit)
        {
            AControler.SetTrigger(hit);
            timeHit = 0f;
        }
    }
     
}



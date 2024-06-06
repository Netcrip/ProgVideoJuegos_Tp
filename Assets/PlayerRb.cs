using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerState;

public class PlayerRb : MonoBehaviour
{


    [SerializeField] private float speed;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    private float velocity;

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
    [SerializeField] private float maxShield = 100f;
    [SerializeField]private float shield=100f;
    [SerializeField] private float shieldRegeneration=1.5f;
    private float timeshield=0f;
    private float shieldRegenerationRate = 1f;


    // ESTADOS PARA MOVIMIENTOS
    [SerializeField]private bool canMove;

    [SerializeField]private playerState State;

    private float attackTimeRemaning;
    private float chargeAttack;
    [SerializeField]private float TimerCanMove=0;

    private float targeAngle;


    // Damage
    private float timeDamage = 0;

    // Start is called before the first frame update
    void Start()
    {
        
       

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
                AControler.SetFloat("isMoving", 0.5f);
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
        if (health <= 0)
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
        else if (Input.GetMouseButton(1) && isGrounded && shield > 0)
        {
            State = playerState.DefenceOn;
        }
        else if (Input.GetMouseButtonUp(1) && isGrounded)
        {
            State = playerState.DefenceOff;
        }
        else if (Input.GetKeyDown(KeyCode.F) && isGrounded)
        {
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
        else if (direction.magnitude >= 0.1f && canMove && Input.GetKey(KeyCode.LeftShift))
        {
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
        if (JustMove)
        {
            _rigidbody.MovePosition(transform.position + movDir * velocity * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            JustMove = false;
           

        }
    }

    //Salto 
    private void physicsJump()
    {
        if (justJumped && doubleJump)
        {

            _rigidbody.AddForce(Vector3.up * Mathf.Sqrt(-2f * Physics.gravity.y * jumpForce) + movDir * 1.5f, ForceMode.Impulse);
            justJumped = false;
            doubleJump = false;
        }
        else if (justJumped)
        {
            _rigidbody.AddForce(Vector3.up * Mathf.Sqrt(-2f * Physics.gravity.y * jumpForce) + movDir * 1.3f, ForceMode.Impulse);           
            justJumped = false;
        }

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
        if (shield  < maxShield && State !=playerState.DefenceOn)
        {
            timeshield += Time.deltaTime;
            if (timeshield>=shieldRegenerationRate)
            {
                shield += shieldRegeneration;
                timeshield = 0f;
            }
            
            
        }
        else if(shield > maxShield)
        {
            shield = maxShield;
        }
    }
    
    // Regeneracion Energia


    
    // daño 
    private void ActDesactivateCollaider()
    {
        weponCollider.enabled = false;
        weponCollider.enabled = true;

    }

    private void OnTriggerEnter(Collider other)
        {          
            // Lógica para aplicar daño al enemigo
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && timeDamage>0)
            {
                enemy.damage(damageAttack);
            }
            Boss boss = other.GetComponent<Boss>();
            if (boss != null && timeDamage > 0)
            {
            Debug.Log("AtakeBoss");
            boss.damage(damageAttack);
            }

    }
    public void Damage(float damage)
    {
        if (State == playerState.DefenceOn)
        {
            if ((shield>=damage))
            {
                shield -= damage;
                AControler.SetTrigger("isDefenceHit");
            }
            else
            {
                damage -= shield;
                shield=0f;
                health -= damage;
                AControler.SetTrigger("isHit");
            }
            
        }
        else
        {
            health -= damage;
            AControler.SetTrigger("isHit");
        }
    }
     
}



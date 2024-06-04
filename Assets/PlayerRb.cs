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

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator AControler;


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
    [SerializeField] private float attackDistance;
    [SerializeField]  LayerMask attackLayer;
    private int countAttack;
    private float damageAttack;
    [SerializeField] Collider weponCollider;

    // ESTADOS PARA MOVIMIENTOS
    [SerializeField]private bool canMove;

    private playerState State;

    private float attackTimeRemaning;
    private float chargeAttack;
    private float TimerCanMove;


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
                float targeAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targeAngle, ref turnSmoothVelocity, turnSmoothTime);
                movDir = Quaternion.Euler(0f, targeAngle, 0f) * Vector3.forward;
                JustMove = true;
                AControler.SetFloat("isMoving", speed);
                break;
            case playerState.Jumping:
                canMove = false;
                TimerCanMove = 1f;
                AControler.SetFloat("isMoving", 0);
                timetoCheckGround = 0.2f;
                AControler.SetBool("isGround", false);
                AControler.SetBool("isJumping", true);
                isGrounded = false;
                justJumped = true;
                canDoubleJump = true;
                break;
            case playerState.FullJumping:
                justJumped = true;
                doubleJump = true;
                AControler.SetBool("isJumping", true);
                AControler.SetBool("isGround", false);
                AControler.SetBool("isJumpFull", true);
                canDoubleJump = false;
                break;
            case playerState.Attacking:
                canMove = false;
                TimerCanMove = 0.5f;
                AControler.SetTrigger("isAttack");
                ActDesactivateCollaider();
                damageAttack = 10;
                break;
            case playerState.Attacking2:
                canMove = false;
                TimerCanMove =2f;
                AControler.SetTrigger("isAttack2");
                ActDesactivateCollaider();
                damageAttack = 20;
                break;
            case playerState.TurningAttack:
                canMove = false;
                TimerCanMove = 0.5f;
                AControler.SetTrigger("isTurningAttack");
                ActDesactivateCollaider();
                damageAttack = 5;
                break;
            case playerState.Defence:
                canMove = !canMove;
                AControler.SetBool("isDefence", !canMove);
                break;
            case playerState.Dead:
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
        if (TimerCanMove <= 0)
        {
            canMove = true;
            
        }
        AControler.SetBool("canMove", canMove);
    }

    private void Movement()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), 0f, +Input.GetAxis("Vertical"));
        //
       
         if (Input.GetButtonDown("Jump") && isGrounded)
        {
            State = playerState.Jumping;
        }
        else if (Input.GetButtonDown("Jump") && canDoubleJump)
        {
            State = playerState.FullJumping;
        }
        else if(Input.GetMouseButtonDown(1) && isGrounded)
        {
            State = playerState.Defence;
        }
        else if (Input.GetMouseButtonUp(1) && isGrounded)
        {
            State = playerState.Defence;
        }
        else if (Input.GetKey(KeyCode.F) && isGrounded)
        {
            Debug.Log("entro");
            //canMove = false;
            State = playerState.TurningAttack;
        }
        else if (Input.GetMouseButtonDown(0) && isGrounded)
        {
            canMove = false;
            chargeAttack = Time.time;
        }
         else if (Input.GetMouseButtonUp(0) && isGrounded && attackTimeRemaning<=0f)
        {
            if (Time.time -chargeAttack <= 0.2)
            {
    
               State = playerState.Attacking;
               attackTimeRemaning = 0.5f;


            }
            else
            {
                State = playerState.Attacking2;
                attackTimeRemaning = 1f;
            }
        }
        else if (direction.magnitude >= 0.1f && canMove)
        {
            State = playerState.Walking;
        }
        else
        {
            State = playerState.Idle;
        }

        // salto
        

        // piso
       if (isGrounded)
        {
            AControler.SetBool("isGround", true);
            AControler.SetBool("isJumping", false);
            AControler.SetBool("isJumpFull", false);
            
            timmers();
        }

    }

    private void PhysicsMovement()
    {
        if (JustMove)
        {
            _rigidbody.MovePosition(transform.position + movDir * speed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            JustMove = false;
           

        }
    }
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
            Debug.Log(" fuerza: " + Mathf.Sqrt(-2f * Physics.gravity.y * jumpForce).ToString() + " pphy: " + Physics.gravity.y);

            justJumped = false;
        }

    } 
    private void chekGround()
    {
        timetoCheckGround-=Time.deltaTime;
        if (timetoCheckGround<=0)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
    }


 

    private void Attack()
    {
        

        
       
          
    }



    private void timmers()
    {
        if (attackTimeRemaning <= 0)
        {
            
            canMove = true;
           
        }
        attackTimeRemaning -= Time.deltaTime;
    }

    private void ActDesactivateCollaider()
    {
        weponCollider.enabled = false;
        weponCollider.enabled = true;

    }

    private void OnTriggerEnter(Collider other)
        {          
            // Lógica para aplicar daño al enemigo
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.damage(damageAttack);
            }
        }
       
 




}



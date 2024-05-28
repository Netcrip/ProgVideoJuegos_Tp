using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private bool canMove;



    //piso
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool isGrounded;

    //salto
    [SerializeField] private float jumpForce;
    private bool justJumped;
    private bool canDoubleJump;

    //move

    // Start is called before the first frame update
    void Start()
    {
        
       

    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), 0f, +Input.GetAxis("Vertical"));
        Attack();
        chekGround();
        Jump();
        Defense();
        


    }

    private void FixedUpdate()
    {
        PhysicsMovement();
        physicsJump();

    }

    private void PhysicsMovement()
    {
        if (direction.magnitude >= 0.1f && canMove)
        {
            float targeAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targeAngle, ref turnSmoothVelocity, turnSmoothTime);

            Vector3 movDir = Quaternion.Euler(0f, targeAngle, 0f) * Vector3.forward;

            _rigidbody.MovePosition(transform.position + movDir * speed * Time.fixedDeltaTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            AControler.SetFloat("isMoving", 1);
        }
        else
            AControler.SetFloat("isMoving", 0);
    }

    private void physicsJump()
    {
        if (justJumped)
        {
            
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            justJumped = false;
        }

    }
    private void chekGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
     

    }

    private void Jump()
    {

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            
            AControler.SetBool("isGround", false);
            AControler.SetBool("isJumping", true);
            justJumped = true;
            canDoubleJump = true;
        }
        else if (Input.GetButtonDown("Jump") && canDoubleJump)
        {
            justJumped = true;
            AControler.SetBool("isJumping", false);
            AControler.SetBool("isGround", false);
            AControler.SetBool("isDoubleJumping", true);
            canDoubleJump = false;
        }
        else if (isGrounded)
        {
            AControler.SetBool("isGround", true);
            AControler.SetBool("isJumping", false);
            AControler.SetBool("isDoubleJumping", false);
        }

    }

    private void Defense()
    {
        if (Input.GetMouseButton(1) && isGrounded)
        {
            AControler.SetBool("isDefence", true);
            canMove = false;
        }
        else if (Input.GetMouseButtonUp(1) && isGrounded)
        {
            AControler.SetBool("isDefence", false);
            canMove = true;
        }
    }

    private void Attack()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            AControler.SetTrigger("isAttackCircle");
            //canMove = false;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            AControler.SetTrigger("isAttack");
            //canMove = false;
        }
        else
            canMove = true;
    }
}



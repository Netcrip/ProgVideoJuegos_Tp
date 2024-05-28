using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private CharacterController cC;
    private float turnSmoothVelocity;
    [SerializeField] private float turnSmoothTime= 0.1f;
    [SerializeField] private Transform cam;

    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeigth = 3;

    [SerializeField] private Animator aC;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] LayerMask groundMask;
   
     //flags
    [SerializeField] private Vector3 velocity;
    [SerializeField] private bool isGrounded;

    [SerializeField] private bool doubleJump;
    
    [SerializeField] private bool isDefense;
    [SerializeField] private bool canMove = true;
     // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (velocity.y<-2f)
        {
            velocity.y = -2f;
        }

        if (isGrounded)
        {
            aC.SetBool("isJumping", false);
            aC.SetBool("isDobleJumping", false);
            aC.SetBool("isGround",true);
        }

        Jumping();
        defense();
        velocity.y += gravity * Time.deltaTime;
        cC.Move(velocity * Time.deltaTime);

    }
    private void Movimiento()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude > 0 && canMove)
        {
            float targeAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targeAngle, ref turnSmoothVelocity, turnSmoothTime);

            Vector3 movDir = Quaternion.Euler(0f, targeAngle, 0f) * Vector3.forward;

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            cC.Move(movDir.normalized * speed * Time.deltaTime);
            aC.SetFloat("isMoving", 1);
        }
        else
            aC.SetFloat("isMoving", 0);

    }

    private void Jumping()
    {

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded && canMove)
            {
                velocity.y = Mathf.Sqrt(jumpHeigth * -2 * gravity);
                doubleJump = true;
                aC.SetBool("isJumping", true);
                aC.SetBool("isGround",false);
            }
            else if(doubleJump && canMove)
            {
                aC.SetBool("isDobleJumping", true);
                velocity.y += Mathf.Sqrt(jumpHeigth * -2 * gravity);
                doubleJump = false;
            }
        }
    }

 private void defense(){


        if (Input.GetMouseButton(1)){
            aC.SetBool("isDefense", true);
            canMove = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            aC.SetBool("isDefense", false);
            canMove = true;
        }
    
 }


}

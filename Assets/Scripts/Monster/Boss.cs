using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Transform player;

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;

    //health
    [SerializeField] private float currentHealth;
    private float maxHealth;
    [SerializeField] private HealthManager health;
    [SerializeField]private bool isAlive;

    // Animator
    [SerializeField] private Animator AControler;


    //Respawn point
    [SerializeField] private Vector3 respawPoint;

    //Attacking
    [SerializeField] private float timeBetweenAttacks, timeBetweenlongAttacks;
    bool alreadyAttacked;
    [SerializeField] private GameObject rock;
    [SerializeField] private Transform rockPosition;
    private RaycastHit hit;
    [SerializeField] private float damageAttack;
    [SerializeField] private GameObject rockShow;
    private bool trhowRock = false;

    //
    [SerializeField] private bool canMove = true;
    private bool stop = false;

    //States
    [SerializeField] private float sightRange, attackRange,attackLongRange, rotationSpeed;
    [SerializeField] private bool playerInSightRange, playerInAttackRange,playerInAttackLongRange,inRespawnPosition;

    //audio
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioSource moveSfx;
    [SerializeField] private AudioClip hitSfx;
    [SerializeField] private AudioClip attackSfx;
    [SerializeField] private AudioClip attack2Sfx;
    [SerializeField] private AudioClip attack2StoneSfx;
    [SerializeField] private AudioClip dieSfx;
    private bool isDead = false;

    public Action <Boss>onDead;
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        respawPoint = transform.position;
        maxHealth = currentHealth;
        GameMangarer.Instance.boosCreate(this);
        PlayertUiManager.Instance.onPlayerDead += Stop;
    }

    private void Update()
    {
        //Comprobasion rango para seguir y atacar 
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackLongRange = Physics.CheckSphere(transform.position, attackLongRange-1f, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange-1f, whatIsPlayer);
        if(transform.position.x == respawPoint.x && transform.position.z == respawPoint.z) inRespawnPosition = true;            
        else inRespawnPosition = false;

        if ( currentHealth > 0.0f) isAlive=true; else isAlive=false;
        if (!playerInSightRange && !playerInAttackRange && !playerInAttackLongRange && inRespawnPosition && isAlive) iddle();
        else if (!playerInSightRange && !playerInAttackRange && !playerInAttackLongRange && isAlive && canMove && !stop) goToRespawn();
        if (playerInAttackLongRange && !playerInSightRange && isAlive && !stop) LongAttackPlayer();
        if (playerInSightRange && !playerInAttackRange && isAlive && canMove && !stop) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && isAlive && !stop) AttackPlayer();


    }
    private void FixedUpdate()
    {
        if (trhowRock)
        {
            RockThrower();
            trhowRock=false;
        }
    }
    private void iddle() 
    {
        AControler.SetFloat("isMoving", 0f);
        moveSfx.enabled = false; 
    }
    
    private void goToRespawn()
    {
        moveSfx.enabled = true;
        agent.SetDestination(respawPoint);
        AControler.SetFloat("isMoving", 0.5f);
    }

    private void toggleCanmove()
    {
        canMove = !canMove;
    }

    private void ChasePlayer()
    {
        moveSfx.enabled = true;
        agent.SetDestination(player.position);
        AControler.SetFloat("isMoving",0.5f);
    }

    private void LongAttackPlayer()
    {
        //fijar enemigo
        
        moveSfx.enabled = false;
        agent.SetDestination(transform.position);
        AControler.SetFloat("isMoving", 0f);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            

            Ray ray = new Ray(transform.position, transform.forward);
            if(Physics.Raycast(ray,out hit, attackLongRange))
            {
                canMove = false;
                sfx.PlayOneShot(attack2StoneSfx);
                sfx.PlayOneShot(attack2Sfx);
             AControler.SetTrigger("longAttack");

                Invoke(nameof(ShowRock), 0.6f);
                ///Calculamos la disntacia para la fuerza de la piedra
               // sfx.PlayOneShot(attack2StoneSfx);
                Invoke(nameof(RockThrowerCall),1.6f);
                Invoke(nameof(toggleCanmove), 2f);
                alreadyAttacked = true;
             Invoke(nameof(ResetAttack), timeBetweenlongAttacks);
            }
            
        }
        
    }

    //stone 
    private void ShowRock()
    {
        rockShow.SetActive(true);
        
    }
    private void RockThrowerCall()
    {
        trhowRock = true;
    }

    private void RockThrower()
    {
        
        rockShow.SetActive(false);
        Rigidbody rb = Instantiate(rock, rockPosition.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * (hit.distance / 1.6f), ForceMode.Impulse);
        rb.AddForce(transform.up * 8f, ForceMode.Impulse);
        rb.AddTorque(UnityEngine.Random.Range(0,500), UnityEngine.Random.Range(0, 500), UnityEngine.Random.Range(0, 500));
    }

    private void AttackPlayer()
    {
        
        //fijar enemigo 
        moveSfx.enabled = false;
        agent.SetDestination(transform.position);
        AControler.SetFloat("isMoving", 0f);
     
        if (!alreadyAttacked)
        {
            //mirar al jugador
            Quaternion newRotation = Quaternion.LookRotation( player.transform.position- transform.position);
            Quaternion currentRotation = transform.rotation;
            Quaternion finalRotation = Quaternion.Lerp(currentRotation, newRotation, Time.deltaTime * rotationSpeed);
            transform.rotation = finalRotation;




            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, attackRange))
            {
                canMove = false;
                sfx.PlayOneShot(attackSfx);
                AControler.SetTrigger("shortAttack");

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
                Invoke(nameof(toggleCanmove), 2f);
            }

           
        }
       
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void Stop()
    {
        stop = true;
        PlayertUiManager.Instance.onPlayerDead -= Stop;
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Lógica para aplicar daño al enemigo
        PlayerRb isplayer = other.GetComponent<PlayerRb>();
        if (isplayer != null)
        {
            isplayer.Damage(damageAttack);
        }
    }
    public void damage(float damage)
    {
        if (!isDead)
        {
            sfx.PlayOneShot(hitSfx);
            currentHealth -= damage;
            health.UpdateHealtBar(maxHealth, currentHealth);
        }
        if (currentHealth <= 0 && !isDead)
        {
            sfx.PlayOneShot(dieSfx);
            AControler.SetBool("isDead", true);
            onDead?.Invoke(this);
            isDead = true;
            Invoke(nameof(DestroyEnemy), 4f);
            
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackLongRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

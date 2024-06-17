using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField]  private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Collider attackCollider;


    [SerializeField] private float currentHealth=80;
    [SerializeField] private float maxHealth = 80;
    [SerializeField] private bool isAlive;

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;

    // Start is called before the first frame update
    [SerializeField] private HealthManager health;
    private Vector3 targetPosition;




    // Animator
    [SerializeField] private Animator AControler;

    //Patroling
    [SerializeField] private Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;


    //Attacking
    [SerializeField] private float timeBetweenAttacks;
    bool alreadyAttacked;
    private RaycastHit hit;
    [SerializeField] private float damageAttack;

    //States
    [SerializeField] private float sightRange, attackRange, rotationSpeed;
    [SerializeField] private bool playerInSightRange, playerInAttackRange;

    private bool isDead=false;

    //
    [SerializeField] private GameObject heart;

    private bool stop = false;

    //Audio
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioSource moveSfx;
    [SerializeField] private AudioClip hitSfx;
    [SerializeField] private AudioClip attackSfx;
    [SerializeField] private AudioClip dieSfx;

    //timer
    private float patroltime=0;
    private float patrolTimeMax = 3f;
    //eventos
    public Action <Enemy> onDead;

    

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        maxHealth = currentHealth;
        GameMangarer.Instance.enemyCreate(this);
        PlayertUiManager.Instance.onPlayerDead += Stop;
    }
    void Start()
    {

        health.UpdateHealtBar(maxHealth, currentHealth);

    }
  

    private void Update()
    {
        //Comprobasion rango para seguir y atacar 
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange - 1f, whatIsPlayer);

        if (currentHealth > 0.0f) isAlive = true; else isAlive = false;
        if (!playerInSightRange && !playerInAttackRange && !stop) Patroling();
        if (!playerInSightRange && !playerInAttackRange ) iddle();
        if (playerInSightRange && !playerInAttackRange && isAlive && !stop) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && isAlive && !stop) AttackPlayer();


    }
    private void FixedUpdate()
    {
        
    }
    private void Stop()
    {
        stop = true;
        PlayertUiManager.Instance.onPlayerDead -= Stop;
    }
    private void iddle() => AControler.SetFloat("isMoving", 0f);

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            patroltime += Time.deltaTime;
            agent.SetDestination(walkPoint);
            AControler.SetFloat("isMoving", 0.5f);
            moveSfx.enabled=true;
        }


        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // llegue
        if (distanceToWalkPoint.magnitude < 2f || patroltime >= patrolTimeMax)
        {
            patroltime = 0;
            AControler.SetFloat("isMoving", 0f);
            walkPointSet = false;
            moveSfx.enabled = false;
        }


    }
    private void SearchWalkPoint()
    {
        //calcular un punto en x e y dentro de la zona del mapa
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        AControler.SetFloat("isMoving", 0.5f);
    }


    private void AttackPlayer()
    {
        //Fijar Enemigo
        moveSfx.enabled = false;
        agent.SetDestination(transform.position);
        AControler.SetFloat("isMoving", 0f);

        if (!alreadyAttacked)
        {
            //mirar al jugador
            Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            Quaternion currentRotation = transform.rotation;
            Quaternion finalRotation = Quaternion.Lerp(currentRotation, newRotation, Time.deltaTime * rotationSpeed);
            transform.rotation = finalRotation;

            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, attackRange))
            {
                sfx.PlayOneShot(attackSfx);
                ActDesactivateCollaider();                
                AControler.SetTrigger("Attack");

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }


        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Lógica para aplicar daño al Player
        PlayerRb isplayer = other.GetComponent<PlayerRb>();
        if (isplayer != null && alreadyAttacked)
        {
            isplayer.Damage(damageAttack);
        }
    }

    public void damage(float damage)
    {
        if (!isDead)
        { 
        currentHealth -= damage;
        health.UpdateHealtBar(maxHealth, currentHealth);
        AControler.SetTrigger("getHit");
        sfx.PlayOneShot(hitSfx);
        } 
        if (currentHealth <= 0 && !isDead)
        {
            AControler.SetBool("isDead", true);
            sfx.PlayOneShot(dieSfx);
            Vector3 itemPosition=transform.position;
            itemPosition.y += 1;
            itemPosition.x += UnityEngine.Random.Range(0,4);
            itemPosition.z += UnityEngine.Random.Range(0, 4);

            GameObject h=Instantiate(heart, itemPosition, Quaternion.Euler(-90f,0f,0f));
            /* SphereCollider sCollider = gameObject.GetComponent<SphereCollider>();
             sCollider.enabled = false;*/
            attackCollider.enabled = false;
            onDead?.Invoke(this);
            sfx.PlayOneShot(dieSfx);
            Invoke(nameof(DestroyEnemy), 3f);
            isDead = true;
        }
    }


    private void ActDesactivateCollaider()
    {
        attackCollider.enabled = false;
        attackCollider.enabled = true;

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

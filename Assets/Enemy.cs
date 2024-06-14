using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        maxHealth = currentHealth;
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
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (!playerInSightRange && !playerInAttackRange) iddle();
        if (playerInSightRange && !playerInAttackRange && isAlive) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && isAlive) AttackPlayer();


    }
    private void FixedUpdate()
    {

    }
    private void iddle() => AControler.SetFloat("isMoving", 0f);

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            AControler.SetFloat("isMoving", 0.5f);
        }


        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // llegue
        if (distanceToWalkPoint.magnitude < 2f)
        {
            AControler.SetFloat("isMoving", 0f);
            walkPointSet = false;
        }


    }
    private void SearchWalkPoint()
    {
        //calcular un punto en x e y dentro de la zona del mapa
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

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
        currentHealth -= damage;
        health.UpdateHealtBar(maxHealth, currentHealth);
        AControler.SetTrigger("getHit");
        if (currentHealth <= 0 && !isDead)
        {
            AControler.SetBool("isDead", true);

            Vector3 itemPosition=transform.position;
            itemPosition.y += 1;
            itemPosition.x += Random.Range(0,3);
            itemPosition.z += Random.Range(0, 3);

            GameObject h=Instantiate(heart, itemPosition, Quaternion.Euler(-90f,0f,0f));

            
            Invoke(nameof(DestroyEnemy), 4f);
            isDead = true;
        }
    }

    private void dead()
    {
        if (currentHealth <= 0)
        {

            Destroy(this.gameObject, 5);
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

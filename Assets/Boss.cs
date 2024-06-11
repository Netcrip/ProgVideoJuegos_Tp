using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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

    //States
    [SerializeField] private float sightRange, attackRange,attackLongRange, rotationSpeed;
    [SerializeField] private bool playerInSightRange, playerInAttackRange,playerInAttackLongRange,inRespawnPosition;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        respawPoint = transform.position;
        maxHealth = currentHealth;
    }

    private void Update()
    {
        //Comprobasion rango para seguir y atacar 
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackLongRange = Physics.CheckSphere(transform.position, attackLongRange-1f, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange-1f, whatIsPlayer);
        if(transform.position.x == respawPoint.x && transform.position.z==respawPoint.z) inRespawnPosition= true;
        if( currentHealth > 0.0f) isAlive=true; else isAlive=false;

        if (!playerInSightRange && !playerInAttackRange && !playerInAttackLongRange && inRespawnPosition && isAlive) iddle();
        else if (!playerInSightRange && !playerInAttackRange && !playerInAttackLongRange && isAlive) goToRespawn();
        if (playerInAttackLongRange && !playerInSightRange && isAlive) LongAttackPlayer();
        if (playerInSightRange && !playerInAttackRange && isAlive) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && isAlive) AttackPlayer();


    }
    private void FixedUpdate()
    {
        if (trhowRock)
        {
            RockThrower();
            trhowRock=false;
        }
    }
    private void iddle() => AControler.SetFloat("isMoving", 0f);
    private void goToRespawn()
    {
        agent.SetDestination(respawPoint);
        AControler.SetFloat("isMoving", 0.5f);
    }



    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        AControler.SetFloat("isMoving",0.5f);
    }

    private void LongAttackPlayer()
    {
        //fijar enemigo
        agent.SetDestination(transform.position);
        AControler.SetFloat("isMoving", 0f);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            
            Ray ray = new Ray(transform.position, transform.forward);
            if(Physics.Raycast(ray,out hit, attackLongRange))
            {
             
             AControler.SetTrigger("longAttack");

                Invoke(nameof(ShowRock), 0.6f);
                ///Calculamos la disntacia para la fuerza de la piedra
                Invoke(nameof(RockThrowerCall),1.6f);               
                
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
        rb.AddForce(transform.forward * (hit.distance / 1.7f), ForceMode.Impulse);
        rb.AddForce(transform.up * 8f, ForceMode.Impulse);
        rb.AddTorque(Random.Range(0,500), Random.Range(0, 500), Random.Range(0, 500));
    }

    private void AttackPlayer()
    {
        //fijar enemigo 
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
                AControler.SetTrigger("shortAttack");

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
        // Lógica para aplicar daño al enemigo
        PlayerRb isplayer = other.GetComponent<PlayerRb>();
        if (isplayer != null)
        {
            isplayer.Damage(damageAttack);
        }
    }
    public void damage(float damage)
    {
        currentHealth -= damage;
        health.UpdateHealtBar(maxHealth, currentHealth);
        if (currentHealth <= 0)
        {
            AControler.SetBool("isDead", true);
            Invoke(nameof(DestroyEnemy), 6f);
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

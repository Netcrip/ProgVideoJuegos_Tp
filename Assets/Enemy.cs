using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]  private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private float currentHealth=80;
    [SerializeField] private float maxHealth = 80;
    // Start is called before the first frame update
    [SerializeField] private HealthManager health;
    private Vector3 targetPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }
    void Start()
    {

        health.UpdateHealtBar(maxHealth, currentHealth);

    }




    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            agent.SetDestination(player.position);
            targetPosition = new Vector3(player.position.x,this.transform.position.y,player.position.z);
            transform.LookAt(targetPosition);
        }
        dead();

       
    }
    public void damage(float damage)
    {
        currentHealth-= damage;
        health.UpdateHealtBar(maxHealth, currentHealth);
    }

        private void dead()
    {
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject, 5);
        }
    }
}

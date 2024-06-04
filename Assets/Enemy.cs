using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]  private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private float hp=80;
    // Start is called before the first frame update

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }
    void Start()
    {
        


    }




    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            agent.SetDestination(player.position);
        }
        dead();
    }
    public void damage(float damage)
    {
        hp -= damage;
    }

        private void dead()
    {
        if (hp <= 0)
        {
            Destroy(this.gameObject, 5);
        }
    }
}

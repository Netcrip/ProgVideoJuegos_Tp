using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float timeToDestroy = 2f;
    [SerializeField] private float speed;
    private Vector3 direction;
    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float damageAttack;

    // Update is called once per frame
    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        //transform.position += direction * (Time.deltaTime * speed);
        if (timeToDestroy <= 0)
        {
            //Destroy
            //instanciar efectos de explosion
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 moveDirection)
    {
        direction = moveDirection;
    }
    private void OnCollisionEnter(Collision collision)
    {
     
        Collider[] collidedObjects = Physics.OverlapSphere(transform.position, explosionRadius, layerMask);
        if(collidedObjects.Length > 0) { 
             foreach (Collider collidedObject in collidedObjects)
             {          
                PlayerRb isplayer = collidedObject.GetComponent<PlayerRb>();
                 if (isplayer != null)
                 {

                     Vector3 diffVector=isplayer.transform.position - transform.position;
                     Debug.Log(damageAttack / diffVector.magnitude);
                     isplayer.Damage(damageAttack/diffVector.magnitude);
                 }
             }

        }
        Destroy(gameObject,1f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

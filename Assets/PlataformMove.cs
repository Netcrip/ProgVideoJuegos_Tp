using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlataformMove : MonoBehaviour
{
    [SerializeField] private Transform initial;
    [SerializeField] private Transform final;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 targetDirection;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Vector3 finalPosition;
    [SerializeField] private Status status;
    [SerializeField] private bool lerp;


    private enum Status
    {
        Ongoing,
        Final,
        Initial
    }
    // Start is called before the first frame update
    void Start()
    {
        targetDirection= initial.position;
        initialPosition =initial.position;
        finalPosition=final.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (status)
        {
            case Status.Ongoing:
                moveTo();
            break;
            case Status.Final:
                targetDirection = initialPosition;
                moveTo();
            break;
            case Status.Initial:
                targetDirection = finalPosition;
                moveTo();
                    
            break;
        }
        if (Vector3.Distance(transform.position,finalPosition)<0.05)
        {
            status = Status.Final;
        }
        else if (Vector3.Distance(transform.position,initialPosition)<0.05)
        {
            status = Status.Initial;
        }
        else status = Status.Ongoing;

    }

    private void moveTo()
    {
        if (lerp) transform.position = Vector3.Lerp(transform.position, targetDirection, (speed /5) * Time.deltaTime);
        else transform.position = Vector3.MoveTowards(transform.position,targetDirection,speed*Time.deltaTime);
        status = Status.Ongoing;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position,initialPosition);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, finalPosition);
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerRb player = other.GetComponent<PlayerRb>();
        if (player!=null)
        {
            player.transform.parent = transform;
            player.OnLanding();
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerRb player = other.GetComponent<PlayerRb>();
        if (player != null)
        {
            player.transform.parent = null;
            player.OnLeave();
            
        }
    }

}

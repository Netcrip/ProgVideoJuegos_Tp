using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlataformMove : MonoBehaviour
{
    [SerializeField] private Transform initial;
    [SerializeField] private Transform final;
    [SerializeField] private float speed;
    private Vector3 direccion;
    [SerializeField] private Status status;
    [SerializeField] private Vector3 initialPosition, finalPosition;
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
        initialPosition = initial.position;
        finalPosition = final.position;
        status = Status.Initial;

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
                direccion = initialPosition;
                moveTo();
            break;
            case Status.Initial:
                direccion = finalPosition;
                moveTo();
                    
            break;
        }

        if (Vector3.Distance(transform.position, finalPosition)<=0.1f)
        {
            status = Status.Final;
        }
        else if (Vector3.Distance(transform.position,initialPosition)<=0.1f)
        {
            status = Status.Initial;
        }
        else status = Status.Ongoing;

    }

    private void moveTo()
    {
        if (lerp)
            transform.position = Vector3.Lerp(transform.position, direccion, Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position,direccion , Time.deltaTime*speed);
        status = Status.Ongoing;
    }

    public void OnTriggerStay(Collider other)
    {
        PlayerRb isplayer = other.GetComponent<PlayerRb>();
        if (isplayer != null)
        {
            isplayer.transform.SetParent(transform);
            isplayer.onLanding();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        PlayerRb isplayer = other.GetComponent<PlayerRb>();
        if (isplayer != null)
        {
            isplayer.transform.SetParent(null);
            isplayer.levePlataform();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, finalPosition);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, initialPosition);

    }

}

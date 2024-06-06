using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlataformMove : MonoBehaviour
{
    [SerializeField] private Transform initial;
    [SerializeField] private Transform finish;
    [SerializeField] private float speed;
    private Vector3 direccion;
    private Status status;
    private enum Status
    {
        going,
        finish,
        initial
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (status)
        {
            case Status.going:
                moveTo();
            break;
            case Status.finish:
                direccion = initial.position;
                moveTo();
            break;
            case Status.initial:
                direccion = finish.position;
                moveTo();
                    
            break;
        }
        if (transform.position == finish.position)
        {
            status = Status.finish;
        }
        else if (transform.position == initial.position)
        {
            status = Status.initial;
        }
        else status = Status.going;

    }
    private void moveTo()
    {
        transform.position += direccion * (Time.deltaTime*speed);
        status = Status.going;
    }

}

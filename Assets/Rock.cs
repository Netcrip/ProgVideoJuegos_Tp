using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float timeToDestroy = 2f;
    [SerializeField] private float speed;
    private Vector3 direction;

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
}

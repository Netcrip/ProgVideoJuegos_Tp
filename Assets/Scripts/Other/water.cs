using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerRb isplayer = other.GetComponent<PlayerRb>();
        if (isplayer != null)
        {
            isplayer.Damage(5000);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    [SerializeField] private Transform destinationPosition;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip teleport;
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
            sfx.PlayOneShot(teleport);
            isplayer.transform.position = destinationPosition.position;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : MonoBehaviour
{
    [SerializeField] private AudioSource aSource;
    [SerializeField] private AudioClip epicSound;
    // Start is called before the first frame update
    private bool flag=false;
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
        if (isplayer != null && !flag)
        {
            aSource.clip = epicSound;
            aSource.Play();
            flag = true;
            
        }
    }
}

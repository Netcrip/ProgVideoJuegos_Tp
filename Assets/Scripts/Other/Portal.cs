using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform destinationPosition;
    [SerializeField] private GameObject cam1;
    [SerializeField] private GameObject cam2;
    [SerializeField] private AudioSource sfx;

    void Awake()
    {
        GameMangarer.Instance.onPortal += portalenable;
        PlayertUiManager.Instance.onPlayerDead += doOnDead;
        PlayertUiManager.Instance.onPlayerDead += doOnwin;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void portalenable()
    {
        sfx.Play();
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        GameMangarer.Instance.onPortal -= portalenable;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerRb isplayer = other.GetComponent<PlayerRb>();
        if (isplayer != null)
        {
            isplayer.transform.position=destinationPosition.position;
            ToggleCam();
        }
    }

    public void doOnDead()
    {
        GameMangarer.Instance.onPortal -= portalenable;
        PlayertUiManager.Instance.onPlayerDead -= doOnDead;
        PlayertUiManager.Instance.onPlayerDead -= doOnwin;
    }
    public void doOnwin()
    {
        GameMangarer.Instance.onPortal -= portalenable;
        PlayertUiManager.Instance.onPlayerDead -= doOnDead;
        PlayertUiManager.Instance.onPlayerDead -= doOnwin;
    }
    private void ToggleCam()
    {
        cam1.SetActive(false);
        cam2.SetActive(true); 
    }
}

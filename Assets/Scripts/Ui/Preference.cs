using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preference : MonoBehaviour
{
    // Start is called before the first frame update
    private Main main;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip button;

    private bool click = false;

    void Start()
    {
        main = GameObject.Find("CanvasMain").GetComponent<Main>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void QuitPreference()
    {
        if (!click)
        {
            main.click = false;
            sfx.PlayOneShot(button);
            Invoke(nameof(quitPref), 1);
            click = true;
        }
        
        
    }
    private void quitPref()
    {
        GameMangarer.Instance.unloadScene("Preference");
    }
}


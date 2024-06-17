using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class youWin : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip button;

    private bool click=false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void QuitGame()
    {
        if (!click)
        {
            sfx.PlayOneShot(button);
            Invoke(nameof(ExitApp), 1);
            click = true;
        }
        
   
    }
    public void MainMenu()
    {
        if (!click)
        {
            sfx.PlayOneShot(button);
            GameMangarer.Instance.LoadLevel("Menu Principal");
            click = true;
        }


    }

    private void ExitApp()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

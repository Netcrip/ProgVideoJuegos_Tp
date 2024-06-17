using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip button;

    public bool click = false;
    private string sceneName;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStart()
    {
        if (!click)
        {
            sceneName = "Scene 2";
            sfx.PlayOneShot(button);
            Invoke(nameof(LoadNew), 0.5f);
            click = true;
        }
        
    }
    public void Preference()
    {
        if (!click)
        {
            sceneName = "Preference";
            sfx.PlayOneShot(button);
            Invoke(nameof(LoadAdittive), 0.5f);
            click = true;
        }
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

    private void ExitApp()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void LoadNew()
    {
        GameMangarer.Instance.LoadLevelwhitUi(sceneName);
    }
    private void LoadAdittive()
    {
        GameMangarer.Instance.LoadLevelAdditive(sceneName);
    }
}

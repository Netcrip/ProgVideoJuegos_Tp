using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStart()
    {
        
        GameMangarer.Instance.LoadLevelwhitUi("Scene 2");
    }
    public void Preference()
    {
        GameMangarer.Instance.LoadLevelAdditive("Preference");
    }

    public void QuitGamae()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

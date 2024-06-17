using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartExit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Restar()
    {
        GameMangarer.Instance.LoadLevelwhitUi("Scene 2");
    }
    public void QuitGamae()
    {
         Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class GameMangarer : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameMangarer Instance =>instance;

    private static GameMangarer instance;
    private bool gameStart = false;
    
    private PlayerRb player;
    public void Awake()
    {
        if( instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
           // instance = this;
            Destroy(gameObject);
        }

    }
    public void Update()
    {
        if (!player && PlayerManager.Instance)
        {
            player = PlayerManager.Instance.PlayerInstance;
            player.onDeath += doOnPlayerDeath;
            // Debug.Log(PlayerManager.Instance.PlayerInstance);}
        }
    }

    public void LoadLevel(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
        gameStart = true;
    }
    public void LoadSceneMorido()
    {
        SceneManager.LoadScene("Morido");
    }
    public void LoadLevelAdditive(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }
    public void unloadScene(string sceneToUnload)
    {
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }

    private void doOnPlayerDeath()
    {
        Invoke(nameof(LoadSceneMorido), 5);
         PlayerManager.Instance.PlayerInstance.onDeath -= doOnPlayerDeath;
    }



}

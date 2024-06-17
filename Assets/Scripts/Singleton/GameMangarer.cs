using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;


public class GameMangarer : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameMangarer Instance =>instance;

    private static GameMangarer instance;
    //private bool gameStart = false;
    

    public PlayerRb PlayerInstance => playerInstance;
    private PlayerRb playerInstance;

    private Boss jefe;

    //evento

    public Action onEnemydead;
    public Action onPortal;
    private int enemyCount;

    public Action onBossDead;



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
        
    }

    public void LoadLevel(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
        //gameStart = true;
    }
    public void LoadLevelwhitUi(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }
    public void LoadSceneMorido()
    {
        SceneManager.LoadScene("Morido");
    }
    public void LoadSceneVictory()
    {
        SceneManager.LoadScene("win");
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
         Invoke(nameof(LoadSceneMorido), 3);
        playerInstance.onDeath += doOnPlayerDeath;
         jefe.onDead -= doOnBossDead;

    }

    public void PlayerCreated(PlayerRb player)
    {
        enemyCount=0;
        playerInstance = player;
        playerInstance.onDeath += doOnPlayerDeath;
        
    }

    public void enemyCreate(Enemy enemy)
    {
        enemy.onDead += doOnEnemyDeath;      
    }
    public void boosCreate(Boss boss)
    {
        jefe = boss;
        jefe.onDead += doOnBossDead;

    }

    private void doOnEnemyDeath(Enemy enemy)
    {
        onEnemydead?.Invoke();
        enemyCount += 1;
        if (enemyCount == 6)
        {
            onPortal?.Invoke();
        }
        enemy.onDead -= doOnEnemyDeath;
    }

    private void doOnBossDead(Boss boss)
    {
        onBossDead?.Invoke();
        Invoke(nameof(LoadSceneVictory), 5);
        boss.onDead-= doOnBossDead;
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayertUiManager : MonoBehaviour
{
  

    public static PlayertUiManager Instance => instance;
    private static PlayertUiManager instance;


    public PlayerRb PlayerInstance => playerInstance;
    private PlayerRb playerInstance;


    public Action<float, float> onHealthchange;
    public Action<float, float> onShieldChange;
    public Action<float, float> onStaminaChange;
    public Action onPlayerDead;
    public Action onEnemyDead;
    public Action onPlayerWin;

  

    public void Awake()
    {
        if (instance == null)
         {
             instance = this;
             DontDestroyOnLoad(this);
         }
         else
         {
             Destroy(gameObject);
         }
    }
    public void Start()
    {


    }

    // Update is called once per frame
    public void Update()
    {

    }
    public void UpdateHealtBar(float currentHealth, float maxHealth)
    {
        onHealthchange?.Invoke(currentHealth, maxHealth);
    }
    public void UpdateShieldBar(float currentShield, float maxShield)
    {
        onShieldChange?.Invoke(currentShield, maxShield);
    }
    public void UpdateStaminadBar(float currentStamina, float maxStamina)
    {
        onStaminaChange?.Invoke(currentStamina, maxStamina);
    }

    public void PlayerCreated(PlayerRb player)
    {
        playerInstance = player;
        playerInstance.onDeath += doOnPlayerDeath;
        playerInstance.onShieldChange += UpdateShieldBar;
        playerInstance.onStaminaChange += UpdateStaminadBar;
        playerInstance.onHealthchange += UpdateHealtBar;
        playerInstance.onVictory += doOnvictory;
        GameMangarer.Instance.onEnemydead += enemysDead;
    }
    public void doOnPlayerDeath()
    {
        onPlayerDead?.Invoke();
        GameMangarer.Instance.unloadScene("HUD");
        playerInstance.onDeath -= doOnPlayerDeath;
        GameMangarer.Instance.onEnemydead -= enemysDead;
    }

    public void enemysDead()
    {
        onEnemyDead?.Invoke();
       
    }
    public void doOnvictory()
    {
        onPlayerWin?.Invoke();
        playerInstance.onVictory -= doOnvictory;
        GameMangarer.Instance.onEnemydead -= enemysDead;
    }
}
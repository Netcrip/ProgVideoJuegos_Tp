using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerManager Instance => instance;
    private static PlayerManager instance;


   /* public float maxHealth { get; set; }
    public float currentHealth { get; set; }
    public float maxShield { get; set; }
    public float currentShield { get; set; }
    public float maxStamina { get; set; }
    public float currentStamina { get; set; }
    public bool havePlayer { get; set; }*/

    public PlayerRb PlayerInstance => playerInstance;   
    private PlayerRb playerInstance;
    public PlayertUiManager UIInstance => uiInstance;
    private PlayertUiManager uiInstance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
           // havePlayer = false;
        }
        else
        {
            //instance = this;
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerCreated(PlayerRb player)
    {
        playerInstance = player;
        player.onDeath += doOnPlayerDeath;
       /* player.onShieldChange += doOnShieldChange;
        player.onStaminaChange += doOnStaminaChange;
        player.onHealthchange += doOnHealthChange;*/
    }
    public void HUDCreate(PlayertUiManager ui)
    {
        uiInstance = ui;
    }
    private void doOnPlayerDeath()
    {
         playerInstance.onDeath -= doOnPlayerDeath;  
    }
   /* private void doOnShieldChange(float Current, float max)
    {
        PlayertUiManager.Instance.UpdateShieldBar(Current, max);
    }
    private void doOnHealthChange(float Current, float max)
    {
        
        PlayertUiManager.Instance.UpdateHealtBar(Current, max);
    }
    private void doOnStaminaChange(float Current, float max)
    {
        PlayertUiManager.Instance.UpdateStaminadBar(Current, max);
    }*/


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerManager Instance => instance;
    private static PlayerManager instance;

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
        }
        else
        {

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

    }
    public void HUDCreate(PlayertUiManager ui)
    {
        uiInstance = ui;
    }
    private void doOnPlayerDeath()
    {
         playerInstance.onDeath -= doOnPlayerDeath;  
    }


}

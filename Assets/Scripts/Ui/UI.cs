using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
     private Image healthBar;

     private Image shieldthBar;

     private Image StaminaBar;

    [SerializeField] private TMP_Text enemyText;

    private int enemys=0;

    

    void Awake()
    {
        PlayertUiManager.Instance.onHealthchange += UpdateHealtBar;
        PlayertUiManager.Instance.onShieldChange += UpdateShieldBar;
        PlayertUiManager.Instance.onStaminaChange += UpdateStaminadBar;
        PlayertUiManager.Instance.onPlayerDead += onPlayerDead;
        PlayertUiManager.Instance.onEnemyDead += onEnemyDead;
        PlayertUiManager.Instance.onPlayerWin += doOnwin;
        healthBar = BuscarHijo("HealtBar");
        shieldthBar = BuscarHijo("ShieldBar");
        StaminaBar = BuscarHijo("StaminaBar");
    }
    void Start()
    {
        shieldthBar.fillAmount = 1;
        StaminaBar.fillAmount = 1;
        healthBar.fillAmount = 1;
        enemyText.text = enemys + "/6 Enemigos";


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateHealtBar(float currentHealth, float maxHealth)
    {
       float healt = currentHealth / maxHealth;
        healthBar.fillAmount = healt;
        if (healt > 0.75)
        {
            healthBar.color = new Color32(40, 255, 0, 255);
        }
        else if (healt > 0.25)
        {
            healthBar.color = new Color32(255, 161, 0, 255);
        }
        else
            healthBar.color = new Color32(255, 0, 0, 255);
    }
    public void UpdateShieldBar(float currentShield, float maxShield)
    {
        
        float shield = currentShield / maxShield;
        shieldthBar.fillAmount = shield;
    }
    public void UpdateStaminadBar(float currentStamina, float maxStamina)
    {
        
        float stamina = currentStamina / maxStamina;
     StaminaBar.fillAmount = stamina;
    }
    public void onPlayerDead()
    {
        PlayertUiManager.Instance.onHealthchange -= UpdateHealtBar;
        PlayertUiManager.Instance.onShieldChange -= UpdateShieldBar;
        PlayertUiManager.Instance.onStaminaChange -= UpdateStaminadBar;
        PlayertUiManager.Instance.onPlayerDead-= onPlayerDead;
        PlayertUiManager.Instance.onEnemyDead -= onEnemyDead;
        PlayertUiManager.Instance.onPlayerWin -= doOnwin;
    }
    private Image BuscarHijo(string name)
    {
        Image[] ts = gameObject.GetComponentsInChildren<Image>();
        foreach (Image t in ts) 
            if (t.gameObject.name == name)
                return t;
           
        return null;
    }
    private void doOnwin()
    {
        PlayertUiManager.Instance.onHealthchange -= UpdateHealtBar;
        PlayertUiManager.Instance.onShieldChange -= UpdateShieldBar;
        PlayertUiManager.Instance.onStaminaChange -= UpdateStaminadBar;
        PlayertUiManager.Instance.onPlayerDead -= onPlayerDead;
        PlayertUiManager.Instance.onEnemyDead -= onEnemyDead;
        PlayertUiManager.Instance.onPlayerWin -= doOnwin;
    }
    private void onEnemyDead()
    {
        if (enemys == 5)
        {
            enemyText.text =  "Portal Abierto";
            PlayertUiManager.Instance.onEnemyDead -= onEnemyDead;
           
        }
        else if(enemys<5)
        {
            enemys += 1;
            enemyText.text = enemys + "/6 Enemigos";
        }
            

    }
}

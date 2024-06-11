using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayertUiManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image healthBar;
    private float healt;
    [SerializeField] private Image shieldthBar;
    private float shield;
    [SerializeField] private Image StaminaBar;
    private float stamina;
    [SerializeField]private float currentHealth, currentStamina, CurrentShield;

    //private float maxStamina, maxShield, maxHealth;

    public void Awake()
    {      
        //UpdateHealtBar(HUDManager.Instance.maxHealth, HUDManager.Instance.currentHealth);
        //UpdateShieldBar(HUDManager.Instance.maxShield, HUDManager.Instance.currentShield);
        //UpdateStaminadBar(HUDManager.Instance.maxStamina, HUDManager.Instance.currentStamina);

    }
    public void Start()
    {


    }

    // Update is called once per frame
    public void Update()
    {
        if (currentHealth != PlayerManager.Instance.currentHealth)
        {
            UpdateHealtBar(PlayerManager.Instance.maxHealth, PlayerManager.Instance.currentHealth);
            currentHealth = PlayerManager.Instance.currentHealth;
        }
        if (CurrentShield != PlayerManager.Instance.currentShield)
        {
            UpdateShieldBar(PlayerManager.Instance.maxShield, PlayerManager.Instance.currentShield);
            CurrentShield = PlayerManager.Instance.currentShield;
        }
        if (currentStamina != PlayerManager.Instance.currentStamina)
        {
            UpdateStaminadBar(PlayerManager.Instance.maxStamina, PlayerManager.Instance.currentStamina);
            currentStamina = PlayerManager.Instance.currentStamina;
        }
    }
    public void UpdateHealtBar(float maxHealth, float currentHealth)
    {
        healt = currentHealth / maxHealth;
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
    public void UpdateShieldBar(float maxShield, float currentShield)
    {
        shield = currentShield / maxShield;
        shieldthBar.fillAmount = shield;
    }
    public void UpdateStaminadBar(float maxStamina, float currentStamina)
    {
        stamina = currentStamina / maxStamina;
        StaminaBar.fillAmount = stamina;
    }
}
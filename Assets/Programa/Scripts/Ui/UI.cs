using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
     private Image healthBar;

     private Image shieldthBar;

     private Image StaminaBar;


    void Awake()
    {
        PlayertUiManager.Instance.onHealthchange += UpdateHealtBar;
        PlayertUiManager.Instance.onShieldChange += UpdateShieldBar;
        PlayertUiManager.Instance.onStaminaChange += UpdateStaminadBar;
        PlayertUiManager.Instance.onPlayerDead += onPlayerDead;
        healthBar = BuscarHijo("HealtBar");
        shieldthBar = BuscarHijo("ShieldBar");
        StaminaBar = BuscarHijo("StaminaBar");
    }
    void Start()
    {
        shieldthBar.fillAmount = 1;
        StaminaBar.fillAmount = 1;
        healthBar.fillAmount = 1;
        
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
    }
    private Image BuscarHijo(string name)
    {
        Image[] ts = gameObject.GetComponentsInChildren<Image>();
        foreach (Image t in ts) 
            if (t.gameObject.name == name)
                return t;
           
        return null;
    }

}

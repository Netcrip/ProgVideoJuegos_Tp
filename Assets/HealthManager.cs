using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Image healthBar;
    private float healt;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
       transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }

    public void UpdateHealtBar(float maxHealth, float currentHealth)
    {
        healt= currentHealth / maxHealth;
        healthBar.fillAmount = healt;
        if (healt > 0.75)
        {
            healthBar.color= new Color32(40,255,0,255);
        }
        else if (healt > 0.25)
        {
            healthBar.color = new Color32(255, 161, 0, 255);
        }
        else
            healthBar.color = new Color32(255, 0, 0, 255);
    }
}

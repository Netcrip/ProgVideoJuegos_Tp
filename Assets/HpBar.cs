using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private Image hpBar;
    private float hp;
    private float maxHp;
    
    void Update()
    {
        hpBar.fillAmount = hp / maxHp;
    }
}

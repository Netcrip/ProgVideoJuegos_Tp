using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChargeBar : MonoBehaviour
{
        // Start is called before the first frame update

        [SerializeField] private Image chargeBar;
        private float charge=0;

        private Camera cam;

        void Start()
        {
         cam = Camera.main;
         chargeBar.fillAmount = charge;
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        }
        public void UpdateChargeBar(float currentCharge, float maxCharge)
        {
            charge = currentCharge / maxCharge;
            chargeBar.fillAmount = charge;
        if (charge > 1f)       
            chargeBar.color = new Color32(255, 0, 0, 255);
        else if (charge > 0.75)
            chargeBar.color = new Color32(255, 0, 0, 200);
        else if (charge > 0.25)
            chargeBar.color = new Color32(180, 90, 53, 200);
        else            
            chargeBar.color = new Color32(195, 159, 0, 150);

    }


}

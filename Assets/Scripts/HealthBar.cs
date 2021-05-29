using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthPointImg;
    public Image healthEffectImg;

    private PlayerController player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        healthPointImg.fillAmount = player.currentHp / player.maxHp;
        if(healthEffectImg.fillAmount > healthPointImg.fillAmount)
        {
            healthEffectImg.fillAmount -= 0.003f;
        }
        else
        {
            healthEffectImg.fillAmount = healthPointImg.fillAmount;
        }
    }
}

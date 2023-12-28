using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Utility;

public class MobHealthDisplay : MonoBehaviour
{
    public MobBrain brain;
    public Image healthBar;
    private void Awake()
    {
        brain.OnHealthChanged += HandleHealthChange;
    }

    private void HandleHealthChange()
    {
        healthBar.fillAmount = NumToPercent(brain.health, brain.maxHealth, brain.minHealth) / 100f;
    }

    public void Hide()
    {
        transform.parent.gameObject.SetActive(false);
    }
}

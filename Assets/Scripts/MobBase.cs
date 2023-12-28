using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class MobBase : MonoBehaviour
{
    public string mobName= string.Empty;
    public float health = 100f;
    public float maxHealth = 100f;
    public readonly float minHealth = 0;
    private bool _isTargetted = false;
    public MobBrain brainChild;
    public MobHealthDisplay healthDisplay;
    public Action OnHealthChanged;
    public GameObject mobCanvas;
    public GameObject dmgPrefab;
    public float attackDelay = 3f;
    public bool isTargetted
    {
        get { return _isTargetted; }
        set
        {
            if (value)
                UIManager.Instance.enemyPanel.SetActive(true);
            else
                UIManager.Instance.enemyPanel.SetActive(false);
            _isTargetted = value;
        }
    }

    public virtual void Attack() { }

    public virtual float TakeDamage(float _damage)
    {
        OnHealthChanged?.Invoke();
        health -= _damage;
        GameObject _dmgObject = Instantiate(dmgPrefab, mobCanvas.transform);
        TMP_Text _dmgText = _dmgObject.GetComponent<TMP_Text>();
        _dmgText.text = _damage.ToString();
        if (health <= minHealth)
        {
            healthDisplay.Hide();
            brainChild.enabled = false;
            brainChild.anim.Die();
            UIManager.Instance.HideEnemyBar();
            brainChild.SpawnItem();
        }
        return health;
    }
}

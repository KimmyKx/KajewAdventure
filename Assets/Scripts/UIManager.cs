using System;
using System.Collections;
using DigitalRuby.Tween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Action<ItemInfoScriptable> OnReceiveItem;

    [Header("Tutorial")]
    public GameObject tutorialButton;

    [Header("UI")]
    public GameObject receiveItemPanel;
    public TMP_Text receiveItemTextPrefab;

    [Header("Enemy")]
    public GameObject enemyPanel;
    public TMP_Text enemyName;
    public Image enemyBar;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        CheckTutorial();
    }

    private void CheckTutorial()
    {
        if(PlayerPrefs.GetInt("tutorial") == 0)
        {
            tutorialButton.SetActive(true);
            StartCoroutine(WaitForButton());
        }
    }

    private IEnumerator WaitForButton()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        tutorialButton.SetActive(false);
        PlayerPrefs.SetInt("tutorial", 1);
    }

    public void ShowEnemyBar(string _name, float _hp)
    {
        enemyName.text = _name;
        enemyBar.fillAmount = _hp / 100;
    }

    public void HideEnemyBar()
    {
        enemyPanel.SetActive(false);
    }

    public void ReceiveItem(ItemInfoScriptable itemInfo)
    {
        OnReceiveItem?.Invoke(itemInfo);
        TMP_Text newText = Instantiate(receiveItemTextPrefab, receiveItemPanel.transform);
        newText.text = $"x{itemInfo.itemCount} {itemInfo.itemName}";
        StartCoroutine(DelayRemoveReceiveItem(newText));
    }

    private IEnumerator DelayRemoveReceiveItem(TMP_Text text)
    {
        yield return new WaitForSeconds(2);
        float a = 1;
        Color textColor = text.color;
        while(a > 0)
        {
            a -= Time.deltaTime;
            textColor.a = a;
            text.color = textColor;
            yield return null;
        }
        Destroy(text.gameObject);
    }
}

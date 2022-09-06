using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayPanel : Panel
{
    private PlayerController m_player;
    private float m_maxHealthBarWidth;
    private float m_maxTopBarWidth;

    [SerializeField] private Image m_healthBarFill;
    [SerializeField] private Image m_blueBitsBarFill;
    [SerializeField] private Image m_redBitsBarFill;
    [SerializeField] private Image m_yellowBitsBarFill;
    [SerializeField] private Image m_purpleBitsBarFill;
    [SerializeField] private TextMeshProUGUI m_killCounterText;
    [SerializeField] private TextMeshProUGUI m_coinCounterText;

    private void Awake()
    {
        m_maxHealthBarWidth = m_healthBarFill.rectTransform.rect.width;
        m_maxTopBarWidth = m_blueBitsBarFill.rectTransform.rect.width;
    }

    private void OnEnable()
    {
        PlayerManager.OnPlayerRegistered += OnPlayerRegistered;
        GameManager.OnRunBegin += OnRunBegin;
        GameManager.OnKillsChanged += OnKillsChanged;
        UpgradeManager.OnCoinsChange += OnCoinsChange;
        UpgradeManager.OnBlueBitsChange += OnBlueBitsChange;
        UpgradeManager.OnYellowBitsChange += OnYellowBitsChange;
        UpgradeManager.OnRedBitsChange += OnRedBitsChange;
        UpgradeManager.OnPurpleBitsChange += OnPurpleBitsChange;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerRegistered -= OnPlayerRegistered;
        GameManager.OnRunBegin -= OnRunBegin;
        GameManager.OnKillsChanged -= OnKillsChanged;
        UpgradeManager.OnCoinsChange -= OnCoinsChange;
        UpgradeManager.OnBlueBitsChange -= OnBlueBitsChange;
        UpgradeManager.OnYellowBitsChange -= OnYellowBitsChange;
        UpgradeManager.OnRedBitsChange -= OnRedBitsChange;
        UpgradeManager.OnPurpleBitsChange -= OnPurpleBitsChange;
    }

    private void OnRunBegin()
    {
        OnKillsChanged(0);
        OnCoinsChange(0);
        OnBlueBitsChange(0);
        OnYellowBitsChange(0);
        OnRedBitsChange(0);
        OnPurpleBitsChange(0);
    }

    private void OnPlayerRegistered(PlayerController player)
    {
        m_player = player;
        m_player.OnHealthChange += OnHealthChange;
    }

    private void OnHealthChange(int health)
    {
        float value = Mathf.Clamp01(health / (float)m_player.UpgradeMaxHealth);
        m_healthBarFill.rectTransform.sizeDelta = new Vector2(value * m_maxHealthBarWidth, m_healthBarFill.rectTransform.sizeDelta.y);
    }

    private void OnKillsChanged(int value)
    {
        m_killCounterText.text = value.ToString();
    }
    private void OnCoinsChange(int value)
    {
        m_coinCounterText.text = value.ToString();
    }
    private void OnBlueBitsChange(int value)
    {
        float width = Mathf.Clamp01(value / (float)UpgradeManager.Instance.NextBlueUpgrade);
        m_blueBitsBarFill.rectTransform.sizeDelta = new Vector2(width * m_maxTopBarWidth, m_blueBitsBarFill.rectTransform.sizeDelta.y);
    }
    private void OnYellowBitsChange(int value)
    {
        float width = Mathf.Clamp01(value / (float)UpgradeManager.Instance.NextYellowUpgrade);
        m_yellowBitsBarFill.rectTransform.sizeDelta = new Vector2(width * m_maxTopBarWidth, m_yellowBitsBarFill.rectTransform.sizeDelta.y);
    }
    private void OnRedBitsChange(int value)
    {
        float width = Mathf.Clamp01(value / (float)UpgradeManager.Instance.NextRedUpgrade);
        m_redBitsBarFill.rectTransform.sizeDelta = new Vector2(width * m_maxTopBarWidth, m_redBitsBarFill.rectTransform.sizeDelta.y);
    }
    private void OnPurpleBitsChange(int value)
    {
        float width = Mathf.Clamp01(value / (float)UpgradeManager.Instance.NextPurpleUpgrade);
        m_purpleBitsBarFill.rectTransform.sizeDelta = new Vector2(width * m_maxTopBarWidth, m_purpleBitsBarFill.rectTransform.sizeDelta.y);
    }
}


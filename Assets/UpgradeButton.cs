using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    private float m_maxBarWidth;

    [SerializeField] private Image m_icon;
    [SerializeField] private TextMeshProUGUI m_title;
    [SerializeField] private TextMeshProUGUI m_description;
    [SerializeField] private TextMeshProUGUI m_statsDescription;
    [SerializeField] private Image m_barFill;

    private void Awake()
    {
        m_maxBarWidth = m_barFill.rectTransform.rect.width;
    }

    public void Initialize(Upgrade upgrade)
    {
        m_icon.sprite = upgrade.Icon;
        m_title.text = upgrade.Name;
        m_description.text = upgrade.Description;
        m_statsDescription.text = upgrade.EffectString;

        float barValue = Mathf.Clamp01(UpgradeManager.Instance.CurrentUpgrade(upgrade.Class)[upgrade.Effect] / (float)upgrade.MaxUpgrades);
        m_barFill.rectTransform.sizeDelta = new Vector2(barValue * m_maxBarWidth, m_barFill.rectTransform.sizeDelta.y);
    }
}
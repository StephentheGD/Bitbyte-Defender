using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : Panel
{
    private List<UpgradeButton> m_upgradeButtons;
    private ShopButton[] m_shopButtons;
    private GameObject m_orSpacer;

    [SerializeField] private GameObject m_orSpacerPrefab;

    [SerializeField] private UpgradeButton m_upgradeButtonPrefab;
    [SerializeField] private ShopButton m_shopButtonPrefab;

    [SerializeField] private Transform m_upgradeContainer;
    [SerializeField] private Transform m_shopContainer;

    [SerializeField] private AudioClip m_displayUpgradeAudio;
    [SerializeField] private AudioClip m_chooseUpgradeAudio;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ClearButtons();
    }

    private void ClearButtons()
    {
        if (m_upgradeButtons == null)
            m_upgradeButtons = new List<UpgradeButton>();

        foreach (UpgradeButton upgradeButton in m_upgradeButtons)
            if (upgradeButton != null)
                Destroy(upgradeButton.gameObject);

        Destroy(m_orSpacer);
    }

    public void DisplayUpgrades(Upgrade upgrade1, Upgrade upgrade2)
    {
        Debug.Log("Displaying Upgrades");
        if (m_upgradeButtons == null)
            m_upgradeButtons = new List<UpgradeButton>();

        ClearButtons();

        UpgradeButton upgradeButton1 = Instantiate(m_upgradeButtonPrefab, m_upgradeContainer);
        upgradeButton1.Initialize(upgrade1);
        m_upgradeButtons.Add(upgradeButton1);
        Button button1 = upgradeButton1.GetComponent<Button>();
        button1.onClick.AddListener(() => UpgradeManager.Instance.ChooseUpgrade(upgrade1));
        button1.onClick.AddListener(() => PlayAudio(m_chooseUpgradeAudio));

        m_orSpacer = Instantiate(m_orSpacerPrefab, m_upgradeContainer);

        UpgradeButton upgradeButton2 = Instantiate(m_upgradeButtonPrefab, m_upgradeContainer);
        upgradeButton2.Initialize(upgrade2);
        m_upgradeButtons.Add(upgradeButton2);
        Button button2 = upgradeButton2.GetComponent<Button>();
        button2.onClick.AddListener(() => UpgradeManager.Instance.ChooseUpgrade(upgrade2));
        button2.onClick.AddListener(() => PlayAudio(m_chooseUpgradeAudio));

        PlayAudio(m_displayUpgradeAudio);
    }
    
    public void DisplayUpgrades(Upgrade upgrade)
    {
        if (m_upgradeButtons == null)
            m_upgradeButtons = new List<UpgradeButton>();

        ClearButtons();

        UpgradeButton button = Instantiate(m_upgradeButtonPrefab, m_upgradeContainer);
        button.Initialize(upgrade);
        m_upgradeButtons.Add(button);
        button.GetComponent<Button>().onClick.AddListener(() => UpgradeManager.Instance.ChooseUpgrade(upgrade));
        button.GetComponent<Button>().onClick.AddListener(() => PlayAudio(m_chooseUpgradeAudio));

        PlayAudio(m_displayUpgradeAudio);
    }

    private void PlayAudio(AudioClip clip)
    {
        GameManager.Instance.AudioSource.PlayOneShot(clip);
    }
}

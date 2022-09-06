using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager instance;
    public static UpgradeManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<UpgradeManager>();
            return instance;
        }
    }

    private List<UpgradeClass> m_pendingUpgrades;
    private bool m_upgrading = false;

    [SerializeField] private UpgradeGroup BlueUpgrades;
    [SerializeField] private UpgradeGroup YellowUpgrades;
    [SerializeField] private UpgradeGroup RedUpgrades;
    [SerializeField] private UpgradeGroup PurpleUpgrades;

    public ref UpgradeGroup Upgrades(UpgradeClass upgradeClass)
    {
        switch (upgradeClass)
        {
            case UpgradeClass.Blue: return ref BlueUpgrades;
            case UpgradeClass.Yellow: return ref YellowUpgrades;
            case UpgradeClass.Red: return ref RedUpgrades;
            case UpgradeClass.Purple: return ref PurpleUpgrades;
            default: return ref BlueUpgrades;
        }
    }

    public int Coins = 0;
    public int BlueBits = 0;
    public int YellowBits = 0;
    public int RedBits = 0;
    public int PurpleBits = 0;

    public ref int BitsValue(UpgradeClass upgradeClass)
    {
        switch (upgradeClass)
        {
            case UpgradeClass.Blue: return ref BlueBits;
            case UpgradeClass.Yellow: return ref YellowBits;
            case UpgradeClass.Red: return ref RedBits;
            case UpgradeClass.Purple: return ref PurpleBits;
            default: return ref BlueBits;
        }
    }

    public int NextBlueUpgrade = 10;
    public int NextYellowUpgrade = 10;
    public int NextRedUpgrade = 10;
    public int NextPurpleUpgrade = 10;

    public ref int NextUpgrade(UpgradeClass upgradeClass)
    {
        switch (upgradeClass)
        {
            case UpgradeClass.Blue: return ref NextBlueUpgrade;
            case UpgradeClass.Yellow: return ref NextYellowUpgrade;
            case UpgradeClass.Red: return ref NextRedUpgrade;
            case UpgradeClass.Purple: return ref NextPurpleUpgrade;
            default: return ref NextBlueUpgrade;
        }
    }

    public int[] CurrentBlueUpgrade;
    public int[] CurrentYellowUpgrade;
    public int[] CurrentRedUpgrade;
    public int[] CurrentPurpleUpgrade;

    public ref int[] CurrentUpgrade(UpgradeClass upgradeClass)
    {
        switch (upgradeClass)
        { 
            case UpgradeClass.Blue: return ref CurrentBlueUpgrade;
            case UpgradeClass.Yellow: return ref CurrentYellowUpgrade;
            case UpgradeClass.Red: return ref CurrentRedUpgrade;
            case UpgradeClass.Purple: return ref CurrentPurpleUpgrade;
            default: return ref CurrentBlueUpgrade;
        }
    }

    public static Action<int> OnCoinsChange;
    public static Action<int> OnBlueBitsChange;
    public static Action<int> OnYellowBitsChange;
    public static Action<int> OnRedBitsChange;
    public static Action<int> OnPurpleBitsChange;

    public ref Action<int> OnBitsChange(UpgradeClass upgradeClass)
    {
        switch (upgradeClass)
        {
            case UpgradeClass.Blue: return ref OnBlueBitsChange;
            case UpgradeClass.Yellow: return ref OnYellowBitsChange;
            case UpgradeClass.Red: return ref OnRedBitsChange;
            case UpgradeClass.Purple: return ref OnPurpleBitsChange;
            default: return ref OnBlueBitsChange;
        }
    }

    public Action OnUpgradeAvailable;

    private void Awake()
    {
        CurrentBlueUpgrade = new int[BlueUpgrades.Upgrades.Length];
        CurrentYellowUpgrade = new int[YellowUpgrades.Upgrades.Length];
        CurrentRedUpgrade = new int[RedUpgrades.Upgrades.Length];
        CurrentPurpleUpgrade = new int[PurpleUpgrades.Upgrades.Length];

        m_pendingUpgrades = new List<UpgradeClass>();
    }

    private void OnEnable()
    {
        CollectableManager.OnCoinsCollect += AddCoins;
        CollectableManager.OnBitsCollect += AddBits;
    }

    public void AddCoins(int delta)
    {
        Coins += delta;
        OnCoinsChange?.Invoke(Coins);
    }

    public void AddYellowBits(int delta)
    {
        YellowBits += delta;
        OnYellowBitsChange?.Invoke(YellowBits);
    }

    public void AddRedBits(int delta)
    {
        RedBits += delta;
        OnRedBitsChange?.Invoke(RedBits);
    }

    public void AddPurpleBits(int delta)
    {
        PurpleBits += delta;
        OnPurpleBitsChange?.Invoke(PurpleBits);
    }

    public void AddBits(UpgradeClass upgradeClass, int delta)
    {
        BitsValue(upgradeClass) += delta;
        OnBitsChange(upgradeClass)?.Invoke(BitsValue(upgradeClass));

        if (BitsValue(upgradeClass) >= NextUpgrade(upgradeClass))
        {
            OfferUpgrade(upgradeClass);
            BitsValue(upgradeClass) -= NextUpgrade(upgradeClass);
            NextUpgrade(upgradeClass) = Mathf.RoundToInt(NextUpgrade(upgradeClass) * GameManager.BitsRequirementFactor);
        }
    }

    public void OfferUpgrade(UpgradeClass upgradeClass)
    {
        if (m_upgrading)
            m_pendingUpgrades.Add(upgradeClass);

        if (upgradeClass == UpgradeClass.Yellow && CurrentYellowUpgrade[0] == 0)
        {
            UpgradePanel panel1 = PanelManager.Instance.GetPanelOfType<UpgradePanel>();
            panel1.gameObject.SetActive(true);

            panel1.DisplayUpgrades(YellowUpgrades.Upgrades[0]);

            m_upgrading = true;
            Time.timeScale = 0;
            return;
        }
        
        if (upgradeClass == UpgradeClass.Red && CurrentRedUpgrade[0] == 0)
        {
            UpgradePanel panel1 = PanelManager.Instance.GetPanelOfType<UpgradePanel>();
            panel1.gameObject.SetActive(true);

            panel1.DisplayUpgrades(RedUpgrades.Upgrades[0]);

            m_upgrading = true;
            Time.timeScale = 0;
            return;
        }

        List<Upgrade> validUpgrades = new();
        for (int i = 0; i < Upgrades(upgradeClass).Upgrades.Length; i++)
        {
            Upgrade upgrade = Upgrades(upgradeClass).Upgrades[i];
            if (CurrentUpgrade(upgradeClass)[i] < upgrade.MaxUpgrades)
                validUpgrades.Add(upgrade);
        }

        if (validUpgrades.Count == 0)
        {
            if (m_pendingUpgrades.Count > 0)
            {
                UpgradeClass pendingUpgradeClass = m_pendingUpgrades[0];
                m_pendingUpgrades.Remove(pendingUpgradeClass);
                OfferUpgrade(pendingUpgradeClass);
                return;
            }
            else
            {
                Time.timeScale = 1;
                return;
            }
        }

        UpgradePanel panel = PanelManager.Instance.GetPanelOfType<UpgradePanel>();
        panel.gameObject.SetActive(true);

        if (validUpgrades.Count == 1)
            panel.DisplayUpgrades(validUpgrades[0]);
        else
        {
            Upgrade upgrade1 = validUpgrades[UnityEngine.Random.Range(0, validUpgrades.Count)];
            validUpgrades.Remove(upgrade1);
            Upgrade upgrade2 = validUpgrades[UnityEngine.Random.Range(0, validUpgrades.Count)]; 

            panel.DisplayUpgrades(upgrade1, upgrade2);
        }

        m_upgrading = true;
        Time.timeScale = 0;
    }

    public void ChooseUpgrade(Upgrade upgrade)
    {
        m_upgrading = false;

        CurrentUpgrade(upgrade.Class)[upgrade.Effect]++;
        PanelManager.Instance.GetPanelOfType<UpgradePanel>().gameObject.SetActive(false);

        if (upgrade.Class == UpgradeClass.Yellow && upgrade.Effect == 0)
            PlayerManager.Instance.CurrentPlayer.AddRotatingGun();

        if (upgrade.Class == UpgradeClass.Red && upgrade.Effect == 0)
            PlayerManager.Instance.CurrentPlayer.AddRotatingMelee();

        if (m_pendingUpgrades.Count > 0)
        {
            UpgradeClass upgradeClass = m_pendingUpgrades[0];
            m_pendingUpgrades.Remove(upgradeClass);
            OfferUpgrade(upgradeClass);
            return;
        }

        Time.timeScale = 1;
    }
}

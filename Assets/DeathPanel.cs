using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathPanel : Panel
{
    [SerializeField] private TextMeshProUGUI m_coinHighScoreText;
    [SerializeField] private TextMeshProUGUI m_killHighScoreText;

    [SerializeField] private Image m_coinHighScoreImage;
    [SerializeField] private Image m_killHighScoreImage;

    private void OnEnable()
    {
        bool newHighScoreKills = false;
        if (!PlayerPrefs.HasKey("high_kills"))
            PlayerPrefs.SetInt("high_kills", GameManager.Instance.Kills);
        else if (PlayerPrefs.GetInt("high_kills") < GameManager.Instance.Kills)
        {
            newHighScoreKills = true;
            PlayerPrefs.SetInt("high_kills", GameManager.Instance.Kills);
        }

        bool newHighScoreCoins = false;
        if (!PlayerPrefs.HasKey("high_coins"))
            PlayerPrefs.SetInt("high_coins", UpgradeManager.Instance.Coins);
        else if (PlayerPrefs.GetInt("high_coins") < UpgradeManager.Instance.Coins)
        {
            newHighScoreCoins = true;
            PlayerPrefs.SetInt("high_coins", UpgradeManager.Instance.Coins);
        }

        m_killHighScoreImage.gameObject.SetActive(newHighScoreKills);
        m_coinHighScoreImage.gameObject.SetActive(newHighScoreCoins);
    }

    public void TryAgain()
    {
        GameManager.Instance.StartRun();
    }

    public void ToMenu()
    {
        GameManager.Instance.ToMenu();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuPanel : Panel
{
    [SerializeField] private TextMeshProUGUI m_coinHighScoreText;
    [SerializeField] private TextMeshProUGUI m_killHighScoreText;

    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey("high_coins"))
            PlayerPrefs.SetInt("high_coins", 0);
        m_coinHighScoreText.text = PlayerPrefs.GetInt("high_coins").ToString();

        if (!PlayerPrefs.HasKey("high_kills"))
            PlayerPrefs.SetInt("high_kills", 0);
        m_killHighScoreText.text = PlayerPrefs.GetInt("high_kills").ToString();
    }

    public void GoButton()
    {
        GameManager.Instance.StartRun();
    }
}

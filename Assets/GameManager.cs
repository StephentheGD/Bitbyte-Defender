using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager Instance
    {
        get 
        { 
            if (m_instance == null)
                m_instance = FindObjectOfType<GameManager>();
            return m_instance; 
        }
    }

    public bool InRun = false;
    public int Kills;

    public AudioSource AudioSource;

    public static float BitsRequirementFactor = 1.25f;

    public static Action OnRunBegin;
    public static Action<int> OnKillsChanged;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
            DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        PanelManager.Instance.GetPanelOfType<MenuPanel>().gameObject.SetActive(true);
        PanelManager.Instance.GetPanelOfType<GameplayPanel>().gameObject.SetActive(false);
        PanelManager.Instance.GetPanelOfType<UpgradePanel>().gameObject.SetActive(false);
        PanelManager.Instance.GetPanelOfType<DeathPanel>().gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        EnemyManager.OnEnemyDeath += OnEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyManager.OnEnemyDeath -= OnEnemyDeath;
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        Kills++;
        OnKillsChanged(Kills);
    }

    public void StartRun()
    {
        SceneManager.LoadScene("Gameplay");
        PanelManager.Instance.GetPanelOfType<MenuPanel>().gameObject.SetActive(false);
        PanelManager.Instance.GetPanelOfType<GameplayPanel>().gameObject.SetActive(true);
        PanelManager.Instance.GetPanelOfType<UpgradePanel>().gameObject.SetActive(false);
        PanelManager.Instance.GetPanelOfType<DeathPanel>().gameObject.SetActive(false);

        InRun = true;
        Kills = 0;
        OnRunBegin?.Invoke();
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
        PanelManager.Instance.GetPanelOfType<MenuPanel>().gameObject.SetActive(true);
        PanelManager.Instance.GetPanelOfType<GameplayPanel>().gameObject.SetActive(false);
        PanelManager.Instance.GetPanelOfType<UpgradePanel>().gameObject.SetActive(false);
        PanelManager.Instance.GetPanelOfType<DeathPanel>().gameObject.SetActive(false);
        InRun = false;
        Kills = 0;
    }

    public void EndRun()
    {
        PanelManager.Instance.GetPanelOfType<MenuPanel>().gameObject.SetActive(false);
        PanelManager.Instance.GetPanelOfType<GameplayPanel>().gameObject.SetActive(false);
        PanelManager.Instance.GetPanelOfType<UpgradePanel>().gameObject.SetActive(false);
        PanelManager.Instance.GetPanelOfType<DeathPanel>().gameObject.SetActive(true);

        InRun = false;
    }
}

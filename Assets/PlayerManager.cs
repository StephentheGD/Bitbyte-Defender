using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager m_manager;

    public static PlayerManager Instance
    {
        get
        {
            if (m_manager == null)
                m_manager = FindObjectOfType<PlayerManager>();
            return m_manager;
        }
    }

    public PlayerController CurrentPlayer;

    public static Action<PlayerController> OnPlayerRegistered;

    public void RegisterPlayer(PlayerController player)
    {
        CurrentPlayer = player;
        OnPlayerRegistered(CurrentPlayer);
    }
}

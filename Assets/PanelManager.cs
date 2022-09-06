using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    private static PanelManager m_instance;
    public static PanelManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<PanelManager>();
            return m_instance;
        }
    }

    private List<Panel> m_panels;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        m_panels = new List<Panel>();
        foreach (Panel panel in GetComponentsInChildren<Panel>())
            m_panels.Add(panel);
    }

    public T GetPanelOfType<T>()
    {
        foreach (Panel panel in m_panels)
            if (panel.GetType() == typeof(T))
                return (T)(object)panel;

        return default;
    }
}

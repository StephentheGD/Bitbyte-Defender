using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static CameraFollow m_instance;
    public static CameraFollow Instance
    {
        get 
        { 
            if (m_instance == null)
                m_instance = FindObjectOfType<CameraFollow>();
            return m_instance; 
        } 
    }

    [SerializeField] private Transform m_target;
    [SerializeField] private float m_followSpeed;
    [SerializeField] private Vector3 m_offset;
    [SerializeField] private Vector3 m_rotation;

    public Camera Camera;

    private void Awake()
    {
        Camera = GetComponent<Camera>();
    }

    private void Start()
    {
        transform.eulerAngles = m_rotation;
    }

    private void Update()
    {
        if (m_target == null) return;

        Vector3 targetPosition = m_target.position - m_offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, m_followSpeed * Time.deltaTime);
    }
}

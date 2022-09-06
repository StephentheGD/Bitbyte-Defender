using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int m_health;
    private Vector3 m_movementVector;
    private Rigidbody m_rb;

    [SerializeField] private int m_baseHealth = 2;
    [SerializeField] private int m_baseDamage = 10;
    [SerializeField] private float m_baseSpeed = 1f;
    [SerializeField] private float m_acceleration = 0.1f;

    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip[] m_damageAudio;

    public Transform VisualsContainer;

    private void Awake()
    {
        m_health = m_baseHealth;
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!GameManager.Instance.InRun) return;

        m_rb.velocity = Vector3.zero;

        Vector3 target = PlayerManager.Instance.CurrentPlayer.transform.position;
        Vector3 directionalVector = (target - transform.position).normalized;
        transform.Translate(directionalVector * m_baseSpeed * Time.deltaTime);

        transform.rotation = Quaternion.identity;

        /*
        transform.LookAt(PlayerManager.Instance.CurrentPlayer.transform);
        Vector3 targetMovementVector = transform.rotation * Vector3.forward * m_baseSpeed * Time.deltaTime;
        m_movementVector = Vector3.Lerp(m_movementVector, targetMovementVector, m_acceleration);
        transform.Translate(m_movementVector);
        transform.rotation = Quaternion.identity;
        */
    }

    public void Initialize(int damage, int health)
    {
        m_baseHealth = health;
        m_baseDamage = damage;
    }

    public void Damage(int damage, float knockback)
    {
        if (!GameManager.Instance.InRun) return;

        m_health -= damage;
        if (m_health < 0) Die();

        Vector3 directionalVector = (transform.position - PlayerManager.Instance.CurrentPlayer.transform.position).normalized;
        m_movementVector = directionalVector * knockback;
        DamageAudio();
    }

    private void DamageAudio()
    {
        GameManager.Instance.AudioSource.PlayOneShot(m_damageAudio[Random.Range(0, m_damageAudio.Length)]);
        //m_audioSource.PlayOneShot(m_damageAudio[Random.Range(0, m_damageAudio.Length)]);
    }

    private void Die()
    {
        EnemyManager.Instance.RegisterEnemyDeath(this);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!GameManager.Instance.InRun) return;
        if (collision == null) return;

        if (collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponent<PlayerController>().Damage(m_baseDamage);
        }
    }
}

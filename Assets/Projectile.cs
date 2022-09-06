using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Projectile : MonoBehaviour
{
    private int m_damage;
    private float m_decayTimer;

    [SerializeField] private float m_baseSpeed = 1;
    //[SerializeField] private int m_baseDamage = 1;
    [SerializeField] private float m_baseKnockback = 1;
    [SerializeField] private float m_decay = 10f;

    public void Initialize(int damage)
    {
        m_damage = damage;
        m_decayTimer = m_decay;
    }

    void Update()
    {
        transform.position += m_baseSpeed * Time.deltaTime * (transform.rotation * Vector3.forward);// * RotateVector3(transform.rotation * Vector3.forward, 45);
        m_decayTimer -= Time.deltaTime;
        if (m_decayTimer < 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;

        if (collision.gameObject.layer == 7)
        {
            Vector3 normal = new Vector3(collision.contacts[0].normal.x, 0, collision.contacts[0].normal.z);
            collision.gameObject.GetComponent<Enemy>().Damage(m_damage, m_baseKnockback);
            Destroy(gameObject);
        }
    }
}

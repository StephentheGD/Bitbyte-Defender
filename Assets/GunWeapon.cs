using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWeapon : MonoBehaviour
{
    [SerializeField] private Projectile m_projectilePrefab;
    [SerializeField] private Transform m_projectileOrigin;
    [SerializeField] private float m_scatterValue = 0.25f;

    [SerializeField] private AudioClip[] m_shotAudio;

    public float BaseDamage;
    public UpgradeClass UpgradeClass;
    public int UpgradeEffect;

    public void Shoot(int damage = 1)
    {
        if (UpgradeClass == UpgradeClass.Blue)
        {
            for (int i = 0; i < UpgradeHandler.GetUpgradedValue(UpgradeClass.Blue, 1, 1); i++)
                CreateProjectile(damage, new Vector3(Random.value * m_scatterValue, 0, Random.value * m_scatterValue));
        }    
        else
            CreateProjectile(damage, Vector3.zero);

        ShotAudio();
    }

    private void ShotAudio()
    {
        GameManager.Instance.AudioSource.PlayOneShot(m_shotAudio[Random.Range(0, m_shotAudio.Length)]);
        //m_audioSource.PlayOneShot(m_damageAudio[Random.Range(0, m_damageAudio.Length)]);
    }

    private void CreateProjectile(int damage, Vector3 offset)
    {
        Projectile newProjectile = Instantiate(m_projectilePrefab, m_projectileOrigin.transform.position + offset, m_projectileOrigin.transform.rotation);
        newProjectile.Initialize(damage);
    }
}

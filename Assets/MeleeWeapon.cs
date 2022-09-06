using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    private Coroutine m_animationRoutine;
    private List<Enemy> m_registeredEnemies;
    private float m_attackTimer;

    [SerializeField] private AnimationCurve m_enablingCurve;
    [SerializeField] private float m_enablingTime = 1f;
    [SerializeField] private Sprite[] m_sprites;
    [SerializeField] private float m_animationSpeed;
    [SerializeField] private float m_attackCooldown;
    [SerializeField] private int m_damage = 1;
    [SerializeField] private float m_knockback = 1;

    public SpriteRenderer Renderer;
    public int UpgradedDamage => Mathf.RoundToInt(UpgradeHandler.GetUpgradedValue(UpgradeClass.Red, 1, m_damage));
    public int UpgradedSize => Mathf.RoundToInt(UpgradeHandler.GetUpgradedValue(UpgradeClass.Red, 2, 1));

    private void OnEnable()
    {
        if (m_registeredEnemies != null) m_registeredEnemies.Clear();
        m_registeredEnemies = new List<Enemy>();

        if (m_animationRoutine != null)
            StopCoroutine(m_animationRoutine);
        m_animationRoutine = StartCoroutine(AnimateEntrance());
    }

    public void Disable()
    {
        if (m_animationRoutine != null)
            StopCoroutine(m_animationRoutine);
        m_animationRoutine = StartCoroutine(AnimateExit());
    }

    private void Update()
    {
        Renderer.sprite = m_sprites[Mathf.RoundToInt(Time.time / m_animationSpeed) % 2];

        m_attackTimer -= Time.deltaTime;
        if (m_attackTimer < 0)
        {
            m_attackTimer = m_attackCooldown;
            HandleAttack();
        }
    }
    
    private void HandleAttack()
    {
        for (int i = m_registeredEnemies.Count - 1; i >= 0; i--)
            if (m_registeredEnemies[i] == null)
                m_registeredEnemies.RemoveAt(i);

        foreach (Enemy enemy in m_registeredEnemies)
            enemy.Damage(UpgradedDamage, m_knockback);
    }

    private IEnumerator AnimateEntrance()
    {
        float startTime = Time.time;
        float endTime = startTime + m_enablingTime;
        while (Time.time < endTime)
        {
            float lerp = Mathf.InverseLerp(startTime, endTime, Time.time);
            float evaluation = m_enablingCurve.Evaluate(lerp);
            transform.localScale = Vector3.one * evaluation * UpgradedSize;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = Vector3.one * UpgradedSize;
    }
    
    private IEnumerator AnimateExit()
    {
        float startTime = Time.time;
        float endTime = startTime + m_enablingTime;
        while (Time.time < endTime)
        {
            float evaluation = m_enablingCurve.Evaluate(Mathf.InverseLerp(endTime, startTime, Time.time));
            transform.localScale = Vector3.one * evaluation * UpgradedSize;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = Vector3.zero;

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 7) return;

        Enemy enemy = other.GetComponent<Enemy>();
        m_registeredEnemies.Add(enemy);
        enemy.Damage(UpgradedDamage, m_knockback);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 7) return;

        m_registeredEnemies.Remove(other.GetComponent<Enemy>());
    }
}

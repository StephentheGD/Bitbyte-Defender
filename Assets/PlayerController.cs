using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_rb;
    private Vector3 m_movement;
    private float m_lastHitTime;
    private int m_kills;
    private float m_rotatingGunTimer;
    private float m_rotatingMeleeTimer;
    private bool m_rotatingMeleeActive = false;
    private float m_shotTimer;

    private List<GunWeapon> m_rotatingGuns;
    private List<MeleeWeapon> m_rotatingMelees;

    [SerializeField] private int m_health = 100;
    [SerializeField] private int m_maxHealth = 100;
    [SerializeField] private float m_hitCooldown = 1f;
    [SerializeField] private int m_baseDamage = 1;
    [SerializeField] private float m_shotCooldown = 0.25f;
    [SerializeField] private float m_moveSpeed = 1;
    [SerializeField] private float m_rotatingGunRotationSpeed = 1;
    [SerializeField] private float m_rotatingGunFireRate = 1;
    [SerializeField] private float m_rotatingMeleeRotationSpeed = 1;
    [SerializeField] private float m_rotatingMeleeLifetime = 2;
    [SerializeField] private float m_rotatingMeleeCooldown = 3;
    [SerializeField] private float m_yRotationOffset = 45;

    [SerializeField] private GameObject m_visuals;
    [SerializeField] private GunWeapon m_gun;
    [SerializeField] private Animator m_animator;

    [SerializeField] private Sprite[] m_meleeSprites;
    [SerializeField] private float m_meleeAnimationSpeed = 0.2f;
    [SerializeField] private Transform m_meleeContainer;

    public float Kills { get => m_kills; }
    public float Health { get => m_health; }
    public bool InCooldown { get => m_lastHitTime + m_hitCooldown < Time.deltaTime; }
    public float UpgradeMoveSpeed => UpgradeHandler.GetUpgradedValue(UpgradeClass.Purple, 0, m_moveSpeed);
    public float UpgradeMaxHealth => UpgradeHandler.GetUpgradedValue(UpgradeClass.Purple, 1, m_maxHealth);
    public float UpgradeShotCooldown => UpgradeHandler.GetUpgradedValue(UpgradeClass.Blue, 2, m_shotCooldown);

    public bool Active = true;
    public bool Invincible = false;

    public Action<int> OnHealthChange;
    public Action<int> OnDeath;

    //TODO: Remove this and implement it properly
    [SerializeField] private GunWeapon m_gunPrefab;
    [SerializeField] private MeleeWeapon m_meleePrefab;
    public bool AddRotatingGunDebug = false;
    public bool AddRotatingMeleeDebug = false;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        EnemyManager.OnEnemyDeath += OnEnemyDeath;
        CollectableManager.OnGreenBitsCollect += Heal;

        m_rotatingGunTimer = m_rotatingGunFireRate;
        m_rotatingMeleeTimer = m_rotatingMeleeCooldown;
    }

    private void OnDisable()
    {
        EnemyManager.OnEnemyDeath -= OnEnemyDeath;
    }

    private void Start()
    {
        m_health = m_maxHealth;
        PlayerManager.Instance.RegisterPlayer(this);
    }

    private void Update()
    {
        if (!GameManager.Instance.InRun) return;
        if (!Active) return;

        UpdateMovement();
        UpdatePointer();
        UpdateRotatingGuns();
        UpdateRotatingMelees();

        m_shotTimer -= Time.deltaTime;
        if (m_shotTimer < 0)
            Shoot();
    }

    private void UpdateRotatingMelees()
    {
        if (m_rotatingMelees == null)
            m_rotatingMelees = new List<MeleeWeapon>();

        if (AddRotatingMeleeDebug)
        {
            AddRotatingMeleeDebug = false;
            AddRotatingMelee();
        }

        int numberOfMelees = m_rotatingMelees.Count;
        if (numberOfMelees == 0) return;

        float degrees = (360f / (float)numberOfMelees);

        for (int i = 0; i < numberOfMelees; i++)
            m_rotatingMelees[i].transform.rotation = Quaternion.Euler(0, (i * degrees) + (UpgradeHandler.GetUpgradedValue(UpgradeClass.Red, 3, m_rotatingMeleeRotationSpeed) * Time.time), 0);

        m_rotatingMeleeTimer -= Time.deltaTime;
        if (m_rotatingMeleeTimer <= 0)
        {
            m_rotatingMeleeActive = !m_rotatingMeleeActive;

            foreach (MeleeWeapon melee in m_rotatingMelees)
            {
                if (m_rotatingMeleeActive)
                    melee.gameObject.SetActive(true);
                else
                    melee.Disable();
            }

            m_rotatingMeleeTimer = m_rotatingMeleeActive ? m_rotatingMeleeLifetime : UpgradeHandler.GetUpgradedValue(UpgradeClass.Red, 4, m_rotatingMeleeCooldown);
        }
    }

    public void AddRotatingGun()
    {
        m_rotatingGuns.Add(Instantiate(m_gunPrefab, transform));
    }

    public void AddRotatingMelee()
    {
        m_rotatingMelees.Add(Instantiate(m_meleePrefab, m_meleeContainer));
    }

    private void UpdateRotatingGuns()
    {
        if (m_rotatingGuns == null)
            m_rotatingGuns = new List<GunWeapon>();

        if (AddRotatingGunDebug)
        {
            AddRotatingGunDebug = false;
            AddRotatingGun();
        }

        int numberOfGuns = m_rotatingGuns.Count;
        if (numberOfGuns == 0) return;

        float degrees = (360f / (float)numberOfGuns);

        for (int i = 0; i < numberOfGuns; i++)
            m_rotatingGuns[i].transform.rotation = Quaternion.Euler(0, (i * degrees) + (UpgradeHandler.GetUpgradedValue(UpgradeClass.Yellow, 3, m_rotatingGunRotationSpeed) * Time.time), 0);

        m_rotatingGunTimer -= Time.deltaTime;
        if (m_rotatingGunTimer <= 0)
        {
            foreach (GunWeapon gun in m_rotatingGuns) gun.Shoot(Mathf.RoundToInt(UpgradeHandler.GetUpgradedValue(UpgradeClass.Yellow, 3, m_rotatingGunRotationSpeed)));

            m_rotatingGunTimer = UpgradeHandler.GetUpgradedValue(UpgradeClass.Yellow, 2, m_rotatingGunFireRate);
        }
    }

    public void ChangeMoveValue(Vector2 movement)
    {
        movement = Utilities.RotateVector2(movement, m_yRotationOffset);
        movement *= UpgradeMoveSpeed * Time.deltaTime;
        Vector3 parsedMovement = new(movement.x, 0, movement.y);
        m_movement = parsedMovement;
    }

    private void UpdateMovement()
    {
        m_rb.velocity = Vector3.zero;
        transform.Translate(m_movement);
        m_animator.SetBool("Running", m_movement.sqrMagnitude > 0);
    }

    private void UpdatePointer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit, maxDistance: 100, layerMask: LayerMask.GetMask("Ground")))
        {
            m_visuals.transform.LookAt(hit.point);
            m_visuals.transform.eulerAngles = new Vector3(0, m_visuals.transform.eulerAngles.y, 0);
        }
    }

    public void Shoot()
    {
        if (m_shotTimer > 0) return;
        int damageValue = Mathf.RoundToInt(UpgradeHandler.GetUpgradedValue(UpgradeClass.Blue, 0, m_baseDamage));
        m_gun.Shoot(damageValue);
        m_shotTimer = UpgradeShotCooldown;
    }

    public void Damage(int damage)
    {
        if (Invincible) return;
        if (InCooldown) return;

        m_health -= damage;
        m_health = Mathf.RoundToInt(Mathf.Clamp(m_health, 0, UpgradeMaxHealth));
        OnHealthChange?.Invoke(m_health);
        m_lastHitTime = Time.time;

        if (m_health <= 0) Die();
    }

    public void Die()
    {
        GameManager.Instance.EndRun();
    }

    public void Heal(int delta)
    {
        m_health += delta;
        m_health = Mathf.RoundToInt(Mathf.Clamp(m_health, 0, UpgradeMaxHealth));
        OnHealthChange?.Invoke(m_health);
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        m_kills++;
    }
}

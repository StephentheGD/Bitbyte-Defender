using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager m_instance;
    public static EnemyManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<EnemyManager>();
            return m_instance;
        }
    }

    private MonsterFactory m_monsterFactory;
    public MonsterFactory MonsterFactory
    {
        get
        {
            if (m_monsterFactory == null)
                m_monsterFactory = GetComponent<MonsterFactory>();
            return m_monsterFactory;
        }
    }

    private float m_spawnTimer = 0;
    private float m_waveTimer = 0;

    //private List<Enemy> m_activeEnemies;

    [SerializeField] private float m_debugSpawnTime = 2f;
    [SerializeField] private float m_spawnOffset = 15f;

    [SerializeField] private GameObject m_enemy;

    public EnemyWave[] EnemyWaves;
    public int CurrentWave;
    public int CurrentPrestige;
    public int PrestigeBonus = 3;

    public List<PendingEnemyGroup> PendingGroups;

    public static Action<Enemy> OnEnemyDeath;

    private void Awake()
    {
        //m_activeEnemies = new List<Enemy>();
        // TODO: This might need to be moved to a callback from OnRunStart
        PendingGroups = new List<PendingEnemyGroup>();
    }

    private void OnEnable()
    {
        GameManager.OnRunBegin += ResetWaves;
    }
    private void OnDisable()
    {
        GameManager.OnRunBegin -= ResetWaves;
    }

    private void Update()
    {
        if (!GameManager.Instance.InRun) return;
        if (PlayerManager.Instance.CurrentPlayer == null) return;

        m_waveTimer -= Time.deltaTime;
        if (m_waveTimer < 0)
        {
            BeginWave();
        }
        /*
        m_spawnTimer += Time.deltaTime;
        if (m_spawnTimer > m_debugSpawnTime)
        {
            Enemy enemy = this.MonsterFactory.GenerateEnemy((MonsterColour)UnityEngine.Random.Range(0, 7));
            
            enemy.transform.position = new Vector3(UnityEngine.Random.value * 10, 0, UnityEngine.Random.value * 10);
            //Instantiate(m_enemy, new Vector3(UnityEngine.Random.value * 10, 0, UnityEngine.Random.value * 10), Quaternion.identity);
            m_spawnTimer = 0;
        }
        */
    }

    public void ResetWaves()
    {
        CurrentWave = 0;
        CurrentPrestige = 0;
        m_waveTimer = 0;
    }

    public void BeginWave()
    {
        EnemyWave currentWave = EnemyWaves[CurrentWave];
        foreach (EnemyWave.EnemyGroup group in currentWave.Groups)
        {
            PendingEnemyGroup pendingGroup = new PendingEnemyGroup(group, group.Count);
            PendingGroups.Add(pendingGroup);
            for (int i = 0; i < group.MaxSpawn; i++)
            {
                pendingGroup.SpawnedEnemies.Add(SpawnEnemy(group));
                pendingGroup.Remaining--;
            }
        }

        m_waveTimer = currentWave.WaveTime;

        CurrentWave++;
        if (CurrentWave >= EnemyWaves.Length)
        {
            CurrentPrestige++;
            CurrentWave -= EnemyWaves.Length;
        }
    }

    private Enemy SpawnEnemy(EnemyWave.EnemyGroup group)
    {
        Enemy enemy = this.MonsterFactory.GenerateEnemy(group.Colour);

        float randomAngle = UnityEngine.Random.Range(0, 360 * Mathf.Deg2Rad);
        float xPos = Mathf.Cos(randomAngle) * m_spawnOffset;
        float yPos = Mathf.Sin(randomAngle) * m_spawnOffset;
        Vector3 spawnPos = new Vector3(xPos, 0, yPos);
        enemy.transform.position = PlayerManager.Instance.CurrentPlayer.transform.position + spawnPos;
        enemy.transform.localScale = Vector3.one * group.Scale;
        enemy.Initialize(group.Attack * Mathf.RoundToInt(Mathf.Pow(CurrentPrestige, PrestigeBonus) + 1), group.Health * Mathf.RoundToInt(Mathf.Pow(CurrentPrestige, PrestigeBonus)));
        //m_activeEnemies.Add(enemy);
        return enemy;
    }

    public void RegisterEnemyDeath(Enemy enemy)
    {
        foreach (PendingEnemyGroup pendingGroup in PendingGroups)
            if (pendingGroup.SpawnedEnemies.Contains(enemy))
            {
                pendingGroup.SpawnedEnemies.Remove(enemy);

                if (pendingGroup.Remaining > 0)
                {
                    pendingGroup.SpawnedEnemies.Add(SpawnEnemy(pendingGroup.Group));
                    pendingGroup.Remaining--;
                }
            }

        for (int i = PendingGroups.Count - 1; i >= 0; i--)
            if (PendingGroups[i].SpawnedEnemies.Count == 0 && PendingGroups[i].Remaining == 0)
                PendingGroups.RemoveAt(i);

        OnEnemyDeath.Invoke(enemy);

        if (PendingGroups.Count == 0)
            BeginWave();
    }

    public class PendingEnemyGroup
    {
        public EnemyWave.EnemyGroup Group;
        public int Remaining;
        public List<Enemy> SpawnedEnemies;

        public PendingEnemyGroup(EnemyWave.EnemyGroup group, int remaining)
        {
            Group = group;
            Remaining = remaining;
            SpawnedEnemies = new List<Enemy>();
        }
    }
}

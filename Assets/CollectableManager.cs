using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    [SerializeField] private Collectable[] m_prefabs;
    [SerializeField] private float m_collectionBeginDistance;
    [SerializeField] private float m_collectionEndDistance;
    [SerializeField] private float m_collectionSpeed;
    [SerializeField] private float m_maxCollectablesPerEnemy;
    [SerializeField] private float m_collectablesSpawnOffset = 1;

    [SerializeField] private AudioClip[] m_collectAudios;

    public List<Collectable> Collectables = new();

    public static Action<int> OnCoinsCollect;
    public static Action<UpgradeClass, int> OnBitsCollect;
    public static Action<int> OnGreenBitsCollect;

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
        for (int i = 0; i < (int)(UnityEngine.Random.value * m_maxCollectablesPerEnemy); i++)
        {
            CollectableType randomType = (CollectableType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(CollectableType)).Length);
            CreateCollectable(randomType, enemy.transform.position);
        }
    }

    public void CreateCollectable(CollectableType type, Vector3 position)
    {
        Vector3 offset = Vector3.one * UnityEngine.Random.value * m_collectablesSpawnOffset;
        offset.y = 0;
        Collectable newCollectable = Instantiate(m_prefabs[(int)type], position + (Vector3.up * 1.5f) + offset, Quaternion.identity);
        Collectables.Add(newCollectable);
    }

    private void Update()
    {
        if (!GameManager.Instance.InRun) return;
        if (PlayerManager.Instance.CurrentPlayer == null) return;

        Vector3 target = PlayerManager.Instance.CurrentPlayer.transform.position;
        foreach (Collectable collectable in Collectables)
        {
            Vector3 targetVector = target - collectable.transform.position;
            float sqrMagnitude = targetVector.sqrMagnitude;
            float beginDistance = UpgradeHandler.GetUpgradedValue(UpgradeClass.Purple, 2, m_collectionBeginDistance);
            if (sqrMagnitude < (beginDistance * beginDistance))
            {
                collectable.transform.position += targetVector.normalized * UpgradeHandler.GetUpgradedValue(UpgradeClass.Purple, 3, m_collectionSpeed) * Time.deltaTime;
                if (sqrMagnitude < (m_collectionEndDistance * m_collectionEndDistance))
                {
                    CollectCollectable(collectable);
                    collectable.gameObject.SetActive(false);
                }
            }
        }

        for (int i = Collectables.Count - 1; i >= 0; i--)
            if (!Collectables[i].gameObject.activeSelf)
            {
                Destroy(Collectables[i]);
                Collectables.RemoveAt(i);
            }
    }

    private void CollectCollectable(Collectable collectable)
    {
        switch (collectable.CollectableType)
        {
            case CollectableType.Coin: OnCoinsCollect?.Invoke(1); break;
            case CollectableType.BlueBit: OnBitsCollect?.Invoke(UpgradeClass.Blue, 1); break;
            case CollectableType.YellowBit: OnBitsCollect?.Invoke(UpgradeClass.Yellow, 1); break;
            case CollectableType.RedBit: OnBitsCollect?.Invoke(UpgradeClass.Red, 1); break;
            case CollectableType.PurpleBit: OnBitsCollect?.Invoke(UpgradeClass.Purple, 1); break;
            case CollectableType.GreenBit: OnGreenBitsCollect?.Invoke(1); break;
        }

        CollectAudio();
    }

    private void CollectAudio()
    {
        GameManager.Instance.AudioSource.PlayOneShot(m_collectAudios[UnityEngine.Random.Range(0, m_collectAudios.Length)], 0.1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Wave", menuName = "ScriptableObjects/Enemy Wave", order = 3)]
public class EnemyWave : ScriptableObject
{
    [System.Serializable]
    public class EnemyGroup
    {
        public MonsterColour Colour;
        public int Health;
        public int Attack;
        public float Scale;
        public int Count;
        public int MaxSpawn;
    }

    public EnemyGroup[] Groups;
    public float WaveTime;
}

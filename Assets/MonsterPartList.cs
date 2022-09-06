using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Part List", menuName = "ScriptableObjects/Monster Part List", order = 1)]
public class MonsterPartList : ScriptableObject
{
    public MonsterPart[] Parts;

    public MonsterPart RandomMonsterPart => Parts[Random.Range(0, Parts.Length)];
}

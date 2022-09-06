using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    [SerializeField] private Enemy m_monsterPrefab;
    [SerializeField] private MonsterPartList m_bodies;
    [SerializeField] private MonsterPartList m_arms;
    [SerializeField] private MonsterPartList m_legs;
    [SerializeField] private MonsterPartList m_ears;

    [SerializeField] private MonsterPartList m_eyes;
    [SerializeField] private MonsterPartList m_eyebrows;
    [SerializeField] private MonsterPartList m_mouths;
    [SerializeField] private MonsterPartList m_noses;
    public Enemy GenerateEnemy(MonsterColour variant)
    {
        Enemy enemy = Instantiate(m_monsterPrefab);
        MonsterBody body = Instantiate(m_bodies.RandomMonsterPart, enemy.VisualsContainer) as MonsterBody;

        body.LeftArm = Instantiate(m_arms.RandomMonsterPart, body.LeftArmTransform) as MonsterAppendage;
        body.RightArm = Instantiate(m_arms.RandomMonsterPart, body.RightArmTransform) as MonsterAppendage;

        body.LeftLeg = Instantiate(m_legs.RandomMonsterPart, body.LeftLegTransform) as MonsterAppendage;
        body.RightLeg = Instantiate(m_legs.RandomMonsterPart, body.RightLegTransform) as MonsterAppendage;

        body.LeftEar = Instantiate(m_ears.RandomMonsterPart, body.LeftEarTransform) as MonsterAppendage;
        body.RightEar = Instantiate(m_ears.RandomMonsterPart, body.RightEarTransform) as MonsterAppendage;


        body.LeftEye = Instantiate(m_eyes.RandomMonsterPart, body.LeftEyeTransform) as MonsterAccessory;
        body.RightEye = Instantiate(m_eyes.RandomMonsterPart, body.RightEyeTransform) as MonsterAccessory;

        body.LeftEyebrow = Instantiate(m_eyebrows.RandomMonsterPart, body.LeftEyebrowTransform) as MonsterAccessory;
        body.RightEyebrow = Instantiate(m_eyebrows.RandomMonsterPart, body.RightEyebrowTransform) as MonsterAccessory;

        body.Mouth = Instantiate(m_mouths.RandomMonsterPart, body.MouthTransform) as MonsterAccessory;
        body.Nose = Instantiate(m_noses.RandomMonsterPart, body.NoseTransform) as MonsterAccessory;

        body.SetVariant(variant);

        return enemy; 
    }
}

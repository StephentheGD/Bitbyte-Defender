using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBody : MonsterPart
{
    [Header("Transforms")]
    public Transform LeftArmTransform;
    public Transform RightArmTransform;

    public Transform LeftLegTransform;
    public Transform RightLegTransform;

    public Transform LeftEarTransform;
    public Transform RightEarTransform;

    public Transform LeftEyeTransform;
    public Transform RightEyeTransform;

    public Transform LeftEyebrowTransform;
    public Transform RightEyebrowTransform;

    public Transform MouthTransform;
    public Transform NoseTransform;

    [Header("Appendages")]
    public MonsterAppendage LeftArm;
    public MonsterAppendage RightArm;

    public MonsterAppendage LeftLeg;
    public MonsterAppendage RightLeg;

    public MonsterAppendage LeftEar;
    public MonsterAppendage RightEar;

    public MonsterAccessory LeftEye;
    public MonsterAccessory RightEye;

    public MonsterAccessory LeftEyebrow;
    public MonsterAccessory RightEyebrow;

    public MonsterAccessory Mouth;
    public MonsterAccessory Nose;

    public override void SetVariant(MonsterColour variant)
    {
        base.SetVariant(variant);
        LeftArm.SetVariant(variant);
        RightArm.SetVariant(variant);
        LeftLeg.SetVariant(variant);
        RightLeg.SetVariant(variant);
        LeftEar.SetVariant(variant);
        RightEar.SetVariant(variant);

        int randomEyes = Random.Range(0, 100);
        LeftEye?.SetRandom(randomEyes);
        RightEye?.SetRandom(randomEyes);

        int randomEyebrows = Random.Range(0, 100);
        LeftEyebrow?.SetRandom(randomEyebrows);
        RightEyebrow?.SetRandom(randomEyebrows);

        int randomMouth = Random.Range(0, 100);
        Mouth?.SetRandom(randomMouth);

        int randomNose = Random.Range(0, 100);
        Nose?.SetRandom(randomNose);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeClass { Blue, Yellow, Red, Purple };
[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade", order = 2)]
public class Upgrade : ScriptableObject
{
    public string Name;
    public string Description;
    public string EffectString;
    public Sprite Icon;
    public UpgradeClass Class;
    public int Effect;
    public int MaxUpgrades;
}

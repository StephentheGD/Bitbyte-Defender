using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade Group", menuName = "ScriptableObjects/Upgrade Group", order = 3)]
public class UpgradeGroup : ScriptableObject
{
    public UpgradeClass Class;
    public Upgrade[] Upgrades;
}

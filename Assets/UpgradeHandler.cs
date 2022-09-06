using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradeHandler
{
    public static float BlueDamageMultiplier = 1.5f;
    public static float BlueBulletAddition = 1f;
    public static float BlueReloadMultiplier = 0.8f;

    public static float YellowGunAddition = 1f;
    public static float YellowDamageMultiplier = 1.5f;
    public static float YellowReloadMultiplier = 0.9f;
    public static float YellowSpeedMultiplier = 1.2f;

    public static float RedMeleeAddition = 1f;
    public static float RedDamageMultiplier = 1.5f;
    public static float RedSizeMultiplier = 1.1f;
    public static float RedSpeedMultiplier = 1.2f;
    public static float RedCooldownMultiplier = 0.9f;

    public static float PurpleMoveMultiplier = 1.1f;
    public static float PurpleHealthAddition = 50f;
    public static float PurpleMagnetSizeAddition = 1.2f;
    public static float PurpleMagnetSpeedAddition = 1.2f;

    public static float GetUpgradedValue(UpgradeClass upgradeClass, int effect, float baseValue)
    {
        return (upgradeClass, effect) switch
        {
            (UpgradeClass.Blue, 0) => baseValue * Mathf.Pow(BlueDamageMultiplier, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect] + 1),
            (UpgradeClass.Blue, 1) => baseValue + Mathf.Pow(BlueBulletAddition, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect]) - BlueBulletAddition,
            (UpgradeClass.Blue, 2) => baseValue * Mathf.Pow(BlueReloadMultiplier, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect] + 1),

            (UpgradeClass.Yellow, 0) => baseValue * Mathf.Pow(YellowGunAddition, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect]) - YellowGunAddition,
            (UpgradeClass.Yellow, 1) => baseValue * Mathf.Pow(YellowDamageMultiplier, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect] + 1),
            (UpgradeClass.Yellow, 2) => baseValue * Mathf.Pow(YellowReloadMultiplier, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect] + 1),
            (UpgradeClass.Yellow, 3) => baseValue * Mathf.Pow(YellowSpeedMultiplier, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect] + 1),

            (UpgradeClass.Red, 0) => baseValue * Mathf.Pow(RedMeleeAddition, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect]) - RedMeleeAddition,
            (UpgradeClass.Red, 1) => baseValue * Mathf.Pow(RedDamageMultiplier, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect] + 1),
            (UpgradeClass.Red, 2) => baseValue * Mathf.Pow(RedSizeMultiplier, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect] + 1),
            (UpgradeClass.Red, 3) => baseValue * Mathf.Pow(RedSpeedMultiplier, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect] + 1),
            (UpgradeClass.Red, 4) => baseValue * Mathf.Pow(RedCooldownMultiplier, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect] + 1),

            (UpgradeClass.Purple, 0) => baseValue * Mathf.Pow(PurpleMoveMultiplier, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect] + 1),
            (UpgradeClass.Purple, 1) => baseValue + Mathf.Pow(PurpleHealthAddition, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect]) - PurpleHealthAddition,
            (UpgradeClass.Purple, 2) => baseValue * Mathf.Pow(PurpleMagnetSizeAddition, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect] + 1),
            (UpgradeClass.Purple, 3) => baseValue * Mathf.Pow(PurpleMagnetSpeedAddition, UpgradeManager.Instance.CurrentUpgrade(upgradeClass)[effect] + 1),

            _ => 0,
        };
    }
}

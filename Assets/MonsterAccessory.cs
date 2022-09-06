using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAccessory : MonsterPart
{
    public void SetRandom(int seed)
    {
        m_spriteRenderer.sprite = Variants[seed % Variants.Length];
    }
}

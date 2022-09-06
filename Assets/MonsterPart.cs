using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterColour { Green, Cyan, Red, Yellow, Black, White };

public class MonsterPart : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer m_spriteRenderer;
    public Sprite[] Variants;

    public virtual void SetVariant(MonsterColour variant)
    {
        m_spriteRenderer.sprite = Variants[(int)variant];
    }
}

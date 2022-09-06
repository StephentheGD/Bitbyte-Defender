using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType { Coin, BlueBit, YellowBit, RedBit, PurpleBit, GreenBit };
public class Collectable : MonoBehaviour
{
    public CollectableType CollectableType;
}

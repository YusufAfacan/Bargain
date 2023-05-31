using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AltarBoonGrant
{
    public string Name;

    public enum Type { Health, Armor }
    public Type type;

    public int duration;
    public int amount;
}

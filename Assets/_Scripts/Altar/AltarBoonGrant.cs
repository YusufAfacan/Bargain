using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AltarBoonGrant : ScriptableObject
{
    public string Name;

    public enum Type { GainHealth, GainArmor, GainStrength, SootheFire, SoothFear }
    public Type type;

    public int duration;
    public int amount;
}

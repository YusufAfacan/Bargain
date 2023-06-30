using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Suffix : ScriptableObject
{
    public string Name;
    public string Description;

    public enum ImbuedInto { OffensiveCard, DefensiveCard }
    public ImbuedInto imbuedInto;

    public enum MainType { HealthGrant, ArmorGrant, StrGrant, ApplyFire, ApplyFear, SootheFire, SootheFear }
    public MainType mainType;

    public int amount;
}

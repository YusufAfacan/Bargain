using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Prefix : ScriptableObject
{
    public string Name;
    public string Description;

    public enum MainType { HealthGrant, ArmorGrant, StrGrant, ApplyFire, ApplyFear, SootheFire, SootheFear }
    public MainType mainType;

    public int amount;


}


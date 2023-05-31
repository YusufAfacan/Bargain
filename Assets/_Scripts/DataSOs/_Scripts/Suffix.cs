using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Suffix : ScriptableObject
{
    public string Name;
    public string Description;

    public enum MainType { Offensive, Defensive }
    public MainType mainType;

    public enum ImbueType { Igni }
    public ImbueType Imbuetype;
}

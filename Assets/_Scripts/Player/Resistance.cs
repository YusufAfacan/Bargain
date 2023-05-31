using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Resistance
{
    public string Name;
    public string Description;
    public int currentAmount;
    public int maxResistance;
    public enum Type { Fire }
    public Type type;


}

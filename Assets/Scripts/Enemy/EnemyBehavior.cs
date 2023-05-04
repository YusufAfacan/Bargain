using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyBehavior
{
    [Range(0f, 100f)]
    public int weight;
    public enum Type { Attack, StrBuff, ArmorUp}
    public Type type;
    public float value;
    public float castTime;
    public int roll;
    public bool isSelected;
}

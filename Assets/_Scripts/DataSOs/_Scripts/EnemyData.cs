using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public string Name;
    public int baseHealth;
    public int baseArmor;
    public int BaseStrength;
    public int BaseOffence;
    public int BaseFireRes;
    public List<EnemyBehavior> behaviors;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public string Name;
    public float health;
    public float armor;
    public List<EnemyBehavior> behaviors;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class CardData : ScriptableObject
{

    public string Name;
    public string description;
    public Sprite background;
    public Sprite icon;

    public int castTime;


    public enum Attribute { None, Ephemeral, Anchored, Sunken }
    public Attribute attribute;

    public enum CardType { Attack, Skill, Item }
    public CardType cardType;
    public enum AttackType { None, Single, Area, Both, }
    public AttackType attackType;
    public enum AttackSubType { None, BasicAttack, Stunning }
    public AttackSubType attackSubType;

    public int singleTargetDamageAmount;
    public float areaDamageAmount;

    public enum SkillType { None, Buff, Debuff, Both }
    public SkillType skillType;

    public int armorAmount;
    public int healAmount;
    public int strAmount;
    
    
    public enum ItemType { None, OnDraw, Consumable }
    public ItemType itemType;

    

    

}

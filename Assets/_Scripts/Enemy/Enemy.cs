using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : MonoBehaviour
{
    
    public int difficultyCoefficent;
    public int aggressionRange = 1;
    private Vector3 target;
    
    private Player player;
    public bool followPlayer;

    public EnemyData data;
    private EnemyBehavior selectedBehaviour;

    public string Name;
    public float health;
    public float armor;
    public float str;
    public float nextBehaviourTime;
    public float actionDelay;
    public Resistance fireResistance;
    public Resistance stunResistance;
    public Resistance fearResistance;
    public Resistance slowResistance;
    public bool isBurning;
    public bool isStunned;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI nextBehaviourTimeText;
    private TextMeshProUGUI nextBehaviourIndicator;
    public GameObject healthGlobe;
    public GameObject expGlobe;


    
    

    private void Awake()
    {
        
        followPlayer = true;
    }


    private void Start()
    {
        player = FindObjectOfType<Player>();

        aggressionRange = 0;
        

        difficultyCoefficent = Mathf.RoundToInt(Vector3.Distance(transform.position, player.transform.position) / 3);

        health = data.baseHealth;
        armor = data.baseArmor;
        Name = data.Name;
        fireResistance.maxResistance = data.BaseFireRes;


        //UpdateHealthText();
        //UpdateArmorText();
        //UpdateNextBehaviourTimeText();
        //UpdateNextBehaviourIndicator();



    }

    private void UpdateNextBehaviourIndicator()
    {
        nextBehaviourIndicator.text = "";

        if (selectedBehaviour.type == EnemyBehavior.Type.Attack)
        {
            nextBehaviourIndicator.text += "Deal " + Mathf.RoundToInt(selectedBehaviour.value * str / 10).ToString() + "damage";
        }
    }

    void Update()
    {
        //target = player.transform.position;

        //if (Vector3.Distance(transform.position, target) < aggressionRange && followPlayer == true)
        //{
        //    //isFollowing = true;
        //    agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
        //}
        //else
        //{
        //    //isFollowing = false;
        //}
    }

    private EnemyBehavior SelectBehavior()
    {
        EnemyBehavior selectedBehaviour = null;

        List<int> weights = new();

        foreach (EnemyBehavior behaviour in data.behaviors)
        {
            weights.Add(behaviour.weight);
        }

        int totalWeight = 0;

        foreach (int weight in weights)
        {
            totalWeight += weight;
        }

        int rndWeightValue = UnityEngine.Random.Range(1, totalWeight + 1);

        //Checking where random weight value falls
        int processedWeight = 0;

        foreach (EnemyBehavior behaviour in data.behaviors)
        {
            processedWeight += behaviour.weight;

            if (rndWeightValue <= processedWeight)
            {
                selectedBehaviour = behaviour;
                break;
            }
        }


        return selectedBehaviour;

    }


    public void TakeDamage(float amount)
    {
        if (amount <= armor)
        {
            armor -= amount;
            UpdateArmorText();
        }
        else
        {
            health -= amount - armor;
            armor = 0;
            UpdateArmorText();

            if (health <= 0)
            {
                health = 0;
                //Die();
            }
            UpdateHealthText();
        }
    }

    //public void ApplyCondition(Suffix.Subtype imbueType, int damageOutput)
    //{
    //    if (imbueType == Suffix.Subtype.Igni)
    //    {
    //        fireResistance.currentAmount += damageOutput;

    //        if (fireResistance.currentAmount >= fireResistance.maxResistance)
    //        {
    //            isBurning = true;
    //        }
    //    }


    //}

    //public void ApplyCondition(Prefix.Subtype imbueType, int damageOutput)
    //{
    //    if (imbueType == Prefix.Subtype.Devastating)
    //    {
    //        stunResistance.currentAmount += damageOutput;

    //        if (stunResistance.currentAmount >= stunResistance.maxResistance)
    //        {
    //            isStunned = true;
    //        }
    //    }
    //}

    public void Die()
    {
        gameObject.SetActive(false);
        Instantiate(healthGlobe, transform.position, Quaternion.identity);
        Instantiate(expGlobe, transform.position, Quaternion.identity);
        
        
    }

    public void ReduceNextAttackTime(float amount)
    {
        nextBehaviourTime -= amount;
        UpdateNextBehaviourTimeText();

        if (nextBehaviourTime <= 0)
        {
            Behave();
        }
    }


    private void Behave()
    {
        if (selectedBehaviour.type == EnemyBehavior.Type.Attack)
        {
            player.TakeDamage(Mathf.RoundToInt(selectedBehaviour.value * str / 10));
        }

        else if (selectedBehaviour.type == EnemyBehavior.Type.StrBuff)
        {
            str += selectedBehaviour.value;
        }

        else if (selectedBehaviour.type == EnemyBehavior.Type.ArmorUp)
        {
            armor += selectedBehaviour.value;
            UpdateArmorText();


        }

        DeclareNewBehavior();


    }

    public void UpdateArmorText()
    {
        armorText.text = armor.ToString();
    }

    public void DeclareNewBehavior()
    {
        selectedBehaviour = SelectBehavior();
        nextBehaviourTime = selectedBehaviour.castTime;
        UpdateNextBehaviourTimeText();
        UpdateNextBehaviourIndicator();
    }




    public void UpdateHealthText()
    {
        healthText.text = health.ToString();
    }

    public void UpdateNextBehaviourTimeText()
    {
        nextBehaviourTimeText.text = nextBehaviourTime.ToString();
    }

    public void GetReadyForBattle()
    {
        healthText = BattleUI.Instance.healthText;
        UpdateHealthText();
        armorText = BattleUI.Instance.armorText;
        UpdateArmorText();
        nextBehaviourTimeText = BattleUI.Instance.nextBehaviourTimeText;
        UpdateNextBehaviourTimeText();
        nextBehaviourIndicator = BattleUI.Instance.nextBehaviourIndicator;
        DeclareNewBehavior();
    }
}



using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    private EnemyBehavior selectedBehaviour;
    public string Name;
    public float health;
    public float armor;
    public float nextAttackTime;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI nextAttackTimeText;

    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();

        health = data.health;
        armor = data.armor;
        Name = data.Name;
        selectedBehaviour = SelectBehaviour();
        nextAttackTime = selectedBehaviour.castTime;


        UpdateHealthText();
        UpdateNextAttackTimeText();


        
    }

    private EnemyBehavior SelectBehaviour()
    {
        int highestRoll = 0;

        foreach (EnemyBehavior behavior in data.behaviors)
        {
            behavior.roll = UnityEngine.Random.Range(0, behavior.weight);

            if(highestRoll < behavior.roll)
            {
                highestRoll = behavior.roll;

                foreach (EnemyBehavior otherbehaviors in data.behaviors)
                {
                    otherbehaviors.isSelected = false;
                }

                behavior.isSelected = true;
            }
        }

        for (int i = 0; i < data.behaviors.Count; i++)
        {
            if (data.behaviors[i].isSelected)
            {
                return data.behaviors[i];
            }
        }

        Debug.Log("Problem is here.");
        return null;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        UpdateHealthText();
    }

    public void ReduceNextAttackTime(float amount)
    {
        nextAttackTime -= amount;

        if (nextAttackTime <= 0)
        {
            Behave();
        }

    }

    private void Behave()
    {

    }










    private IEnumerator yea()
    {
        player.canPlayCard = false;
        player.health -= 10;
        yield return new WaitForSeconds(3);
        player.canPlayCard = true;
    }

    public void UpdateHealthText()
    {
        healthText.text = health.ToString();
    }

    public void UpdateNextAttackTimeText()
    {
        nextAttackTimeText.text = nextAttackTime.ToString();
    }

    

}


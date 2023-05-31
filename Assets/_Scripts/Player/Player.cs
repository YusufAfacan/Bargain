using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private readonly int movementSpeed = 5;
    private readonly int handSize = 4;
    
    public int ownedCommonToken;
    public Slider expBar;



    public bool engagedBattle = false;
    public bool canPlayCard = true;
    public bool canMove = true;
    public bool playerTurn = true;

    public Stat health;
    public Stat armor;
    public Stat str;
    public Stat offence;
    public Stat defence;

    public Transform playerDeck;
    public Transform playerHand;
    public Transform discardPile;

    public List<CardData> ownedCardsData;
    public GameObject cardBasePrefab;


    public GameObject battleUI;
    private Enemy targetEnemy;
    private Camera cam;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armorText;

    public AltarPanel altarPanel;
    public Altar contactedAltar;


    private void Start()
    {
        cam = Camera.main;
        CreateDeck();
        UpdateHealthText();
        UpdateArmorText();
        
    }

    public void CreateDeck()
    {
        foreach (CardData cardData in ownedCardsData)
        {
            GameObject cardObj = Instantiate(cardBasePrefab, playerDeck.position, Quaternion.identity);
            cardObj.transform.SetParent(playerDeck);
            cardObj.transform.localScale = Vector3.one; 

            Card card = cardObj.GetComponent<Card>();
            card.cardData = cardData;

            card.Name.text = cardData.Name;
            card.castTime.text = cardData.castTime.ToString();
            cardObj.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            targetVelocity *= movementSpeed * Time.fixedDeltaTime;
            Vector2 pos = transform.position;
            pos += targetVelocity;
            transform.position = pos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() == true && engagedBattle == false)
        {
            targetEnemy = collision.gameObject.GetComponent<Enemy>();
            CommenceBattle(targetEnemy);
        }

        if (collision.gameObject.GetComponent<Altar>() == true && engagedBattle == false)
        {
            altarPanel.gameObject.SetActive(true);
            Altar altar = collision.gameObject.GetComponent<Altar>();

            canMove = false;
            contactedAltar = altar;
            contactedAltar.Interact();
        }
    }

    private void CommenceBattle(Enemy enemy)
    {
        battleUI.SetActive(true);
        engagedBattle = true;
        canMove = false;
        FillHand();
        enemy.GetReadyForBattle();
        
        cam.GetComponent<CameraController>().Zoom(enemy);
   
    }

    public void FillHand()
    {
        for (int i = 0; i < handSize; i++)
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        playerDeck.GetChild(0).gameObject.SetActive(true);
        playerDeck.GetChild(0).transform.SetParent(playerHand);

        if (playerDeck.childCount <= 0)
        {
            DiscardPileToDeck();
        }
    }

    private void DiscardPileToDeck()
    {
        for (int i = 0; i < discardPile.childCount; i++)
        {
            discardPile.GetChild(Random.Range(0, discardPile.childCount)).SetParent(playerDeck);
        }
    }

    private void Update()
    {
        if (engagedBattle)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayCard(playerHand.GetChild(0).GetComponent<Card>());
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayCard(playerHand.GetChild(1).GetComponent<Card>());
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayCard(playerHand.GetChild(2).GetComponent<Card>());
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PlayCard(playerHand.GetChild(3).GetComponent<Card>());
            }
        }
    }

    private void PlayCard(Card card)
    {
        if (card.cardData.cardType == CardData.CardType.Attack)
        {
            if (card.cardData.attackType == CardData.AttackType.Single)
            {
                int damageOutput = DamageOutput(card.cardData.singleTargetDamageAmount);

                targetEnemy.TakeDamage(damageOutput);

                if (card.suffix != null && card.suffix.mainType == Suffix.MainType.Offensive)
                {
                    if (card.suffix.Imbuetype == Suffix.ImbueType.Igni)
                    {
                        targetEnemy.ApplyCondition(Suffix.ImbueType.Igni, damageOutput);
                    }
                }
            }
        }

        CheckSkills(card);

        if(targetEnemy.health > 0)
        {
            DiscardHand();
            FillHand();
            targetEnemy.ReduceNextAttackTime(card.cardData.castTime);
            
        }
        else if (targetEnemy.health <= 0 || targetEnemy == null)
        {
            targetEnemy.Die();
            DiscardHand();
            WinBattle();
        }
    }

    private void CheckSkills(Card card)
    {
        if (card.cardData.healAmount >= 0)
        {
            health.value += card.cardData.healAmount;
            UpdateHealthText();
        }

        if (card.cardData.armorAmount >= 0)
        {
            armor.value += card.cardData.armorAmount;
            UpdateArmorText();
        }

        if (card.cardData.strAmount >= 0)
        {
            str.value += card.cardData.strAmount;
            //player.UpdateArmorText();
        }
    }


    public void DiscardHand()
    {
        for (int i = 0; i < handSize; i++)
        {
            playerHand.GetChild(0).gameObject.SetActive(false);
            playerHand.GetChild(0).transform.SetParent(discardPile);

        }
    }

    public void WinBattle()
    {
        cam.GetComponent<CameraController>().Unzoom();
        engagedBattle = false;
        canMove = true;

        battleUI.SetActive(false);
        DiscardPileToDeck();
        HandToDeck();

    }

    private void HandToDeck()
    {
        for (int i = 0; i < playerHand.childCount; i++)
        {
            playerHand.GetChild(0).SetParent(playerDeck);
        }
    }

    public int DamageOutput(int carddamage)
    {
        int damage = (carddamage + offence.value) * (str.value / 10);

        return Mathf.RoundToInt(damage);
    }

    public void UpdateHealthText()
    {
        healthText.text = health.value.ToString();
    }

    public void UpdateArmorText()
    {
        armorText.text = armor.value.ToString();
    }

    public void TakeDamage(int amount)
    {
        if (amount <= armor.value)
        {
            armor.value -= amount;
            UpdateArmorText();
        }
        else
        {
            health.value -= amount - armor.value;
            armor.value = 0;
            UpdateArmorText();

            if (health.value < 0)
            {
                health.value = 0;
                Die();
            }
            UpdateHealthText();
        }

    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    public void GainExp(int amount)
    {
        if (amount < expBar.maxValue - expBar.value)
        {
            expBar.value += amount;
        }
        else if( amount == expBar.maxValue - expBar.value)
        {
            LevelUp();
            expBar.value = 0;
        }
        else
        {
            LevelUp();

            int excessExp = amount - ((int)expBar.maxValue - (int)expBar.value);
            expBar.value = 0;
            GainExp(excessExp);
        }

    }

    private void LevelUp()
    {
        ownedCommonToken++;
    }
}

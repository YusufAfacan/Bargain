using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour//, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    

    private Player player;
    private Vector3 startPosition;
    private Transform startParent;
    private Transform discardPile;
    private Transform battleCanvas;

    public Prefix prefix;
    public CardData cardData;
    public Suffix suffix;

    public TextMeshProUGUI Name;
    public TextMeshProUGUI description;
    public Image background;
    public Image icon;
    public TextMeshProUGUI castTime;


    private void Start()
    {
        //battleCanvas = FindObjectOfType<BattleResolver>().battleCanvas.transform;
        
        player = FindObjectOfType<Player>();

        //Name.text = cardData.Name;
        SetDescription();
        //description.text = cardData.description;
        //background.sprite = cardData.background;
        //icon.sprite = cardData.icon;
        //castTime.text = cardData.castTime.ToString();

    }

    private void SetDescription()
    {
        description.text = "";

        if(cardData.cardType == CardData.CardType.Attack)
        {
            description.text += "Deal";
        }

        if (cardData.attackType == CardData.AttackType.Single)
        {
            description.text += " " + player.DamageOutput(cardData.singleTargetDamageAmount).ToString() + " damage";
        }

        if (cardData.healAmount > 0)
        {
            description.text += "\nHeal " + cardData.healAmount.ToString();
        }

        if (cardData.armorAmount > 0)
        {
            description.text += "\nGain " + cardData.armorAmount.ToString() + " armor";
        }

        if (cardData.cardType == CardData.CardType.Skill)
        {
            description.text = cardData.description.ToString();
        }
    }


    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    if (player.canPlayCard)
    //    {
    //        startPosition = transform.position;
    //        startParent = transform.parent;

    //        // disable raycasting on the card so it doesn't interfere with other drag operations
    //        GetComponent<CanvasGroup>().blocksRaycasts = false;

    //        // set the card's parent to the canvas so it can be dragged across the entire screen
    //        transform.SetParent(transform.root);
    //    }
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    if (player.canPlayCard)
    //    {
    //        // move the card to the pointer's position
    //        transform.position = eventData.position;
    //    }
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
        

    //    if (player.canPlayCard)
    //    {
    //        if (eventData.pointerEnter == null)
    //        {
    //            transform.position = startPosition;
    //            transform.SetParent(startParent);
    //        }

    //        else if (cardData.cardType == CardData.CardType.Attack && eventData.pointerEnter.GetComponent<Enemy>())
    //        {
    //            Debug.Log(eventData.pointerClick.name);


    //            Enemy enemy = eventData.pointerEnter.GetComponent<Enemy>();

    //            if (cardData.attackType == CardData.AttackType.Both)
    //            {
    //                enemy.TakeDamage(cardData.singleTargetDamageAmount);

    //                Enemy[] enemyList = FindObjectsOfType<Enemy>();

    //                foreach (Enemy enemies in enemyList)
    //                {
    //                    enemies.TakeDamage(cardData.areaDamageAmount);
    //                    enemies.ReduceNextAttackTime(cardData.castTime);
    //                }

    //                CheckSkills(player);
                    
    //                player.DiscardHand();
    //                Discard();
    //            }

    //            else if (cardData.attackType == CardData.AttackType.Single)
    //            {
    //                enemy.TakeDamage(player.DamageOutput(cardData.singleTargetDamageAmount));
    //                enemy.ReduceNextAttackTime(cardData.castTime);
    //                CheckSkills(player);
                    
    //                player.DiscardHand();
    //                Discard();
    //            }

    //            else if (cardData.attackType == CardData.AttackType.Area)
    //            {
    //                Enemy[] enemyList = FindObjectsOfType<Enemy>();

    //                foreach (Enemy enemies in enemyList)
    //                {
    //                    enemies.TakeDamage(cardData.areaDamageAmount);
    //                    enemies.ReduceNextAttackTime(cardData.castTime);
    //                }

    //                CheckSkills(player);
                    
    //                player.DiscardHand();
    //                Discard();
    //            }

    //            else
    //            {
    //                Debug.Log("there");
    //                transform.position = startPosition;
    //                transform.SetParent(startParent);
    //            }

    //        }

    //        else if (cardData.cardType == CardData.CardType.Skill && eventData.pointerEnter.GetComponent<Player>())
    //        {

                
    //            CheckSkills(player);

    //            Enemy[] enemyList = FindObjectsOfType<Enemy>();

    //            foreach (Enemy enemies in enemyList)
    //            {

    //                enemies.ReduceNextAttackTime(cardData.castTime);
    //            }


    //            player.DiscardHand();
    //            Discard();
    //        }

    //        else
    //        {
    //            transform.position = startPosition;
    //            transform.SetParent(startParent);
    //        }

    //        // re-enable raycasting on the card
    //        GetComponent<CanvasGroup>().blocksRaycasts = true;

    //    }

    //}

    //private void CheckSkills(Player player)
    //{
    //    if (cardData.healAmount >= 0)
    //    {
    //        player.health += cardData.healAmount;
    //        player.UpdateHealthText();
    //    }

    //    if (cardData.armorAmount >= 0)
    //    {
    //        player.armor += cardData.armorAmount;
    //        player.UpdateArmorText();
    //    }

    //    if (cardData.strAmount >= 0)
    //    {
    //        player.str += cardData.strAmount;
    //        //player.UpdateArmorText();
    //    }
    //}

    //public void Discard()
    //{
    //    transform.SetParent(discardPile);
    //    gameObject.SetActive(false);
    //}

    
}


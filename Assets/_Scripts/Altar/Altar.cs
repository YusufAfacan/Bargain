using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Unity.VisualScripting;

public class Altar : MonoBehaviour
{
    private Player player;

    public enum AltarType { Red , Black, Blue};

    public AltarType altarType;

    public enum OfferType { Boon, CardGrant, Prefix, Suffix };

    public OfferType offerType;

    public TextMeshProUGUI NameText;

    private AltarPanel altarPanel;
    public GameObject cardBasePrefab;

    private CardData cardToGrant;
    private Prefix prefixToGrant;
    private Suffix suffixToGrant;
    private AltarBoonGrant boonToGrant;

    public List<Prefix> RedAltarPrefixesToGrant;
    public List<Suffix> RedAltarSuffixesToGrant;
    public List<AltarBoonGrant> RedAltarBoonToGrant;
    public List<CardData> redAltarCardsToGrant;

    public List<Prefix> BlackAltarPrefixesToGrant;
    public List<Suffix> BlackAltarSuffixesToGrant;
    public List<AltarBoonGrant> BlackAltarBoonToGrant;
    public List<CardData> blackAltarCardsToGrant;

    public List<Prefix> BlueAltarPrefixesToGrant;
    public List<Suffix> BlueAltarSuffixesToGrant;
    public List<AltarBoonGrant> BlueAltarBoonToGrant;
    public List<CardData> blueAltarCardsToGrant;

    

    



    // Start is called before the first frame update
    void Start()
    {
        altarType = (AltarType)Random.Range(0, Enum.GetNames(typeof(AltarType)).Length);
        altarPanel = FindObjectOfType<AltarPanel>();
        player = FindObjectOfType<Player>();
    }

    public void Interact()
    {
        if (altarType == AltarType.Red)
        {
            RedInteract();
        }

        if (altarType == AltarType.Black)
        {
            BlackInteract();
        }

        if (altarType == AltarType.Blue)
        {
            BlueInteract();
        }
    }

    

    private void RedInteract()
    {
        
        OfferType offer = (OfferType)Random.Range(0, Enum.GetNames(typeof(OfferType)).Length);

        if (offer == OfferType.Boon)
        {
            altarPanel.boonPanel.gameObject.SetActive(true);

            boonToGrant = RedAltarBoonToGrant[Random.Range(0, RedAltarBoonToGrant.Count)];

            altarPanel.boonPanel.descriptionText.text = "Gain " + boonToGrant.amount.ToString() + " " + boonToGrant.type;
        }

        if (offer == OfferType.CardGrant)
        {
            altarPanel.cardGrantPanel.gameObject.SetActive(true);
            

            cardToGrant = redAltarCardsToGrant[Random.Range(0, redAltarCardsToGrant.Count)];

            altarPanel.cardGrantPanel.cardName.text = cardToGrant.name;
            altarPanel.cardGrantPanel.cardDescription.text = cardToGrant.description;


        }

        if( offer == OfferType.Prefix)
        {
            altarPanel.imbuePanel.gameObject.SetActive(true);

            prefixToGrant = RedAltarPrefixesToGrant[Random.Range(0, RedAltarPrefixesToGrant.Count)];

            //altarPanel.imbuePanel.descriptionText.text = "Imbue a random card with " + prefixToGrant.subtype;

        }

        if (offer == OfferType.Suffix)
        {
            altarPanel.imbuePanel.gameObject.SetActive(true);

            suffixToGrant = RedAltarSuffixesToGrant[Random.Range(0, RedAltarSuffixesToGrant.Count)];

            //altarPanel.imbuePanel.descriptionText.text = "Imbue a random card with " + suffixToGrant.subType;

        }

    }


    private void BlackInteract()
    {
        OfferType offer = (OfferType)Random.Range(0, Enum.GetNames(typeof(OfferType)).Length);

        if (offer == OfferType.Boon)
        {
            altarPanel.boonPanel.gameObject.SetActive(true);

            boonToGrant = BlackAltarBoonToGrant[Random.Range(0, BlackAltarBoonToGrant.Count)];

            altarPanel.boonPanel.descriptionText.text = "Gain " + boonToGrant.amount.ToString() + " " + boonToGrant.type;
        }

        if (offer == OfferType.CardGrant)
        {
            altarPanel.cardGrantPanel.gameObject.SetActive(true);


            cardToGrant = blackAltarCardsToGrant[Random.Range(0, blackAltarCardsToGrant.Count)];

            altarPanel.cardGrantPanel.cardName.text = cardToGrant.name;
            altarPanel.cardGrantPanel.cardDescription.text = cardToGrant.description;


        }

        if (offer == OfferType.Prefix)
        {
            altarPanel.imbuePanel.gameObject.SetActive(true);

            prefixToGrant = BlackAltarPrefixesToGrant[Random.Range(0, BlackAltarPrefixesToGrant.Count)];

            //altarPanel.imbuePanel.descriptionText.text = "Imbue a random card with " + prefixToGrant.subtype;

        }

        if (offer == OfferType.Suffix)
        {
            altarPanel.imbuePanel.gameObject.SetActive(true);

            suffixToGrant = BlackAltarSuffixesToGrant[Random.Range(0, BlackAltarSuffixesToGrant.Count)];

            //altarPanel.imbuePanel.descriptionText.text = "Imbue a random card with " + suffixToGrant.subType;

        }
    }

    private void BlueInteract()
    {
        OfferType offer = (OfferType)Random.Range(0, Enum.GetNames(typeof(OfferType)).Length);

        if (offer == OfferType.Boon)
        {
            altarPanel.boonPanel.gameObject.SetActive(true);

            boonToGrant = BlueAltarBoonToGrant[Random.Range(0, BlueAltarBoonToGrant.Count)];

            altarPanel.boonPanel.descriptionText.text = "Gain " + boonToGrant.amount.ToString() + " " + boonToGrant.type;
        }

        if (offer == OfferType.CardGrant)
        {
            altarPanel.cardGrantPanel.gameObject.SetActive(true);


            cardToGrant = blueAltarCardsToGrant[Random.Range(0, blueAltarCardsToGrant.Count)];

            altarPanel.cardGrantPanel.cardName.text = cardToGrant.name;
            altarPanel.cardGrantPanel.cardDescription.text = cardToGrant.description;


        }

        if (offer == OfferType.Prefix)
        {
            altarPanel.imbuePanel.gameObject.SetActive(true);

            prefixToGrant = BlueAltarPrefixesToGrant[Random.Range(0, BlueAltarPrefixesToGrant.Count)];

            //altarPanel.imbuePanel.descriptionText.text = "Imbue a random card with " + prefixToGrant.subtype;

        }

        if (offer == OfferType.Suffix)
        {
            altarPanel.imbuePanel.gameObject.SetActive(true);

            suffixToGrant = BlueAltarSuffixesToGrant[Random.Range(0, BlueAltarSuffixesToGrant.Count)];

            //altarPanel.imbuePanel.descriptionText.text = "Imbue a random card with " + suffixToGrant.subType;

        }
    }

    public void Imbue()
    {
        
        if (suffixToGrant.imbuedInto == Suffix.ImbuedInto.OffensiveCard)
        {
            if(AttackCardsWithoutSuffix().Count > 0)
            {
                RandomAttackCardWithoutSuffix().suffix = suffixToGrant;
            }
            
        }

        altarPanel.imbuePanel.gameObject.SetActive(false);
        
        
        player.canMove = true;
    }

    public void GrantBoon()
    {

        if (boonToGrant.type == AltarBoonGrant.Type.GainHealth)
        {
            player.health.value += boonToGrant.amount;
            player.UpdateHealthText();
        }

        if (boonToGrant.type == AltarBoonGrant.Type.GainArmor)
        {
            player.armor.value += boonToGrant.amount;
        }

        
        altarPanel.boonPanel.gameObject.SetActive(false);
        
        
        player.canMove = true;
    }

    public void GrantCard()
    {
        player.ownedCardsData.Add(cardToGrant);

        GameObject cardObj = Instantiate(cardBasePrefab, player.playerDeck.position, Quaternion.identity);
        cardObj.transform.SetParent(player.playerDeck);
        cardObj.transform.localScale = Vector3.one;

        Card card = cardObj.GetComponent<Card>();
        card.cardData = cardToGrant;

        card.Name.text = cardToGrant.Name;
        card.castTime.text = cardToGrant.castTime.ToString();
        cardObj.SetActive(false);

        
        altarPanel.cardGrantPanel.gameObject.SetActive(false);
        
        
        player.canMove = true;
    }

    private List<Card> CardsOfPlayer()
    {
        List<Card> cardsOfPlayer = new List<Card>();

        for (int i = 0; i < player.playerDeck.childCount; i++)
        {
            cardsOfPlayer.Add(player.playerDeck.GetChild(i).GetComponent<Card>());
        }

        for (int i = 0; i < player.discardPile.childCount; i++)
        {
            cardsOfPlayer.Add(player.discardPile.GetChild(i).GetComponent<Card>());
        }

        return cardsOfPlayer;
    }

    private List<Card> AttackCardsWithoutSuffix()
    {
        List<Card> attackCardsWithoutSuffix = new List<Card>();

        foreach (Card card in CardsOfPlayer())
        {

            if (card.cardData.cardType == CardData.CardType.Attack && card.suffix == null)
            {
                attackCardsWithoutSuffix.Add(card);
            }
        }

        return attackCardsWithoutSuffix;

    }

    private Card RandomAttackCardWithoutSuffix()
    {

        if (AttackCardsWithoutSuffix().Count > 0)
        {
            return AttackCardsWithoutSuffix()[Random.Range(0, AttackCardsWithoutSuffix().Count)];
        }
        else
        {
            return null;
        }

    }

    
}

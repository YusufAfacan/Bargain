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

    public enum AltarType { Red };

    public AltarType altarType;

    public enum OfferType { Boon, CardGrant, Imbue };

    public OfferType offerType;

    public TextMeshProUGUI NameText;

    public List<CardData> cardsToGrant;

    private AltarPanel altarPanel;
    private ImbuePanel imbuePanel;
    private CardGrantPanel cardGrantPanel;
    private BoonPanel boonPanel;


    public List<CardData> redAltarCardsToGrant;
    public CardData cardToGrant;

    public List<Suffix> RedAltarSuffixsToGrant;
    public List<AltarBoonGrant> RedAltarBoonToGrant;

    public Suffix suffixToGrant;
    public AltarBoonGrant boonToGrant;

    public GameObject cardBasePrefab;



    // Start is called before the first frame update
    void Start()
    {
        altarType = (AltarType)Random.Range(0, Enum.GetNames(typeof(AltarType)).Length);
        
        player = FindObjectOfType<Player>();
    }

    public void Interact()
    {
        if (altarType == AltarType.Red)
        {
            RedInteract();
        }
    }

    private void RedInteract()
    {
        altarPanel = FindObjectOfType<AltarPanel>();

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
            cardGrantPanel = FindObjectOfType<CardGrantPanel>();

            cardToGrant = redAltarCardsToGrant[Random.Range(0, cardsToGrant.Count)];

            cardGrantPanel.cardName.text = cardToGrant.name;
            cardGrantPanel.cardDescription.text = cardToGrant.description;


        }

        if( offer == OfferType.Imbue)
        {
            altarPanel.imbuePanel.gameObject.SetActive(true);

            suffixToGrant = RedAltarSuffixsToGrant[Random.Range(0, RedAltarSuffixsToGrant.Count)];

            altarPanel.imbuePanel.descriptionText.text = "Imbue a random card with " + suffixToGrant.Imbuetype;

        }

       
        
    }

    public void Imbue()
    {
        
        if (suffixToGrant.mainType == Suffix.MainType.Offensive )
        {
            RandomAttackCardWithoutSuffix().suffix = suffixToGrant;
        }

        imbuePanel = FindObjectOfType<ImbuePanel>();
        imbuePanel.gameObject.SetActive(false);
        altarPanel = FindObjectOfType<AltarPanel>();
        altarPanel.gameObject.SetActive(false);
        player.canMove = true;
    }

    public void GrantBoon()
    {

        if (boonToGrant.type == AltarBoonGrant.Type.Health)
        {
            player.health.value += boonToGrant.amount;
        }

        if (boonToGrant.type == AltarBoonGrant.Type.Armor)
        {
            player.armor.value += boonToGrant.amount;
        }

        boonPanel = FindObjectOfType<BoonPanel>();
        boonPanel.gameObject.SetActive(false);
        altarPanel = FindObjectOfType<AltarPanel>();
        altarPanel.gameObject.SetActive(false);
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

        
        cardGrantPanel.gameObject.SetActive(false);
        altarPanel = FindObjectOfType<AltarPanel>();
        altarPanel.gameObject.SetActive(false);
        player.canMove = true;
    }

    private Card RandomAttackCardWithoutSuffix()
    {
        List<Card> cardsOfPlayer = new List<Card>();

        List<Card> attackCardsWithoutSuffix = new List<Card>();

        for (int i = 0; i < player.playerDeck.childCount; i++)
        {
            cardsOfPlayer.Add(player.playerDeck.GetChild(i).GetComponent<Card>());
        }

        foreach (Card card in cardsOfPlayer)
        {

            if (card.cardData.cardType == CardData.CardType.Attack && card.suffix == null)
            {
                attackCardsWithoutSuffix.Add(card);
            }
        }

        return attackCardsWithoutSuffix[Random.Range(0, attackCardsWithoutSuffix.Count)];

        
    }

    
}

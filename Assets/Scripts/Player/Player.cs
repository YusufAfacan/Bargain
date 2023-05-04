using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canPlayCard = true;

    public float health;
    public float armor;
    public float str;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armorText;


    public Transform playerDeck;
    public Transform discardPile;

    public Transform playerHand;

    public int handSize = 4;
    public bool playerTurn = true;

    private void Start()
    {
        discardPile = FindObjectOfType<DiscardPile>().transform;

        FillHand();
        UpdateHealthText();
        UpdateArmorText();
    }

    public void UpdateHealthText()
    {
        healthText.text = health.ToString();
    }

    public void UpdateArmorText()
    {
        armorText.text = armor.ToString();
    }

    public void DiscardHand()
    {
        while (playerHand.childCount > 0)
        {
            playerHand.GetChild(0).GetComponent<Card>().Discard();
        }

        FillHand();
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
            ReloadDeck();
        }
    }

    private void ReloadDeck()
    {
        while (discardPile.childCount > 0)
        {
            int index = Random.Range(0, discardPile.childCount);
            discardPile.GetChild(index).SetParent(playerDeck);
        }


    }
}

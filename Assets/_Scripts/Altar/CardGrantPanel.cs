using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardGrantPanel : MonoBehaviour
{
    

    public TextMeshProUGUI descriptionText;
    public Button button;
    private Player player;
    public Altar activeAltar;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDescription;

    private void OnEnable()
    {
        player = FindObjectOfType<Player>();
        activeAltar = player.contactedAltar;
        
        
    }
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(activeAltar.GrantCard);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

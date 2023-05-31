using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoonPanel : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;
    public Button button;
    private Player player;
    public Altar activeAltar;

    private void OnEnable()
    {
        player = FindObjectOfType<Player>();
        activeAltar = player.contactedAltar;
        button.onClick.AddListener(activeAltar.GrantBoon);
        
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

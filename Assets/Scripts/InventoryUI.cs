using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI textHoney;
    // Start is called before the first frame update
    void Start()
    {
       textHoney = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void UpdateTextHoney(PlayerInventory playerInventory)
    {
        textHoney.text = playerInventory.NumberOfHoney.ToString();

    }
   
}

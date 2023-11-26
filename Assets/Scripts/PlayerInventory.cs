using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
   public int NumberOfHoney {  get; private set; }
    public UnityEvent<PlayerInventory> OnHoneyCollected;

    public void HoneyCollected()
    {
        NumberOfHoney++;
        OnHoneyCollected.Invoke(this);
    }

}

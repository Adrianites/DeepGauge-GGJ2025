using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public string itemName;
    public int groupID; // The group the item belongs to (if you're using groups)
    public bool isPickedUp = false;

    // This method handles the pickup of the item
    public void PickUp()
    {
        // If you want to do something when the item is picked up, like play an animation or sound
        isPickedUp = true;

        // Optionally disable the item visually
        gameObject.SetActive(false);

        // Handle winning conditions in the GameManager
        GameManager.Instance.AddItemToInventory(this);
    }
}

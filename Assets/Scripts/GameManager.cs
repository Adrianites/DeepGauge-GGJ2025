using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Track picked up items
    private List<Items> collectedItems = new List<Items>();

    // Track how many items are needed to win (by group or total)
    public int totalItemsNeeded = 5;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItemToInventory(Items items)
    {
        if (!collectedItems.Contains(items))
        {
            collectedItems.Add(items);
        }

        // Check for win condition
        if (collectedItems.Count >= totalItemsNeeded)
        {
            WinLevel();
        }
    }

    private void WinLevel()
    {
        // Handle win condition, e.g. show UI, load next level, etc.
        Debug.Log("Level Completed!");
    }
}

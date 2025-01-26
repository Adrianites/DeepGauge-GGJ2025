using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs; // Array of item prefabs to spawn
    public Transform[] spawnLocations; // Array of spawn locations
    public int groupIDToSpawn; // Group ID of items to spawn
    public int numberOfItemsToSpawn = 5; // Number of items to spawn

    private void SpawnItems()
    {
        // Get all items that belong to the selected group
        GameObject[] itemsInGroup = GetItemsByGroupID(groupIDToSpawn);

        if (itemsInGroup.Length == 0)
        {
            Debug.Log("No items found in Group ID: " + groupIDToSpawn);
            return;
        }

        // Shuffle the spawn locations to ensure randomness
        Shuffle(spawnLocations);

        // Spawn items at each spawn location
        for (int i = 0; i < Mathf.Min(numberOfItemsToSpawn, spawnLocations.Length); i++)
        {
            // Randomly pick an item from the items in the selected group
            int randomIndex = Random.Range(0, itemsInGroup.Length);

            // Instantiate the item at the spawn location
            Instantiate(itemsInGroup[randomIndex], spawnLocations[i].position, Quaternion.identity);
        }
    }

    // Method to filter items based on group ID
    private GameObject[] GetItemsByGroupID(int groupID)
    {
        // List to store items in the selected group
        List<GameObject> itemsInGroup = new List<GameObject>();

        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            // Check if the item's group matches the group ID we're looking for
            Items itemScript = itemPrefabs[i].GetComponent<Items>();
            if (itemScript != null && itemScript.groupID == groupID)
            {
                itemsInGroup.Add(itemPrefabs[i]);
            }
        }

        return itemsInGroup.ToArray();
    }

    // Method to shuffle an array
    private void Shuffle(Transform[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Transform temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    private void Start()
    {
        // Call the SpawnItems method when the game starts
        SpawnItems();
    }
}
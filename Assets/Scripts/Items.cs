using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public string itemName;
    public int groupID; // The group the item belongs to (if you're using groups)
    public bool isPickedUp = false;
    public AudioClip pickupSound; // Sound to play when the item is picked up
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = pickupSound;
        audioSource.volume = 1.0f; // Ensure the volume is set to an audible level
        audioSource.playOnAwake = false; // Ensure the sound does not play on awake
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPickedUp)
        {
            isPickedUp = true;
            Debug.Log("Player picked up the item.");
            PickUp();
        }
    }

    // This method handles the pickup of the item
    public void PickUp()
    {
        // If you want to do something when the item is picked up, like play an animation or sound
        isPickedUp = true;

        // Optionally disable the item visually
        gameObject.SetActive(false);

        // Handle winning conditions in the GameManager
        GameManager.Instance.AddItemToInventory(this);

        // Play the pickup sound
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        // Destroy the item after it has been picked up
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public string itemName;
    public int groupID; // The group the item belongs to (if you're using groups)
    public bool isPickedUp = false;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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

    public void PickUp()
    {
        isPickedUp = true;
        audioSource.Play();
        GameManager.Instance.AddItemToInventory(this);
        StartCoroutine(DestroyAfterSound());

    }
    
    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}
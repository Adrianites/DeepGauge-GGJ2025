using UnityEngine;
using System.Collections; // Ensure this namespace is included

public class OxygenRecoveryItem : MonoBehaviour
{
    public float recoveryAmount = 20f; // Amount of oxygen to recover
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
        if (other.CompareTag("Player"))
        {
            OxygenManager.Instance.RecoverOxygen(recoveryAmount);
            audioSource.Play(); // Play the pickup sound
            StartCoroutine(DestroyAfterSound()); // Destroy the item after the sound has played
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        // Wait for the sound to finish playing before destroying the object
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject); // Destroy the item after the sound has played
    }
}
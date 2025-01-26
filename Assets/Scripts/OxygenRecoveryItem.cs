using UnityEngine;
using System.Collections;

public class OxygenRecoveryItem : MonoBehaviour
{
    public float recoveryAmount = 20f; 

    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OxygenManager.Instance.RecoverOxygen(recoveryAmount);
            audioSource.Play();
            StartCoroutine(DestroyAfterSound());
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}
using UnityEngine;
using System.Collections;

public class OxygenRecoveryItem : MonoBehaviour
{
    public float recoveryAmount = 20f;

    public AudioSource audioSource;

    private bool isCollected = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            if (OxygenManager.Instance != null)
            {
                OxygenManager.Instance.RecoverOxygen(recoveryAmount);
            }
            StartCoroutine(DestroyAfterSound());
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        foreach (Renderer itemRenderer in GetComponentsInChildren<Renderer>())
        {
            itemRenderer.enabled = false;
        }
        foreach (Collider itemCollider in GetComponentsInChildren<Collider>())
        {
            itemCollider.enabled = false;
        }

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
        }
        Destroy(gameObject);
    }
}

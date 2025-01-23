using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float openingSpeed = 2f;
    public float openHeight = 5f;

    private bool isOpen = false;

    public void OpenDoor()
    {
        if (!isOpen)
        {
            StartCoroutine(Open());
        }
    }

    private IEnumerator Open()
    {
        isOpen = true;
        Vector3 targetPosition = transform.position + Vector3.up * openHeight;
        while (transform.position.y < targetPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, openingSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Valve : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public ParticleSystem particleEffect;
    public Light valveLight;
    public Color inactiveLightColour = Color.red;
    public Color activeLightColour = Color.green;
    public float interactionRange = 3f;

    private bool isInteracting = false;
    private bool isActive = false;
    private Transform player;

    private void Start()
    {
        valveLight.color = inactiveLightColour;
        particleEffect.Play();
    }

    private void Update()
    {
        if (isInteracting)
        {
            RotateValve();
        }
    }

    private void RotateValve()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && !isActive && Vector3.Distance(player.position, transform.position) <= interactionRange)
        {
            if (!isInteracting)
            {
                isInteracting = true;
                StartCoroutine(StopRotationAfterTime(3f));
            }
        }
    }

    private IEnumerator StopRotationAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        isInteracting = false;
        isActive = true;
        valveLight.color = activeLightColour;
        particleEffect.Stop();
        ValvePuzzleManager.Instance.IncrementActiveValves(this);
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    public bool IsActive()
    {
        return isActive;
    }
}
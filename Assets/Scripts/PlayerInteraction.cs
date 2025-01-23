using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Valve currentValve;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Valve"))
        {
            currentValve = other.GetComponent<Valve>();
            currentValve.SetPlayer(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Valve"))
        {
            currentValve = null;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (currentValve != null)
        {
            currentValve.OnInteract(context);
        }
    }
}
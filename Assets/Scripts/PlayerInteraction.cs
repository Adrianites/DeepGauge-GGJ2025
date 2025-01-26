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
            if (currentValve != null)
            {
                currentValve.SetPlayer(transform);
            }
        }
        else if (other.CompareTag("Items"))
        {
            Debug.Log("Item");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Valve"))
        {
            currentValve = null;
        }
        else if (other.CompareTag("Items"))
        {
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
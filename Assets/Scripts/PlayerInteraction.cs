using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Valve currentValve;
    public SceneSwitcher sceneSwitcher;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Valve"))
        {
            currentValve = other.GetComponent<Valve>();
            currentValve.SetPlayer(transform);
        }
        else if (other.CompareTag("SceneSwitcher"))
        {
            sceneSwitcher.SwitchScene();
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
        else if (other.CompareTag("SceneSwitcher"))
        {
            sceneSwitcher.SwitchScene();
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
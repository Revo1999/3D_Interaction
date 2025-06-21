using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OSCListenerEventTrigger : MonoBehaviour, NetworkListener
{
    [Tooltip("Trigger name to listen for in the OSC message")]
    public string eventTriggerName;

    [Tooltip("Event to invoke when the trigger name matches")]
    public UnityEvent onEventTriggered;

    private void OnEnable()
    {
        NetworkController.getController().addListener(this);
    }

    private void OnDisable()
    {
        NetworkController.getController().removeListener(this);
    }

    public void messageArrived(string address, List<NetworkController.OSCValue> values)
    {
        Debug.Log($"[OSCListenerEventTrigger] Received OSC message at address: {address}");

        if (address == "/Events")
        {
            if (values == null || values.Count == 0)
            {
                Debug.LogWarning("[OSCListenerEventTrigger] No values received.");
                return;
            }

            string received = values[0].getString();
            Debug.Log($"[OSCListenerEventTrigger] Comparing received '{received}' to expected '{eventTriggerName}'");

            if (received == eventTriggerName)
            {
                Debug.Log($"[OSCListenerEventTrigger] Trigger match! Invoking UnityEvent for '{eventTriggerName}'");
                onEventTriggered.Invoke();
            }
        }
    }
}

using UnityEngine;
using UnityEngine.Events;
public class SignalListener : MonoBehaviour
{
    [SerializeField] private Signal signal;
    [SerializeField] private UnityEvent signalEvent;

    public void OnSignalRaised()
    {
        signalEvent.Invoke();   // calls the event
    }

    private void OnEnable()
    {
        signal.RegisterListener(this);
    }
    
    private void OnDisable()
    {
        signal.DeregisterListener(this);
    }
}

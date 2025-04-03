using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushAnimEvent : MonoBehaviour
{
    public UnityEvent Event_PushAnim;

    public void EventCall() {
        Event_PushAnim?.Invoke();
    }
}

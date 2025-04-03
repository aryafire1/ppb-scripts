using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CurlingGM : Tally2v2GameManager
{
    public UnityEvent Event_ControllerStart;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Event_ControllerStart?.Invoke();
    }
}

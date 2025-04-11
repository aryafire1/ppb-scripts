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
        /* team1Object.SetActive(false);
        team2Object.SetActive(false);
        StartCoroutine(Countdown()); */
        base.Start();
        Event_ControllerStart?.Invoke();
    }

    /* IEnumerator Countdown() {
        //hardcoding rn
        yield return new WaitForSeconds(12.05f);
        team1Object.SetActive(true);
        team2Object.SetActive(true);
        base.Start();
        //countdownManager.gameObject.SetActive(false);
        Event_ControllerStart?.Invoke();
        //countdownManager.gameObject.SetActive(true);
    } */
}

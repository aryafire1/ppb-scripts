using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public TMP_Text text;
    public CurlingPlayerController controller;

    bool loop = false;

    void OnEnable()
    {
        controller.Event_Input.AddListener(InputCallback);
        StartCoroutine(Countdown(RoundManager.manager.WaitTime));
    }
    void OnDisable() {
        controller.Event_Input.RemoveListener(InputCallback);
    }

    void InputCallback() {
        loop = true;
    }

    IEnumerator Countdown(float seconds) {
        if (loop == false || seconds < 0) {
            yield return null;
            StartCoroutine(Countdown(RoundManager.manager.WaitTime));
        }
        else {
            text.text = seconds.ToString();
            yield return new WaitForSeconds(1f);
            StartCoroutine(Countdown(seconds - 1));
        }
    }
}

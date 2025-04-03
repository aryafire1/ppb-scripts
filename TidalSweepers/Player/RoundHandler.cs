using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


public class RoundHandler : MonoBehaviour
{
#region Variables

    public Camera mainCamera;
    public TallyPlayerPortrait teamScore;
    public int score;
    public float targetDistance;

    [Tooltip("The GameObject that has the next team's power script (likely on the canvas)")]
    public GameObject nextStart;
    public RoundHandler nextTeam;

    public bool yourTurn;
    public bool team2Check;

    public GameObject puck;

    public Vector3 startpoint;
    public Vector3 playerStartpoint;
    [HideInInspector]
    public Vector3 cameraStartpoint;

#endregion

    void Start() {
        cameraStartpoint = mainCamera.gameObject.transform.position;
    }

#region Handling

    public void ScoreUpdate(int s) {
        teamScore.AddPoints(s);


        if (yourTurn) {
            //Debug.Log(RoundManager.manager.rounds);
            if (RoundManager.manager.rounds >= RoundManager.manager.MaxRounds) {
                Invoke("Delay", 0.1f);
            }
            else if (RoundManager.manager.rounds < RoundManager.manager.MaxRounds) {
                Loop();
            }
        }
    }

    void Loop() {
        if (yourTurn) {
            yourTurn = false;
            nextTeam.yourTurn = true;
        }
        puck.GetComponent<Rigidbody>().velocity = Vector3.zero;
        puck.transform.position = startpoint;
    }

    void Delay() {
        RoundManager.manager.Event_EndGame?.Invoke();
    }

#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
#region Variables

    public static AnimManager main;

    public GameObject team1, team2;
    public Animator team1Stick, team2Stick;
    [HideInInspector]
    public Animator team1p1, team1p2, team2p1, team2p2;

#endregion

#region Init

    // Start is called before the first frame update
    void Awake()
    {
        InitSingleton();

        team1p1 = team1.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        team1p2 = team1.transform.GetChild(1).GetChild(0).GetComponent<Animator>();
        team2p1 = team2.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        team2p2 = team2.transform.GetChild(1).GetChild(0).GetComponent<Animator>();
    }

    void InitSingleton() {
        if (main == null) {
            main = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

#endregion

#region Singleton voids

    public void Team1(bool state){
        team1p1.SetBool("Curling", state);
        team1p2.SetBool("Scrubbing", state);
        //team1p2.SetBool("IsScrubbing", state);
    }
    public void Team2(bool state){
        team2p1.SetBool("Curling", state);
        team2p2.SetBool("Scrubbing", state);
        //team2p2.SetBool("IsScrubbing", state);
    }
    public void Team1Trigger() {
        team1p1.SetTrigger("IsCurling");
        team1p2.SetTrigger("IsScrubbing");
        
    }
    public void Team2Trigger() {
        team2p1.SetTrigger("IsCurling");
        team2p2.SetTrigger("IsScrubbing");
    }
    public void SetAll(bool state) {
        Team1(state);
        Team2(state);
    }
    public void SetSpeed1(float speed) {
        team1p2.speed = speed;
        team1Stick.speed = speed;
    }
    public void SetSpeed2(float speed) {
        team2p2.speed = speed;
        team2Stick.speed = speed;
    }
    public void SetSpeedAll(float speed) {
        SetSpeed1(speed);
        SetSpeed2(speed);
    }

#endregion
}

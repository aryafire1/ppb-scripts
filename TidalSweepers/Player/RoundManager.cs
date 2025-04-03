using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    //this is used to set a baseline and then handled by individual team handlers maybe
#region Variables

    //Singleton
    public static RoundManager manager;

    [Header("Round controls")]
    public int MaxRounds;
    public int MaxPoints;
    [Tooltip("How long until input is automatically fired")]
    public float WaitTime;
    [HideInInspector]
    public int rounds;
    int roundCount;
    public TMP_Text roundText;

    [Header("Team objects")]

    public RoundHandler team1Handler;
    public RoundHandler team2Handler;
    public ControllerList team1Controllers, team2Controllers; 
    public GameObject target;

    public UnityEvent Event_CalcScore, Event_EndGame;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        InitSingleton();
    }
    // Start is called before the first frame update
    void Start()
    {
        MaxRounds = MaxRounds * 2;
        RoundUpdate();
        
    }

    void InitSingleton() {
        if (manager != null) {
            Destroy(this.gameObject);
        }
        else {
            manager = this;
        }
    }

#endregion

#region Score scripts

    public void RoundUpdate() {
        if (rounds % 2 == 0) {
            if (rounds != 0) {
                Event_CalcScore?.Invoke();
                gameObject.GetComponent<AudioSource>().PlayOneShot(CurlingSound.main.crowdCheer[Random.Range(0,1)], 1f);
            }
            if (rounds < MaxRounds) {
                roundCount++;
                target.transform.position = target.GetComponent<TargetLocation>().NewLocation();
            }
        }
        roundText.text = "Round " + roundCount;
        
        rounds++;

        AnimManager.main.team1Stick.SetBool("isScrubbing", false);
        AnimManager.main.team2Stick.SetBool("isScrubbing", false);
        AnimManager.main.team1p2.SetBool("IsScrubbing", false);
        AnimManager.main.team2p2.SetBool("IsScrubbing", false);

        if (rounds != 0) {
            StartCoroutine(ParticleDelay());
        }
    }

    public int ScoreUpdate(int distance) {
        int temp = MaxPoints - distance;
        if (temp < 0) {
            temp = 0;
        }
        //Debug.Log($"temp: {temp}");
        return temp;
    }

    public RoundHandler SetNextHandler() {
        if (team1Handler.yourTurn) {
            return team1Handler;
        }
        else if (team2Handler.yourTurn) {
            return team2Handler;
        }
        else {
            return null;
        }
    }

    public ControllerList SetNextController() {
        if (team1Handler.yourTurn) {
            return team1Controllers;
        }
        else if (team2Handler.yourTurn) {
            return team2Controllers;
        }
        else {
            return null;
        }
    }

    IEnumerator ParticleDelay() {
        yield return new WaitForSeconds(1f);
        team1Handler.gameObject.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        team2Handler.gameObject.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
    }

#endregion
}

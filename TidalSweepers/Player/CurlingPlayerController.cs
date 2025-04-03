using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CurlingPlayerController : PlayerController
{
#region Variables

    [Tooltip("The GameObject that has the team's power script (likely on the canvas)")]
    public GameObject startingObject;

    public Mesh[] puckModels;

    //public Material greyscaleMat;
    public AimUIFill powerFill;
    public AimUIFill aimFill;
    public GizmoAngle teamPuck;
    public GameObject teamTimer;
    public bool powerController, aimController;

    [HideInInspector]
    public UnityEvent Event_Power, Event_Aiming, Event_Input;
    public StagnantPlayer correspondingStagnantPlayer;
    [HideInInspector]
    public Image playerImage;

    bool startGame = false; //prevents input until this is true

#endregion

#region Monobehaviour

    protected override void Start() {
        base.Start();
        RoundHandler temp = gameObject.GetComponent<RoundHandler>();
        temp.cameraStartpoint = temp.mainCamera.transform.position;
    }

    public void Init() {
        if (Event_Power == null) {
            Event_Power = new UnityEvent();
        }
        if (Event_Aiming == null) {
            Event_Aiming = new UnityEvent();
        }

        correspondingStagnantPlayer.SetColorById(playerId);

        if (powerController) {
            powerFill.SetSlider(playerColors[playerId]);
        }
        else if (aimController) {
            aimFill.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = aimFill.SetReticle(playerId);
            aimFill.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = aimFill.SetReticle(playerId);
        }

        teamPuck.gameObject.GetComponent<MeshFilter>().mesh = puckModels[playerId];

        playerImage = playerPortrait.portraitObject.GetComponent<Image>();
        //playerImage.material = greyscaleMat;

        UIPortraitInit();
        AnimManager.main.SetAll(true);
        AnimManager.main.team2p1.SetBool("team2Mirror", true);
        AnimManager.main.team2p2.SetBool("team2Mirror", true);
        AnimManager.main.team2Stick.SetBool("team2Mirror", true);
    }

    void UIPortraitInit() {
        if (powerController) {
            powerFill.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = playerImage.sprite;
        }
        if (aimController) {
            aimFill.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = playerImage.sprite;
        }
    }

#endregion

    /* public void AnimateTeam() {
        AnimManager.main.SetAll(true);
    } */

#region Overrides

    public override void OnPrimaryAction(InputAction.CallbackContext value)
    //----------------------------------//
    {
        // Primary is linked to jumping. Handle jumping.

        if (value.started && startGame == true)
        {
            if (powerController) {
                Event_Power?.Invoke();
                Debug.Log("power call");
           }
            else if (aimController) {
                Event_Aiming?.Invoke();
                Debug.Log("aiming call");
            }
            Event_Input?.Invoke();
        }

    }

    public override void OnMovement(InputAction.CallbackContext value)
    //----------------------------------//
    {
        //Debug.Log("movement stuff here maybe??");

    }

    public override void OnStart() {
        base.OnStart();
        
        startGame = true;
        if (this.gameObject.GetComponent<RoundHandler>().yourTurn) {
            startingObject.SetActive(true);
            teamTimer.SetActive(true);
            teamPuck.enabled = true;
        }
    }

    public IEnumerator Autofire(float seconds) {
        teamTimer.transform.GetChild(1).GetComponent<TMP_Text>().text = seconds.ToString();
        seconds--;
        yield return new WaitForSeconds(1f); 
        if (seconds < 0) {
            if (powerController) {
                Event_Power?.Invoke();
           }
            else if (aimController) {
                Event_Aiming?.Invoke();
            }
        }
        else {
            StartCoroutine(Autofire(seconds));
        }
    }

#endregion

#region Debug
#if UNITY_EDITOR

//for singleplayer testing
    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Event_Power?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            Event_Aiming?.Invoke();
        }
    }

#endif
#endregion
}

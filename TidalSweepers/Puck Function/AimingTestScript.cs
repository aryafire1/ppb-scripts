using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AimingTestScript : MonoBehaviour
{
    //hello this script works best when it's attatched to the aiming reticle parent ui object :3

#region Variables

    public ControllerList controller;
    CurlingPlayerController correspondingController;

    [Tooltip("The reticle gameobject needed")]
    public GameObject Rect1, Rect2;
    public float radius;
    public float angleSpeed;
    Sprite rectHold, flash;

    RectTransform rect1, rect2;

    //trig stuff
    Vector2 fixedPoint;
    float currentAngle;
    float needThis;

#endregion

#region Monobehaviour
    
    void OnEnable()
    {
        Init();
        //correspondingController.playerImage.material = null;
    }

    void OnDisable() {
        //correspondingController.playerImage.material = correspondingController.greyscaleMat;
    }

    void Init() {
        rect1 = Rect1.GetComponent<RectTransform>();
        rect2 = Rect2.GetComponent<RectTransform>();

        rectHold = rect1.GetComponent<Image>().sprite;
        flash = GetComponent<AimUIFill>().reticleSet[4];

        fixedPoint = rect1.position;

        correspondingController = controller.controllers[1];
        correspondingController.Event_Aiming.AddListener(StopScript);

        StartCoroutine(SpiralCode());
        StartCoroutine(correspondingController.Autofire(RoundManager.manager.WaitTime));
    }

    //black magic that makes the targets rotate (trigonometry)
    IEnumerator SpiralCode() {
        currentAngle += angleSpeed * Time.deltaTime;
        needThis = Mathf.PingPong(Mathf.Cos(currentAngle)/currentAngle, Mathf.Cos(currentAngle)); //god i hope this works!!!
        Vector2 offset = new Vector2 (Mathf.Sin(currentAngle), needThis) * radius;
        rect1.position = fixedPoint + offset;
        
        rect2.position = fixedPoint - offset;

        yield return new WaitForSeconds(Time.deltaTime);
        StartCoroutine(SpiralCode());
    }

#endregion

#region Event Call

    void StopScript() {
        StartCoroutine(Stop());
        //just to make sure the ui flash actually plays out
    }

    IEnumerator Stop() {
        correspondingController.StopAllCoroutines();
        correspondingController.teamTimer.SetActive(false);

        float distance = Vector2.Distance(rect1.position, rect2.position);
        //Debug.Log(distance); //eventually return this value to use in the final force calc
            //^^ the smaller this value the better //use this as the rotation value???
        
        if (distance < 10) {
            CurlingSound.main.PlaySound(CurlingSound.main.perfectHit);
            yield return StartCoroutine(PerfectFlash());
        }
        else {
            CurlingSound.main.PlaySound(CurlingSound.main.inputHit);
        }

        Rect1.transform.parent.gameObject.SetActive(false); //turn everything off

        //sets aiming stuff up
        correspondingController.teamPuck.aimAngle = distance;
        correspondingController.teamPuck.aimCheck = needThis;

        correspondingController.Event_Aiming.RemoveListener(StopScript);
        //this is going to be very weird maybe
        //correspondingController.teamPuck.FirePuck();
        
        if (correspondingController.gameObject.GetComponent<RoundHandler>().team2Check) {
            //AnimManager.main.Team2(true);
            AnimManager.main.Team2Trigger();
            AnimManager.main.team2Stick.SetBool("isScrubbing", true);
        }
        else {
            //AnimManager.main.Team1(true);
            AnimManager.main.Team1Trigger();
            AnimManager.main.team1Stick.SetBool("isScrubbing", true);
        }
    }

    IEnumerator PerfectFlash() {
        Rect1.GetComponent<Image>().sprite = flash;
        Rect2.GetComponent<Image>().sprite = flash;
        yield return new WaitForSeconds(0.1f);
        Rect1.GetComponent<Image>().sprite = rectHold;
        Rect2.GetComponent<Image>().sprite = rectHold;
    }

#endregion
}

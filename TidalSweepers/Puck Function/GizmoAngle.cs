using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GizmoAngle : MonoBehaviour
{
#region Variables


    [Tooltip("Adjust this for how long to wait before finding distance from target")]
    public int waitTime;
    public int powerStrength;
    public float sweepAnimSpeed;

    [Header("Variables set by the player input")]
    public float power;
    public float aimAngle;
    public float aimCheck;

    [Header("Objects needed")]
    public GameObject target;
    public RoundHandler handler;
    public ControllerList controllerList;
    public PushAnimEvent pushPlayer;
    public GameObject particlePouf;

    GameObject puck;
    Rigidbody rb;

    Vector3 position;
    //Vector2 newDirection;

#endregion

#region Monobehaviour

    void Start() {
        position = this.transform.position;
        puck = this.gameObject;
        rb = puck.GetComponent<Rigidbody>();
    }

    void OnEnable() {
        CurlingSound.main.CroudReact(this.gameObject.GetComponent<AudioSource>(), this.gameObject, target);
        pushPlayer.Event_PushAnim.AddListener(FirePuck);
    }

    void OnDisable() {
        pushPlayer.Event_PushAnim.RemoveListener(FirePuck);
    }

 #endregion

 #region Collision check

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.GetComponent<ColliderTag>() != null && other.relativeVelocity.magnitude > 2) {
            CurlingSound.main.PlaySound(CurlingSound.main.puckHit[Random.Range(0, CurlingSound.main.puckHit.Length)]);
        }
    }

 #endregion

 #region Puck Handling

    //this is fired in the PuckAnimEvent call! it's located on the player's curl anim as an anim event
    public void FirePuck() {

        if (aimCheck > 0) {
            //along the horizontal travel (good)
            aimAngle = aimAngle * (aimCheck + 0.1f);
        }

        if (aimCheck < 0 && aimCheck > -1.5f) {
            //range: -0.1 - -1.4
            aimAngle = aimAngle / 3;
        }

        if (aimCheck <= -1.5f) {
            //everything less than -1.5
            aimAngle = -aimAngle;
            aimAngle = aimAngle / 3;
        }
        //Debug.Log($"new angle: {aimAngle}");

        if (handler.team2Check) {
            //inverts team 2's angle
            aimAngle = -aimAngle;
        }

        //sets the direction it fires in
        puck.transform.rotation = Quaternion.Euler(0, aimAngle, 0);
        handler.transform.GetChild(0).rotation = Quaternion.Euler(0, aimAngle, 0); //curling player rotation
        rb.velocity = power * powerStrength * puck.transform.forward;
        StartCoroutine(PlayerVelocity());

        CurlingSound.main.PlayMovingSound(CurlingSound.main.puckSlide, rb);

        StartCoroutine(WaitForStop());
        if (handler.team2Check) {
            StartCoroutine(SlowAnim2(sweepAnimSpeed));
        }
        else {
            StartCoroutine(SlowAnim1(sweepAnimSpeed));
        }
    }

    public void GetDistance() {
        //handler.score = 0;
            handler.targetDistance = (Vector3.Distance(handler.puck.transform.position, target.transform.position)) * 1.5f;
            Debug.Log($"target distance: {handler.targetDistance}");
            handler.ScoreUpdate(RoundManager.manager.ScoreUpdate((int)handler.targetDistance));
    }

        
    void NextPuck() {
        //StartCoroutine(ReturnCam());
        StopAllCoroutines();
        AnimManager.main.SetSpeedAll(1f);

        if (RoundManager.manager.rounds >= 0 && RoundManager.manager.rounds < RoundManager.manager.MaxRounds) {
            StartCoroutine(CurlingSound.main.FadeOut(this.gameObject.GetComponent<AudioSource>()));
            handler.nextTeam.puck.GetComponent<GizmoAngle>().enabled = !handler.nextTeam.puck.GetComponent<GizmoAngle>().enabled;
            handler.nextStart.SetActive(true);

            handler.gameObject.transform.GetChild(1).GetComponent<Rigidbody>().velocity = Vector3.zero;
            handler.gameObject.transform.GetChild(1).position = handler.playerStartpoint;
            particlePouf.SetActive(true);

            this.enabled = !this.enabled;
        }
    }

#endregion

#region Coroutines

    IEnumerator PlayerVelocity() {
        yield return null;
        handler.gameObject.transform.GetChild(1).GetComponent<Rigidbody>().velocity = rb.velocity;
        StartCoroutine(PlayerVelocity());
    }
    IEnumerator WaitForStop() {
        yield return new WaitForSeconds(waitTime);
        NextPuck();
        RoundManager.manager.RoundUpdate();
    }

    IEnumerator SlowAnim1(float speed) {
        if (rb.velocity.z < speed) {
            speed = rb.velocity.z;
        }
        AnimManager.main.SetSpeed1(speed);
        yield return null;
        StartCoroutine(SlowAnim1(speed));
    }
    IEnumerator SlowAnim2(float speed) {
        if (rb.velocity.z < speed) {
            speed = rb.velocity.z;
        }
        AnimManager.main.SetSpeed2(speed);
        yield return null;
        StartCoroutine(SlowAnim2(speed));
    }

    /* IEnumerator ReturnCam() {
        yield return null;
        if (handler.mainCamera.gameObject.transform.position.z > handler.cameraStartpoint.z) {
            handler.mainCamera.gameObject.transform.position = 
                Vector3.MoveTowards(handler.mainCamera.gameObject.transform.position, handler.cameraStartpoint, 30 * Time.deltaTime);
            StartCoroutine(ReturnCam());
        }
    } */

#endregion
}


#region Old puck instantiating

/* GizmoAngle newPuck = Instantiate(this.gameObject, position, Quaternion.Euler(0, 0, 0)).GetComponent<GizmoAngle>();

            newPuck.handler = RoundManager.manager.SetNextHandler();
            newPuck.controllerList = RoundManager.manager.SetNextController();

            newPuck.controllerList.controllers[0].teamPuck = newPuck;
            newPuck.controllerList.controllers[1].teamPuck = newPuck;
            newPuck.handler.pucks.Add(newPuck.gameObject);

            //to stop calculating old pucks test, working rn
            if (newPuck.handler.pucks.Count > 1) {
                Destroy(newPuck.handler.pucks[0]);
                newPuck.handler.pucks.RemoveAt(0);
            } */

#endregion
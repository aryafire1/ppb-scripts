using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PowerTestScript : MonoBehaviour
{
    //hello this script works best when it's attatched to the power slider ui object :3

#region Variables

    public ControllerList controller;
    CurlingPlayerController correspondingController;

    [Tooltip("The slider object displaying the amount of power")]
    public GameObject sliderUI;
    public GameObject AimingUI;
    Slider slider; 

    int countdown;

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
        slider = sliderUI.GetComponent<Slider>();   

        correspondingController = controller.controllers[0];     

        correspondingController.Event_Power.AddListener(StopScript);
        correspondingController.teamTimer.SetActive(true);

        StartCoroutine(PowerFlux());
        
        StartCoroutine(correspondingController.Autofire(RoundManager.manager.WaitTime));
    }

    /* //this is hardcoding but don't worry about it
    IEnumerator Countdown() {
        countdown--;
        yield return new WaitForSeconds(1f);
        if (countdown > 0) {
            StartCoroutine(Countdown());
        }
        else {
            StartCoroutine(PowerFlux());
        }
    } */

    //coroutine handling continuous value change until input is registered
    IEnumerator PowerFlux() {
        slider.value = Mathf.PingPong(Time.time, slider.maxValue);
        
        yield return new WaitForSeconds(Time.deltaTime); //this might explode, so far it's not
        StartCoroutine(PowerFlux());
    }

#endregion

#region Event Call

    void StopScript() {
        CurlingSound.main.PlaySound(CurlingSound.main.inputHit);

        correspondingController.StopAllCoroutines();

        this.gameObject.SetActive(false); //turn everything off
        AimingUI.SetActive(true); //wake up aiming

        //Debug.Log(slider.value); //eventually return this value to use in the final force calc
        correspondingController.teamPuck.power = slider.value * 10;

        correspondingController.Event_Power.RemoveListener(StopScript);
    }

#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    /* PAUSE SCREEN NOTES
    /// The pause screen is currently not working due to setting the time scale to 0. 
    /// It has an event system set up and ready to be implemented, but I started this on the last day of work essentially.
    /// It is set up to be as independent as possible to the point where you just need to add it into the scene without any changes to the inspector.
    */

    public static PauseScreen main;
    public UnityEvent EVENT_Paused, EVENT_Resume;
    public bool isPaused = false;

    public List<GameObject> children = new List<GameObject>();

    void Awake() {
        if (main != null) {
            Destroy(this.gameObject);
        }
        else {
            main = this;
            DontDestroyOnLoad(this.gameObject);
        }

        if (EVENT_Paused == null) {
            EVENT_Paused = new UnityEvent();
        }
        if (EVENT_Resume == null) {
            EVENT_Resume = new UnityEvent();
        }

        int childcount = gameObject.transform.childCount;
        for (int i = 0; i < childcount; ++i) {
            children.Add(gameObject.transform.GetChild(i).gameObject);
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void OnPause(InputAction.CallbackContext value) {
        if (isPaused && value.performed) {
            Resume();
        }
        else if (isPaused == false && value.performed) {
            Pause();
        }
    }
    
    void Pause() {
        children[0].SetActive(true);
        children[1].SetActive(true);
        isPaused = true;
        EVENT_Paused?.Invoke();
        Time.timeScale = 0;
    }
    void Resume() {
        for (int i = 0; i < children.Count; ++i) {
            children[i].SetActive(false);
        }
        isPaused = false;
        EVENT_Resume?.Invoke();
        Time.timeScale = 1;
    }

    //Button UI voids
    public void ResumeUI() {
        Resume();
    }
    public void MM() {
        children[2].SetActive(true);
        children[1].SetActive(false);
    }
    public void MM_Yes() {
        SceneManager.LoadSceneAsync(0);
    }
    public void MM_No() {
        children[2].SetActive(false);
        children[1].SetActive(true);
    }
    public void QuitUI() {
        children[3].SetActive(true);
        children[1].SetActive(false);
    }
    public void Quit_Yes() {
        Application.Quit();
    }
    public void Quit_No() {
        children[3].SetActive(false);
        children[1].SetActive(true);
    }
}

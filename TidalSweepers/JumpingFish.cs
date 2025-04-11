using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingFish : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    public GameObject vfx;
    float swimSpeed;
    float waitTime;

    void Awake() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        vfx.SetActive(false);
        waitTime = Random.Range(1, 5);
        swimSpeed = Random.Range(1, 5);
        swimSpeed = swimSpeed * -1;
        StartCoroutine(Motion());
        StartCoroutine(RandomJump());
    }

    IEnumerator Motion() {
        swimSpeed = swimSpeed * -1;
        rb.velocity = new Vector3(0, 0, swimSpeed);
        yield return new WaitForSeconds(waitTime);
        gameObject.transform.rotation = gameObject.transform.rotation * new Quaternion(0, 2, 0, 0);
        StartCoroutine(Motion());
    }

    IEnumerator RandomJump() {
        int check = Random.Range(1, 11);
        if (check == 10) {
            anim.SetTrigger("Jump");
        }
        Debug.Log(check);
        yield return new WaitForSeconds(1f);
        StartCoroutine(RandomJump());
    }

    // anim event!
    IEnumerator SplashCall() {
        vfx.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        vfx.SetActive(false);
    }
}

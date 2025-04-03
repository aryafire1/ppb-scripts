using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurlingSound : MonoBehaviour
{
    public static CurlingSound main;

    public AudioClip inputHit;
    public AudioClip perfectHit;
    public AudioClip[] puckHit;
    public AudioClip puckSlide;
    public AudioClip crowdWow;
    public AudioClip[] crowdCheer;

    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        if (main != null) {
            Destroy(this.gameObject);
        }
        else {
            main = this;
        }

        audio = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip) {
        audio.PlayOneShot(clip, 1f);
    }

    public void PlayMovingSound(AudioClip clip, Rigidbody rb) {
        audio.PlayOneShot(clip, 1f);
        StartCoroutine(MovementCheck(rb));
    }

    public void CroudReact(AudioSource source, GameObject puck, GameObject target) {
        source.PlayOneShot(crowdWow, 1f);
        source.volume = 0f;

        StartCoroutine(DistanceReaction(source, puck, target));
    }

    IEnumerator MovementCheck(Rigidbody rb) {
        yield return null;
        
        if (rb.velocity.z > 1) {
            StartCoroutine(MovementCheck(rb));
        }
        else if (rb.velocity.z < 1 && rb.velocity.z > 0.1) {
            audio.volume = rb.velocity.z;
            StartCoroutine(MovementCheck(rb));
        }
        else {
            audio.Stop();
            audio.volume = 1;
        }
    }

    IEnumerator DistanceReaction(AudioSource source, GameObject puck, GameObject target) {
        yield return null;
        float distance = Vector3.Distance(puck.transform.position, target.transform.position);

        if (distance < 1) {
            StartCoroutine(DistanceReaction(source, puck, target));
        }
        else if (puck.GetComponent<Rigidbody>().velocity.z < 1) {
            //source.volume = puck.GetComponent<Rigidbody>().velocity.z;
            StartCoroutine(DistanceReaction(source, puck, target));
        }
        else if (distance < 10 && distance > 1) {
            source.volume = 1 - (distance * 0.1f);
            StartCoroutine(DistanceReaction(source, puck, target));
        }
        else {
            StartCoroutine(DistanceReaction(source, puck, target));
        }
    }

    public IEnumerator FadeOut(AudioSource source) {
        yield return new WaitForSeconds(0.1f);

        if (source.volume > 0) {
            source.volume = source.volume - 0.1f;
            StartCoroutine(FadeOut(source));
        }
    }
}

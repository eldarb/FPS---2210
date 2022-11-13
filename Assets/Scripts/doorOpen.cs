using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpen : MonoBehaviour
{
    [SerializeField] Animator myDoor = null;
    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip sound;

    private void OnTriggerEnter(Collider other)
    {
        float soundVol = PlayerPrefs.GetFloat("volume");
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Small Door"))
            {
                myDoor.Play("DoorOpenAnimation", 0, 0.0f);
                aud.PlayOneShot(sound, soundVol * 0.5f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        float soundVol = PlayerPrefs.GetFloat("volume");
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Small Door"))
            {
                myDoor.Play("DoorCloseAnimation", 0, 0.0f);
                aud.PlayOneShot(sound, soundVol * 0.5f);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    [SerializeField] Animator myDoor = null;
    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip sound;

    bool isDoorOpen = true;

    void Update()
    {
        isDoorOpen = !isDoorOpen;
        float soundVol = PlayerPrefs.GetFloat("volume");
        if (Input.GetButtonDown("Interact") && isDoorOpen == false)
        {
            myDoor.Play("DoorOpenAnimation", 0, 0.0f);
            aud.PlayOneShot(sound, soundVol * 0.5f);
            isDoorOpen = true;
        }
    }
}

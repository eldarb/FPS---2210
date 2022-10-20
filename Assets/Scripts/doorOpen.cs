using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpen : MonoBehaviour
{
    [SerializeField] Animator myDoor = null;
    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip sound;
    [Range(0, 1)] [SerializeField] float soundVol;
    bool openTrigger;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Small Door"))
            {
                if (!openTrigger)
                {
                    openTrigger = !openTrigger;
                    myDoor.Play("DoorOpenAnimation", 0, 0.0f);
                    aud.PlayOneShot(sound, soundVol);
                }
                else if (openTrigger)
                {
                    openTrigger = !openTrigger;
                    myDoor.Play("DoorCloseAnimation", 0, 0.0f);
                    aud.PlayOneShot(sound, soundVol);
                }
            }
        }
    }
}

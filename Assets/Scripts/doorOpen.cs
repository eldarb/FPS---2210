using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpen : MonoBehaviour
{
    [SerializeField] Animator myDoor = null;
    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip sound;

    int closeDelay = 10;
    bool isDoorOpen = true;

    private void Start()
    {
        isDoorOpen = !isDoorOpen;
    }

    void Update()
    {
        float soundVol = PlayerPrefs.GetFloat("volume");
        if (Input.GetButtonDown("Interact") && isDoorOpen == false)
        {
            myDoor.Play("DoorOpenAnimation", 0, 0.0f);
            aud.PlayOneShot(sound, soundVol * 0.5f);
            isDoorOpen = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        float soundVol = PlayerPrefs.GetFloat("volume");
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Small Door"))
            {
                gameManager.instance.interactMessage.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //float soundVol = PlayerPrefs.GetFloat("volume");
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Small Door"))
            {
                gameManager.instance.interactMessage.gameObject.SetActive(false);
                StartCoroutine(delayedClose());
                //myDoor.Play("DoorCloseAnimation", 0, 0.0f);
                //aud.PlayOneShot(sound, soundVol * 0.5f);
            }
        }
    }

    IEnumerator delayedClose()
    {
        if (isDoorOpen)
        {
            float soundVol = PlayerPrefs.GetFloat("volume");
            yield return new WaitForSeconds(closeDelay);
            myDoor.Play("DoorCloseAnimation", 0, 0.0f);
            aud.PlayOneShot(sound, soundVol * 0.5f);
            isDoorOpen = false;
        }
    }
}

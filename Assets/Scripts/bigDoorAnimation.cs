using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigDoorAnimation : MonoBehaviour
{
    [SerializeField] Animator leftDoor;
    [SerializeField] Animator rightDoor;
    [SerializeField] AudioSource doorAudPlayer;
    [SerializeField] AudioClip doorCloseAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Big Door"))
        {
            float vol = PlayerPrefs.GetFloat("volume");
            gameManager.instance.isInBossRoom = true;
            rightDoor.Play("BigDoorRightAnimation", 0, 0.0f);
            leftDoor.Play("BigDoorLeftAnimation", 0, 0.0f);
            doorAudPlayer.PlayOneShot(doorCloseAudio, vol);
            gameObject.SetActive(false);
            gameManager.instance.spawnPosition.transform.position = gameObject.transform.position;
        }
    }
}

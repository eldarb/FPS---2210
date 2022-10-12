using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigDoorAnimation : MonoBehaviour
{
    [SerializeField] Animator myDoor = null;
    [SerializeField] string animationName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Big Door"))
        {
            if (animationName == "BigDoorRightAnimation")
            {
                myDoor.Play("BigDoorRightAnimation", 0, 0.0f);
                gameObject.SetActive(false);
            }
            else if (animationName == "BigDoorLeftAnimation")
            {
                myDoor.Play("BigDoorLeftAnimation", 0, 0.0f);
                gameObject.SetActive(false);
            }
            gameManager.instance.spawnPosition.transform.position = gameObject.transform.position;
        }
    }
}

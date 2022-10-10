using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpen : MonoBehaviour
{
    [SerializeField] Animator myDoor = null;

    [SerializeField] bool openTrigger;
    [SerializeField] bool closeTrigger;
    [SerializeField] string animationName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            {
            if (closeTrigger && animationName == "BigDoorRightAnimation")
            {
                myDoor.Play("BigDoorRightAnimation", 0, 0.0f);
                gameObject.SetActive(false);
            }
            else if (closeTrigger && animationName == "BigDoorLeftAnimation")
            {
                myDoor.Play("BigDoorLeftAnimation", 0, 0.0f);
                gameObject.SetActive(false);
            }
            else if (openTrigger && animationName == "DoorOpenAnimation")
            {
                myDoor.Play("DoorOpenAnimation", 0, 0.0f);
                gameObject.SetActive(false);
            }
            else if(closeTrigger && animationName == "DoorCloseAnimation")
            {
                myDoor.Play("DoorCloseAnimation", 0, 0.0f);
                gameObject.SetActive(false);
            }
        }
    }
}

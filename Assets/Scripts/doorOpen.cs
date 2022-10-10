using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpen : MonoBehaviour
{
    [SerializeField] Animator myDoor = null;

    [SerializeField] bool openTrigger;
    [SerializeField] bool closeTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            {
            if(openTrigger)
            {
                myDoor.Play("DoorOpenAnimation", 0, 0.0f);
                gameObject.SetActive(false);
            }
            else if(closeTrigger)
            {
                myDoor.Play("DoorCloseAnimation", 0, 0.0f);
                gameObject.SetActive(false);
            }
        }
    }
}

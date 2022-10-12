using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpen : MonoBehaviour
{
    [SerializeField] Animator myDoor = null;

    bool openTrigger;
    bool open;

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
                }
                else if (openTrigger)
                {
                    openTrigger = !openTrigger;
                    myDoor.Play("DoorCloseAnimation", 0, 0.0f);
                }
            }
        }
    }
}

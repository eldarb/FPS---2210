using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedTraits : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.EnhanceTraits();
            gameObject.SetActive(false);
        }
    }
}

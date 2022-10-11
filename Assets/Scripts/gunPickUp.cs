using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickUp : MonoBehaviour
{
    [SerializeField] gunStats gunStat;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.gunPickUp(gunStat);
            Destroy(gameObject);
        }
    }







}

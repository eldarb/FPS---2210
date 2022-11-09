using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soulPickUp : MonoBehaviour
{
    [SerializeField] soulStats soulStats;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.healPlayer(soulStats.soulsAmount);
            Destroy(gameObject);
        }
    }
}

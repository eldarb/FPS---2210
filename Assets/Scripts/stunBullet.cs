using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stunBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int spd;
    [SerializeField] int dmg;
    [SerializeField] effect efct;
    [SerializeField] int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = gameObject.transform.forward * spd;
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Melee") || other.gameObject.CompareTag("Range") || other.gameObject.CompareTag("King"))
        {
            other.gameObject.GetComponent<IDamage>().takeDamage(dmg);
            other.gameObject.GetComponent<IDamage>().takeEffect(efct);
        }

        Destroy(gameObject);
    }
}

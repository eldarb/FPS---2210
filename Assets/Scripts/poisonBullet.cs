using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poisonBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int spd;
    [SerializeField] int dmg;
    [SerializeField] effect efct;
    [SerializeField] int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * spd;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Melee"))
        {
            other.gameObject.GetComponent<IDamage>().takeDamage(dmg);
            other.gameObject.GetComponent<IDamage>().takeEffect(efct);
        }

        Destroy(gameObject);
    }
}

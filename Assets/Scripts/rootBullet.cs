using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rootBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int spd;
    [SerializeField] int dmg;
    [SerializeField] int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * spd;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<IDamage>().takeDamage(dmg);
        }

        Destroy(gameObject);
    }
}

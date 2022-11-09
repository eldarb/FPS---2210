using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnAnimation : MonoBehaviour
{
    [SerializeField] CapsuleCollider col;

    private void Start()
    {
        //col = GetComponent<CapsuleCollider>();
        col.enabled = false;
    }
    public void EnableColliderOnTheSword()
    { col.enabled = true; }
    public void DisableColliderOnTheSword()
    { col.enabled = false; }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Melee") || collision.gameObject.CompareTag("Range") || collision.gameObject.CompareTag("King"))
        {
            collision.gameObject.GetComponent<IDamage>().takeDamage(gameManager.instance.weaponHandlerScript.GetDamage());
        }
    }
}

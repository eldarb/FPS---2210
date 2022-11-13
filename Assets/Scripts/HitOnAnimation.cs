using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnAnimation : MonoBehaviour
{
    [SerializeField] CapsuleCollider col;
    [SerializeField] AudioClip sound;
    [SerializeField] AudioClip hitSound;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [Range(0, 1)] [SerializeField] float gunShotAudVol;
    private void Start()
    {
        gunShotAudVol = PlayerPrefs.GetFloat("volume") * 0.5f;
        col.enabled = false;
    }
    public void EnableColliderOnTheSword()
    { col.enabled = true; aud.PlayOneShot(sound, gunShotAudVol); }
    public void DisableColliderOnTheSword()
    { col.enabled = false; }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Melee") || collision.gameObject.CompareTag("Range") || collision.gameObject.CompareTag("King"))
        {
            collision.gameObject.GetComponent<IDamage>().takeDamage(gameManager.instance.weaponHandlerScript.GetDamage());
            aud.PlayOneShot(hitSound, gunShotAudVol);
        }
    }
}

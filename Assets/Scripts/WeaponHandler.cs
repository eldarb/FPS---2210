using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("----- Gun Stats -----")]
    [SerializeField] string gunType;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;
    [SerializeField] int shootDamage;
    [SerializeField] GameObject gunModel;
    [SerializeField] AudioClip sound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] List<gunStats> gunStat = new List<gunStats>();

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [Range(0, 1)] [SerializeField] float gunShotAudVol;


    bool isShooting;
    int selectGun;

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(shoot());
        gunSelect();
    }
    IEnumerator shoot()
    {
        if (gunStat.Count > 0 && Input.GetButton("Shoot") && !isShooting)
        {
            isShooting = true;
            if (gunType == "Melee")
            {
                gunModel.GetComponent<Animator>().SetTrigger("Attack");
            }
            else if (gunType == "Range")
            {
            }
            aud.PlayOneShot(sound, gunShotAudVol);
            //yield return new WaitForSeconds(shootRate);
            yield return new WaitForSeconds(gunModel.GetComponent<Animator>().speed);
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    aud.PlayOneShot(hitSound, gunShotAudVol);
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                }
            }

            isShooting = false;
        }
    }

    public void ShootOnAnimation()
    {
        StartCoroutine(shoot());
    }

    public void gunPickUp(gunStats stats)
    {
        if (gunStat.Count > 0)
            Destroy(gunModel);
        gunType = stats.gunType;
        shootRate = stats.shootRate;
        shootDistance = stats.shootDistance;
        shootDamage = stats.shootDamage;
        sound = stats.sound;
        hitSound = stats.hitSound;
        gunModel = Instantiate(stats.gunModel, transform);

        gunStat.Add(stats);
    }

    void gunSelect()
    {
        if (gunStat.Count > 1)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectGun < gunStat.Count - 1)
            {
                selectGun++;
                changeGun();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectGun > 0)
            {
                selectGun--;
                changeGun();
            }
        }
    }

    void changeGun()
    {
        Destroy(gunModel);
        gunType = gunStat[selectGun].gunType;
        shootRate = gunStat[selectGun].shootRate;
        shootDistance = gunStat[selectGun].shootDistance;
        shootDamage = gunStat[selectGun].shootDamage;
        sound = gunStat[selectGun].sound;
        hitSound = gunStat[selectGun].hitSound;
        gunModel = Instantiate(gunStat[selectGun].gunModel, transform);
    }
}

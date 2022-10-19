using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("----- Gun Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;
    [SerializeField] int shootDamage;
    [SerializeField] GameObject gunModel;
    [SerializeField] List<gunStats> gunStat = new List<gunStats>();


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
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
            }
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    public void gunPickUp(gunStats stats)
    {
        if(gunStat.Count > 0)
            Destroy(gunModel);

        shootRate = stats.shootRate;
        shootDistance = stats.shootDistance;
        shootDamage = stats.shootDamage;

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
        shootRate = gunStat[selectGun].shootRate;
        shootDistance = gunStat[selectGun].shootDistance;
        shootDamage = gunStat[selectGun].shootDamage;
        gunModel = Instantiate(gunStat[selectGun].gunModel, transform);
    }
}

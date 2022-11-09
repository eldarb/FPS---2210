using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("----- Gun Stats -----")]
    [SerializeField] string gunName;
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

    private void OnDestroy()
    {
        SaveGunStats();
    }

    private void Start()
    {
        LoadGunStats();
    }
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
                gunModel.GetComponent<Animator>().speed = 1/shootRate;
            }
            else if (gunType == "Range")
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
                {
                    if (hit.collider.GetComponent<IDamage>() != null)
                    {
                        aud.PlayOneShot(hitSound, gunShotAudVol);
                        hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                    }
                }
            }
            aud.PlayOneShot(sound, gunShotAudVol);
            yield return new WaitForSeconds(shootRate);
            gunModel.GetComponent<Animator>().speed = 1;

            isShooting = false;
        }
    }

    public void ShootOnAnimation()
    {
        StartCoroutine(shoot());
    }

    public void gunPickUp(gunStats stats)
    {
        bool toAddGun = true;
        for (int i = 0; i < gunStat.Count; i++)
            if (gunStat[i].gunName == stats.gunName)
            {
                toAddGun = false;
            }
        if (toAddGun)
        {
            if (gunStat.Count > 0)
                Destroy(gunModel);
            gunName = stats.gunName;
            gunType = stats.gunType;
            shootRate = stats.shootRate;
            shootDistance = stats.shootDistance;
            shootDamage = stats.shootDamage;
            sound = stats.sound;
            hitSound = stats.hitSound;
            gunModel = Instantiate(stats.gunModel, transform);

            gunStat.Add(stats);
        }
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
        gunName = gunStat[selectGun].gunName;
        gunType = gunStat[selectGun].gunType;
        shootRate = gunStat[selectGun].shootRate;
        shootDistance = gunStat[selectGun].shootDistance;
        shootDamage = gunStat[selectGun].shootDamage;
        sound = gunStat[selectGun].sound;
        hitSound = gunStat[selectGun].hitSound;
        gunModel = Instantiate(gunStat[selectGun].gunModel, transform);
    }

    void SaveGunStats()
    {
        for (int i = 0; i < gunStat.Count; i++)
        {
            PlayerPrefs.SetString("Weapon_" + i + "_name", gunStat[i].gunName);
        }
    }

    void LoadGunStats()
    {
        int i = 0;
        while (PlayerPrefs.HasKey("Weapon_" + i + "_name"))
        {
            string name;
            name = PlayerPrefs.GetString("Weapon_" + i + "_name");
            for (int j = 0; j < gameManager.instance.GunList.Count; j++)
            {
                if (gameManager.instance.GunList[j].gunName == name)
                {
                    gunPickUp(gameManager.instance.GunList[j]);
                }
            }
            i++;
        }
    }

    public void ClearGuns()
    {
        gunStat.Clear();
        Destroy(gunModel);
    }

    public int GetDamage()
    {
        return shootDamage;
    }
}

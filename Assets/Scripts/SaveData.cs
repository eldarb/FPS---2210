using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    static bool inPocketDimension;
    // Start is called before the first frame update
    //void Start()
    //{
    //    LoadPlayerStats();
    //}

    //private void OnDestroy()
    //{
    //    SavePlayerStats();
    //}
    //void SavePlayerStats()
    //{
    //    //PlayerPrefs.SetInt("Health", health);

    //    for (int i = 0; i < weaponList.Count; i++)
    //    {
    //        PlayerPrefs.SetFloat("Weapon_" + i + "_name", weaponList[i].name);
    //        PlayerPrefs.SetFloat("Weapon_" + i + "rateOfFire", weaponList[i].name);
    //        PlayerPrefs.SetFloat("Weapon_" + i + "rateOfFire", weaponList[i].name);
    //    }
    //}
    //void LoadPlayerStats()
    //{
    //    //PlayerPrefs.SetInt("Health", health);

    //    int i = 0;
    //    while(PlayerPrefs.HasKey("Weapon_" + i + "_name"))
    //    {
    //        gunStats loadedweapon = new gunStats();
    //        loadedweapon.gunType = PlayerPrefs.GetString("Weapon_" + i + "_name", weaponList[i].name);
    //        loadedweapon.shootRate = PlayerPrefs.GetFloat("Weapon_" + i + "rateOfFire", weaponList[i].name);
    //        loadedweapon.shootDamage = PlayerPrefs.GetFloat("Weapon_" + i + "rateOfFire", weaponList[i].name);
    //        weaponList.add(loadedweapon);
    //        i++;
    //    }
    //}
}

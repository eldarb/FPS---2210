using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWipeOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        PlayerPrefs.DeleteKey("Health");
        while (PlayerPrefs.HasKey("Weapon_" + i + "_name"))
        {
            PlayerPrefs.DeleteKey("Weapon_" + i + "_name");
            i++;
        }
        gameManager.instance.weaponHandlerScript.ClearGuns();
    }
}

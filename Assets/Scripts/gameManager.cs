using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    public int enemyCount;
    public int waveCount;

    [Header("----- Player -----")]
    public GameObject player;
    public PlayerController playerScript;
    public GameObject spawnPosition;

    [Header("----- Weapon Handler -----")]
    public GameObject weaponHandler;
    public WeaponHandler weaponHandlerScript;
    [Header("----- UI -----")]
    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject winMenu;
    public GameObject currentMenu;
    public GameObject playerDamageFlash;
    public Image playerHPBar;
    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI enemyText;
    public TextMeshProUGUI waveCountText;
    public TextMeshProUGUI waveText;
    public GameObject hiddenWinConditionPanel;

    public bool isPaused;
    // Start is called before the first frame update
    // Creates the instance that holds the playerController
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        weaponHandler = GameObject.FindGameObjectWithTag("Weapon Handler");
        weaponHandlerScript = weaponHandler.GetComponent<WeaponHandler>();
        spawnPosition = GameObject.FindGameObjectWithTag("Spawn Point");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !playerDeadMenu.activeSelf && !winMenu.activeSelf)
        {
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);

            if (isPaused)
                cursorLockPause();
            else
                cursorUnLockUnPause();
        }
    }

    public void cursorLockPause()
    {
        playerDamageFlash.SetActive(false);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void cursorUnLockUnPause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public IEnumerator playerDamage()
    {
        playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageFlash.SetActive(false);
    }

    public void checkEnemyTotal()
    {
        enemyCount--;
        enemyCountText.text = enemyCount.ToString("F0");
    }
    public void CheckWinCondition()
    {
        waveCountText.text = waveCount.ToString("F0");
        if (waveCount == 5)
        {
            winMenu.SetActive(true);
            cursorLockPause();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    public int enemyCount;

    [Header("----- Player -----")]
    public GameObject player;
    public PlayerController playerScript;
    public GameObject spawnPosition;

    [Header("----- UI -----")]
    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject winMenu;
    public GameObject currentMenu;
    public GameObject playerDamageFlash;
    public Image playerHPBar;
    public TextMeshProUGUI enemyCountText;

    public bool isPaused;

    // Start is called before the first frame update
    // Creates the instance that holds the playerController
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        //spawnPosition = GameObject.FindGameObjectWithTag("Spawn Position");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !playerDeadMenu.activeSelf && !winMenu.activeSelf)
        {
            togglePause();
        }
    }

    public void togglePause() {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
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
        if (enemyCount < 0)
        {
            winMenu.SetActive(true);
            togglePause();
        }
    }
}

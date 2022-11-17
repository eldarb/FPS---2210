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
    public int waveMax;
    public int dmgCount;
    public bool isInBossRoom;
    public bool hasPlayerBeatAllWaves = false;

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
    public GameObject creditScreen;
    public bool credits;
    public bool spawnerEnabled;
    public GameObject abilityMenu;
    public abilityBar abilityBar;
    public List<GameObject> menuAbilities = new List<GameObject>();
    public GameObject currentMenu;
    public GameObject playerDamageFlash;
    public Image playerHPBar;
    public TextMeshProUGUI playerCurrentHPText;
    public TextMeshProUGUI playerMaxHPText;
    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI enemyText;
    public TextMeshProUGUI waveCountText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI soulsText;
    public TextMeshProUGUI soulsCount;
    public GameObject hiddenWinConditionPanel;
    public GameObject enhancedTraitsNotifier;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip sound;
    [Range(0, 1)] [SerializeField] float soundVol;

    [Header("----- Teleport -----")]
    [SerializeField] GameObject teleportToNextLevel;

    [Header("----- Gun List -----")]
    public List<gunStats> GunList = new List<gunStats>();


    public bool isPaused;
    
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        weaponHandler = GameObject.FindGameObjectWithTag("Weapon Handler");
        weaponHandlerScript = weaponHandler.GetComponent<WeaponHandler>();
        spawnPosition = GameObject.FindGameObjectWithTag("Spawn Point");
        aud = GameObject.FindGameObjectWithTag("Big Door").GetComponent<AudioSource>();
        teleportToNextLevel = GameObject.FindGameObjectWithTag("Teleport");
        teleportToNextLevel.SetActive(false);
        soundVol = PlayerPrefs.GetFloat("volume");
        waveNumberText.text = waveMax.ToString("F0");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !playerDeadMenu.activeSelf && !winMenu.activeSelf && !abilityMenu.activeSelf)
        {
            if (gameManager.instance.creditScreen.activeSelf)
            {
                creditScreen.SetActive(false);
            }
            else
            {
                isPaused = !isPaused;
                pauseMenu.SetActive(isPaused);
                if (isPaused)
                {
                    cursorLockPause();
                }
                else
                {
                    cursorUnLockUnPause();
				}
            }                       
        }
        else if (Input.GetButtonDown("Tab") && !playerDeadMenu.activeSelf && !winMenu.activeSelf && !pauseMenu.activeSelf)
        {
            isPaused = !isPaused;
            abilityMenu.SetActive(isPaused);
            if (isPaused)
                cursorLockPause();
            else
                cursorUnLockUnPause();
        }

        if (isInBossRoom)
        {
            aud.PlayOneShot(sound, soundVol);
            StartCoroutine(EnhancedTraitsNotifier());

            isInBossRoom = false;
        }
    }

    public void cursorLockPause()
    {
        playerScript.enabled = false;
        playerDamageFlash.SetActive(false);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    public void cursorUnLockUnPause()
    {
        playerScript.enabled = true;
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
        if (spawnerEnabled) {
        enemyCount--;
        enemyCountText.text = enemyCount.ToString("F0");
    }
    }

    public void HasPlayerBeatAllWaves()
    {
        UpdateWaveCount();
        if (waveCount == waveMax)
        {
            teleportToNextLevel.SetActive(true);
            hasPlayerBeatAllWaves = true;
        }
    }

    public void UpdateWaveCount()
    {
        waveCountText.text = waveCount.ToString("F0");
    }

    public void CheckWinCondition()
    {
            winMenu.SetActive(true);
            cursorLockPause();
    }


    public IEnumerator EnhancedTraitsNotifier()
    {
        enhancedTraitsNotifier.SetActive(true);
        yield return new WaitForSeconds(2);
        enhancedTraitsNotifier.SetActive(false);
    }

    public void updateSoulsCount() {
        soulsCount.text = playerScript.soulCount.ToString("F0");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDamage
{

    public int soulCount;

    [Header("----- Component -----")]
    [SerializeField] CharacterController playerController;

    [Header("----- Player Stats -----")]
    [Range(1, 100)] [SerializeField] int HP;
    [Range(1, 20)] [SerializeField] float playerSpeed;
    [Range(1.1f, 2f)] [SerializeField] float sprintMultiplier;
    [Range(5, 15)] [SerializeField] float jumpHeight;
    [Range(15, 35)] [SerializeField] float gravityValue;
    [Range(1, 5)] [SerializeField] int jumpsMax;

    [Header("----- Control Variables -----")]
    [Range(0, 5)] [SerializeField] float damageDelay;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] playerHurtAud;
    [Range(0, 1)] [SerializeField] float playerHurtAudVol;
    [SerializeField] AudioClip[] playerStepsAud;
    [Range(0, 1)] [SerializeField] float playerStepsAudVol;
    [SerializeField] AudioClip[] playerJumpAud;
    [Range(0, 1)] [SerializeField] float playerJumpAudVol;

    [Header("----- Useable Objects")]
    [SerializeField] GameObject teleportToPocketDimension;

    [Header("----- Abilities -----")]
    [SerializeField] public List<ability> abilities = new List<ability>();
    [SerializeField] public int selected;
    [SerializeField] GameObject shootPosition;

    bool[] coolingdown = new bool[4];

    Vector3 playerVelocity;
    Vector3 teleportPosition;
    private int timesJumped;
    int HPOrig;
    float playerSpeedOrig;
    bool isSprinting;
    bool playingSteps;
    bool canTeleport;
    bool isInvincible;

    private void OnDestroy()
    {
        SavePlayerStats();
    }

    private void Start()
    {
        HPOrig = HP;
        teleportPosition = teleportToPocketDimension.transform.localPosition;
        respawn();
        //LoadPlayerStats(); There should be a condition controlling if these need to be reset.
        updatePlayerHUD();
        playerSpeedOrig = playerSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        playerMove();
        sprint();
        StartCoroutine(shoot0());
        StartCoroutine(shoot1());
        StartCoroutine(shoot2());
        abilitySelect();
        StartCoroutine(TeleportToPocketDimension());
    }

    void playerMove()
    {
        if (playerController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            timesJumped = 0;
        }

        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        Vector3 move = transform.right * xAxis + transform.forward * zAxis;
        playerController.Move(move * playerSpeed * Time.deltaTime);

        StartCoroutine(playSteps());

        if (Input.GetButtonDown("Jump") && timesJumped < jumpsMax)
        {
            timesJumped++;
            playerVelocity.y += jumpHeight;
            aud.PlayOneShot(playerJumpAud[Random.Range(0, playerJumpAud.Length)], playerJumpAudVol);
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        playerController.Move(playerVelocity * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed *= sprintMultiplier;
        }

        if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed = playerSpeedOrig;
        }
    }

    IEnumerator playSteps()
    {
        if (!playingSteps && playerController.velocity.magnitude > 0.3f && playerVelocity.y == 0)
        {
            playingSteps = true;
            aud.PlayOneShot(playerStepsAud[Random.Range(0, playerStepsAud.Length)], playerStepsAudVol);
            if (isSprinting)
            {
                yield return new WaitForSeconds(0.225f);
            }
            else
            {
                yield return new WaitForSeconds(0.3f);
            }
            playingSteps = false;
        }
    }

    public void takeDamage(int damage)
    {
        if (!isInvincible)
        {
            HP -= damage;

            aud.PlayOneShot(playerHurtAud[Random.Range(0, playerHurtAud.Length - 1)], playerHurtAudVol);

            updatePlayerHUD();
            StartCoroutine(gameManager.instance.playerDamage());

            if (HP <= 0)
            {
                gameManager.instance.playerDamageFlash.SetActive(false);
                gameManager.instance.playerDeadMenu.SetActive(true);
                gameManager.instance.cursorLockPause();
            }

            StartCoroutine(delayDamage()); 
        }
    }

    IEnumerator delayDamage()
    {
        isInvincible = true;
        yield return new WaitForSeconds(damageDelay);
        isInvincible = false;
    }

    public void updatePlayerHUD()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / (float)HPOrig;
    }

    public void respawn()
    {
        playerController.enabled = false;
        gameManager.instance.playerDeadMenu.SetActive(false);
        transform.position = gameManager.instance.spawnPosition.transform.position;
        playerController.enabled = true;
    }

    public void EnhanceTraits()
    {
        HP = 20;
        HPOrig = HP;
        playerSpeed *= 1.3f;
        jumpHeight *= 1.2f;
        gravityValue = 20;
        sprintMultiplier = 2f;
        jumpsMax *= 3;
        updatePlayerHUD();
    }

    IEnumerator TeleportToPocketDimension()
    {
        //if (Input.GetKeyDown(KeyCode.E) && gameManager.instance.inPocketDimension == false)
        if (Input.GetKeyDown(KeyCode.E))
        {
            teleportToPocketDimension.SetActive(true);
            teleportToPocketDimension.transform.parent = null;
            yield return new WaitForSeconds(3);
            teleportToPocketDimension.SetActive(false);
            teleportToPocketDimension.transform.parent = gameObject.transform;
            teleportToPocketDimension.transform.localPosition = teleportPosition;
            teleportToPocketDimension.transform.LookAt(gameObject.transform);
        }
    }
    IEnumerator shoot0() // Separate co-routines for each ability to have their own cooldown timer
    {
        if (Input.GetButton("Shoot Ability") && selected == 0 && coolingdown[selected] != true && HP > abilities[selected].HPcost)
        {
            coolingdown[selected] = true;

            gameManager.instance.abilityBar.cooldown(selected);

            HP -= abilities[selected].HPcost;
            updatePlayerHUD();

            Instantiate(abilities[selected].bullet, shootPosition.transform.position, transform.rotation);

            yield return new WaitForSeconds(abilities[selected].cooldown);

            coolingdown[selected] = false;
        }
    }
    IEnumerator shoot1()
    {
        if (Input.GetButton("Shoot Ability") && selected == 1 && coolingdown[selected] != true && HP > abilities[selected].HPcost)
        {
            coolingdown[selected] = true;

            gameManager.instance.abilityBar.cooldown(selected);

            HP -= abilities[selected].HPcost;
            updatePlayerHUD();

            Instantiate(abilities[selected].bullet, shootPosition.transform.position, transform.rotation);

            yield return new WaitForSeconds(abilities[selected].cooldown);

            coolingdown[selected] = false;
        }
    }
    IEnumerator shoot2()
    {
        if (Input.GetButton("Shoot Ability") && selected == 2 && coolingdown[selected] != true && HP > abilities[selected].HPcost)
        {
            coolingdown[selected] = true;

            gameManager.instance.abilityBar.cooldown(selected);

            HP -= abilities[selected].HPcost;
            updatePlayerHUD();

            Instantiate(abilities[selected].bullet, shootPosition.transform.position, transform.rotation);

            yield return new WaitForSeconds(abilities[selected].cooldown);

            coolingdown[selected] = false;
        }
    }


    public void abilitySelect()
    {
        if (Input.GetButtonDown("Ability1") && abilities[0] != null)
        {
            selected = 0;
        }
        else if (Input.GetButtonDown("Ability2") && abilities[1] != null)
        {
            selected = 1;
        }
        else if (Input.GetButtonDown("Ability3") && abilities[2] != null)
        {
            selected = 2;
        }
        gameManager.instance.abilityBar.updateAbilities();
    }
    public void takeEffect(effect efct)
    {

    }

    public void Restart()
    {
        HP = HPOrig;
    }

    void SavePlayerStats()
    {
        PlayerPrefs.SetInt("Health", HP);
    }
    void LoadPlayerStats()
    {
        HP = PlayerPrefs.GetInt("Health", HP);
    }
}

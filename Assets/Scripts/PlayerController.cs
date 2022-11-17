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
    [Range(1, 100)][SerializeField] int maxHP;
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
    [Range(0, 1)] [SerializeField] float audioVolume;
    [SerializeField] AudioClip[] playerHurtAud;
    [SerializeField] AudioClip[] playerStepsAud;
    [SerializeField] AudioClip[] playerJumpAud;

    [Header("----- Abilities -----")]
    [SerializeField] public List<ability> abilities = new List<ability>();
    [SerializeField] public int selected;
    [SerializeField] GameObject shootPosition;

    bool[] coolingdown = new bool[5];

    Vector3 hitNormal;
    Vector3 playerVelocity;
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
        audioVolume = PlayerPrefs.GetFloat("volume");
        respawn();
        LoadPlayerStats();
        HPOrig = HP;
        maxHP = HP;
        updatePlayerHUD();
        playerSpeedOrig = playerSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        playerMove();
        sprint();
        if (Input.GetButton("Shoot Ability"))
            ShootAbility();
        abilitySelect();
        gameObject.GetComponent<CharacterController>().Move(new Vector3(hitNormal.x * Time.deltaTime, playerVelocity.y * Time.deltaTime, hitNormal.z * Time.deltaTime));
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
        playerController.Move(playerSpeed * Time.deltaTime * move);

        StartCoroutine(playSteps());

        if (Input.GetButtonDown("Jump") && timesJumped < jumpsMax)
        {
            timesJumped++;
            playerVelocity.y = jumpHeight;
            aud.PlayOneShot(playerJumpAud[Random.Range(0, playerJumpAud.Length)], audioVolume);
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        playerController.Move(playerVelocity * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
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
            aud.PlayOneShot(playerStepsAud[Random.Range(0, playerStepsAud.Length)], audioVolume * 0.1f);
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

            aud.PlayOneShot(playerHurtAud[Random.Range(0, playerHurtAud.Length - 1)], audioVolume);

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

    public int GetHealth()
    {
        return HP;
    }

    public void healPlayer(int healing)
    {
        if (HP < maxHP)
        {
            HP += healing;
            if (HP > maxHP)
                HP = maxHP;
            updatePlayerHUD();
        }
    }

    public void updatePlayerHUD()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / (float)HPOrig;
        gameManager.instance.playerCurrentHPText.text = HP.ToString("F0");
        gameManager.instance.playerMaxHPText.text = maxHP.ToString("F0");
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
        HPOrig = maxHP = HP = 20;
        playerSpeed *= 1.3f;
        jumpHeight *= 1.2f;
        gravityValue = 20;
        sprintMultiplier = 2f;
        jumpsMax *= 3;
        updatePlayerHUD();
    }

    public void ShootAbility()
    {
        if (selected == 0 && coolingdown[selected] != true && HP > abilities[selected].HPcost)
        {
            Debug.Log(coolingdown[selected]);
            coolingdown[selected] = true;
            StartCoroutine(shoot0());
        }            
        else if (selected == 1 && coolingdown[selected] != true && HP > abilities[selected].HPcost)
        {
            Debug.Log(coolingdown[selected]);
            coolingdown[selected] = true;
            StartCoroutine(shoot1());
        }
        else if (selected == 2 && coolingdown[selected] != true && HP > abilities[selected].HPcost)
        {
            Debug.Log(coolingdown[selected]);
            coolingdown[selected] = true;
            StartCoroutine(shoot2());
        }
        else if (selected == 3 && coolingdown[selected] != true && HP > abilities[selected].HPcost)
        {
            Debug.Log(coolingdown[selected]);
            coolingdown[selected] = true;
            StartCoroutine(shoot3());
        }
        else if (selected == 4 && coolingdown[selected] != true && HP > abilities[selected].HPcost)
        {
            Debug.Log(coolingdown[selected]);
            coolingdown[selected] = true;
            StartCoroutine(shoot4());
        }
    }


    
    IEnumerator shoot0() // Separate co-routines for each ability to have their own cooldown timer
    {
        Debug.Log(selected);
        int slctd = selected;

        gameManager.instance.abilityBar.cooldown(slctd);

        HP -= abilities[slctd].HPcost;
        updatePlayerHUD();

        Instantiate(abilities[slctd].bullet, shootPosition.transform.position, shootPosition.transform.rotation);

        yield return new WaitForSeconds(abilities[slctd].cooldown);

        coolingdown[slctd] = false;
    }
    IEnumerator shoot1()
    {
        Debug.Log(selected);
        int slctd = selected;

        gameManager.instance.abilityBar.cooldown(slctd);

        HP -= abilities[slctd].HPcost;
        updatePlayerHUD();

        Instantiate(abilities[slctd].bullet, shootPosition.transform.position, shootPosition.transform.rotation);

        yield return new WaitForSeconds(abilities[slctd].cooldown);

        coolingdown[slctd] = false;
    }
    IEnumerator shoot2()
    {
        Debug.Log(selected);
        int slctd = selected;

        gameManager.instance.abilityBar.cooldown(slctd);

        HP -= abilities[slctd].HPcost;
        updatePlayerHUD();

        Instantiate(abilities[slctd].bullet, shootPosition.transform.position, shootPosition.transform.rotation);

        yield return new WaitForSeconds(abilities[slctd].cooldown);

        coolingdown[slctd] = false;
    }

    IEnumerator shoot3()
    {
        Debug.Log(selected);
        int slctd = selected;

        gameManager.instance.abilityBar.cooldown(slctd);

        HP -= abilities[slctd].HPcost;
        updatePlayerHUD();

        Instantiate(abilities[slctd].bullet, shootPosition.transform.position, shootPosition.transform.rotation);

        yield return new WaitForSeconds(abilities[slctd].cooldown);

        coolingdown[slctd] = false;
    }

    IEnumerator shoot4()
    {
        Debug.Log(selected);
        int slctd = selected;

        gameManager.instance.abilityBar.cooldown(slctd);

        HP -= abilities[slctd].HPcost;
        updatePlayerHUD();

        Instantiate(abilities[slctd].bullet, shootPosition.transform.position, shootPosition.transform.rotation);

        yield return new WaitForSeconds(abilities[slctd].cooldown);
        coolingdown[slctd] = false;
    }


    public void abilitySelect()
    {
        if (Input.GetButtonDown("Ability1") && abilities[0] != null)
        {
            selected = 0;
            gameManager.instance.abilityBar.updateAbilities();
        }
        else if (Input.GetButtonDown("Ability2") && abilities[1] != null)
        {
            selected = 1;
            gameManager.instance.abilityBar.updateAbilities();
        }
        else if (Input.GetButtonDown("Ability3") && abilities[2] != null)
        {
            selected = 2;
            gameManager.instance.abilityBar.updateAbilities();
        }
        else if (Input.GetButtonDown("Ability4") && abilities[3] != null)
        {
            selected = 3;
            gameManager.instance.abilityBar.updateAbilities();
        }
        else if (Input.GetButtonDown("Ability5") && abilities[4] != null)
        {
            selected = 4;
            gameManager.instance.abilityBar.updateAbilities();
        }
    }
    
    public void takeEffect(effect efct)
    {

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

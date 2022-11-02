using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Component -----")]
    [SerializeField] CharacterController playerController;

    [Header("----- Player Stats -----")]
    [Range(1, 100)] [SerializeField] int HP;
    [Range(1, 20)] [SerializeField] float playerSpeed;
    [Range(1.1f, 2f)] [SerializeField] float sprintMultiplier;
    [Range(5, 15)] [SerializeField] float jumpHeight;
    [Range(15, 35)] [SerializeField] float gravityValue;
    [Range(1, 5)] [SerializeField] int jumpsMax;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] playerHurtAud;
    [Range(0, 1)] [SerializeField] float playerHurtAudVol;
    [SerializeField] AudioClip[] playerStepsAud;
    [Range(0, 1)] [SerializeField] float playerStepsAudVol;
    [SerializeField] AudioClip[] playerJumpAud;
    [Range(0, 1)] [SerializeField] float playerJumpAudVol;

    [Header("----- Abilities -----")]
    [SerializeField] public List<ability> abilities = new List<ability>();
    [SerializeField] int selected;
    [SerializeField] GameObject shootPosition;

    bool[] cooldown = new bool[4];
    Vector3 playerVelocity;
    private int timesJumped;
    int HPOrig;
    float playerSpeedOrig;
    bool isSprinting;
    bool playingSteps;

    private void Start()
    {
        HPOrig = HP;
        playerSpeedOrig = playerSpeed;
        respawn();
    }


    // Update is called once per frame
    void Update()
    {
        playerMove();
        sprint();
        StartCoroutine(shoot0());
        StartCoroutine(shoot1());
        StartCoroutine(shoot2());
        StartCoroutine(shoot3());
        abilitySelect();
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
            if (isSprinting) {
                yield return new WaitForSeconds(0.225f);
            } else {
                yield return new WaitForSeconds(0.3f);
            }
            playingSteps = false;
        }
    }

    public void takeDamage(int damage)
    {
        HP -= damage;

        aud.PlayOneShot(playerHurtAud[Random.Range(0, playerHurtAud.Length-1)], playerHurtAudVol);

        updatePlayerHUD();
        StartCoroutine(gameManager.instance.playerDamage());
        
        if(HP <= 0)
        {
            gameManager.instance.playerDamageFlash.SetActive(false);
            gameManager.instance.playerDeadMenu.SetActive(true);
            gameManager.instance.cursorLockPause();
        }
    }
    public void takeEffect(effect efct)
    {

    }

    public void updatePlayerHUD()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / (float)HPOrig;
    }

    public void respawn()
    {
        playerController.enabled = false;
        gameManager.instance.playerDeadMenu.SetActive(false);
        HP = HPOrig;
        updatePlayerHUD();
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

    // Change cost based on HP and check for HP amt
    IEnumerator shoot0() // Slot 0 ability
    {
        if (Input.GetButton("Shoot Ability") && selected == 0 && cooldown[selected] != true && HP > abilities[selected].HPcost)
        {
            cooldown[selected] = true;

            HP -= abilities[selected].HPcost;
            updatePlayerHUD();

            Instantiate(abilities[selected].bullet, shootPosition.transform.position, transform.rotation);

            yield return new WaitForSeconds(abilities[selected].cooldown);

            cooldown[selected] = false;
        }
    }IEnumerator shoot1() 
    {
        if (Input.GetButton("Shoot Ability") && selected == 1 && cooldown[selected] != true && HP > abilities[selected].HPcost)
        {
            cooldown[selected] = true;

            HP -= abilities[selected].HPcost;
            updatePlayerHUD();

            Instantiate(abilities[selected].bullet, shootPosition.transform.position, transform.rotation);

            yield return new WaitForSeconds(abilities[selected].cooldown);

            cooldown[selected] = false;
        }
    }IEnumerator shoot2() 
    {
        if (Input.GetButton("Shoot Ability") && selected == 2 && cooldown[selected] != true && HP > abilities[selected].HPcost)
        {
            cooldown[selected] = true;

            HP -= abilities[selected].HPcost;
            updatePlayerHUD();

            Instantiate(abilities[selected].bullet, shootPosition.transform.position, transform.rotation);

            yield return new WaitForSeconds(abilities[selected].cooldown);

            cooldown[selected] = false;
        }
    }IEnumerator shoot3()
    {
        if (Input.GetButton("Shoot Ability") && selected == 3 && cooldown[selected] != true && HP > abilities[selected].HPcost)
        {
            cooldown[selected] = true;

            HP -= abilities[selected].HPcost;
            updatePlayerHUD();

            Instantiate(abilities[selected].bullet, shootPosition.transform.position, transform.rotation);

            yield return new WaitForSeconds(abilities[selected].cooldown);

            cooldown[selected] = false;
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
        else if (Input.GetButtonDown("Ability4") && abilities[3] != null)
        {
            selected = 3;
        }
    } 

}

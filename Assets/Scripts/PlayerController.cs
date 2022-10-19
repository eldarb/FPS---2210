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

    //[Header("----- Gun Stats -----")]
    //[SerializeField] float shootRate;
    //[SerializeField] int shootDistance;
    //[SerializeField] int shootDamage;
    //[SerializeField] GameObject gunModel;
    //[SerializeField] List<gunStats> gunStat = new List<gunStats>();

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] playerHurtAud;
    [Range(0, 1)] [SerializeField] float playerHurtAudVol;
    [SerializeField] AudioClip[] playerStepsAud;
    [Range(0, 1)] [SerializeField] float playerStepsAudVol;
    [SerializeField] AudioClip[] playerJumpAud;
    [Range(0, 1)] [SerializeField] float playerJumpAudVol;

    Vector3 playerVelocity;
    private int timesJumped;
    //bool isShooting;
    //int selectGun;
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
        //StartCoroutine(shoot());
        //gunSelect();
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
        Debug.Log(playerVelocity.y);
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

    //IEnumerator shoot()
    //{
    //    if (gunStat.Count > 0 && Input.GetButton("Shoot") && !isShooting)
    //    {
    //        isShooting = true;
    //        RaycastHit hit;
    //        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
    //        {
    //            if (hit.collider.GetComponent<IDamage>() != null)
    //                hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
    //        }
    //        yield return new WaitForSeconds(shootRate);
    //        isShooting = false;
    //    }
    //}

    //public void gunPickUp(gunStats stats)
    //{
    //    shootRate = stats.shootRate;
    //    shootDistance = stats.shootDistance;
    //    shootDamage = stats.shootDamage;

    //    gunModel = Instantiate(stats.gunModel, transform);
    //    gunStat.Add(stats);
    //}

    //void gunSelect()
    //{
    //    if (gunStat.Count > 1)
    //    {
    //        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectGun < gunStat.Count - 1)
    //        {
    //            selectGun++;
    //            changeGun();
    //        }
    //        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectGun > 0)
    //        {
    //            selectGun--;
    //            changeGun();
    //        }
    //    }
    //}

    //void changeGun()
    //{
    //    shootRate = gunStat[selectGun].shootRate;
    //    shootDistance = gunStat[selectGun].shootDistance;
    //    shootDamage = gunStat[selectGun].shootDamage;
    //    gunModel.GetComponent<MeshFilter>().sharedMesh = gunStat[selectGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
    //    gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStat[selectGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    //}

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
        HP *= 2;
        HPOrig = HP;
        updatePlayerHUD();
        playerSpeed *= 1.3f;
        jumpHeight *= 1.2f;
        gravityValue = 20;
        jumpsMax *= 3;
    }
}

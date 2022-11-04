using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyAI : MonoBehaviour, IDamage
{

    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Animator anim;
    [SerializeField] Collider col;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] Slider healthBarSlider;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int sightRange;
    [SerializeField] int speedChase; // new10/16/22 > 
    [SerializeField] int animLerpSpeed; // new10/16/22 > 
    [SerializeField] int viewAngle; // new10/16/22 > 
    [SerializeField] int roamDist; // new10/16/22 > 
    [SerializeField] GameObject headPosition;// new10/16/22
    [SerializeField] int numSouls;

    [Header("----- Enemy Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shootPosition;

    [Header("----- Enemy Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] enemyHurtAud;
    [Range(0, 1)] [SerializeField] float enemyHurtAudVol;
    [SerializeField] AudioClip[] enemyStepsAud;
    [Range(0, 1)] [SerializeField] float enemyStepsAudVol;
    [SerializeField] AudioClip bowSound;
    [SerializeField] AudioClip swordSound;
    [Range(0, 1)] [SerializeField] float enemyGunShotAudVol;

    bool isShooting;
    int damageDealt;
    public bool playerInRange;
    Vector3 playerDirection;
    float stoppingDistOrig;
    Vector3 startingPos;
    float angle;
    float speedPatrol;
    bool playingSteps;

    // Start is called before the first frame update
    void Start()
    {
       
        //setHealthBar();
        gameManager.instance.enemyCountText.text = gameManager.instance.enemyCount.ToString("F0");
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
        speedPatrol = agent.speed;
        roam();
    }

    // Update is called once per frame
    void Update()//updated from lecture 5 10/16/22
    {
        if (HP > 0)
        {
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animLerpSpeed));
            if (agent.enabled)
            {
                
                if (playerInRange)
                {
                    playerDirection = gameManager.instance.player.transform.position - headPosition.transform.position;
                    angle = Vector3.Angle(playerDirection, transform.forward);
                    canSeePlayer();
                    StartCoroutine(playSteps());
                }

                if (agent.remainingDistance < 0.1f && agent.destination != gameManager.instance.player.transform.position)
                {
                    roam();
                    StartCoroutine(playSteps());
                }
            }
        }
    }

    void roam() //new10/16/22
    {
        agent.stoppingDistance = 0;
        agent.speed = speedPatrol;

        Vector3 randomDirection = Random.insideUnitSphere * roamDist;
        randomDirection += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 1, 1);
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(randomDirection, path);
        agent.CalculatePath(hit.position, path);
        agent.SetPath(path);
    }

    void canSeePlayer() //new10/16/22
    {
        RaycastHit hit;
        if (Physics.Raycast(headPosition.transform.position, playerDirection, out hit, sightRange))
        {
            Debug.DrawRay(headPosition.transform.position, playerDirection);


            if (hit.collider.CompareTag("Player") && angle <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.speed = speedChase;
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (!isShooting)
                    StartCoroutine(shoot());

                if (agent.remainingDistance < agent.stoppingDistance)
                    facePlayer();
            }
            else
                agent.stoppingDistance = 0;
        }
    }

    void facePlayer()
    {
        playerDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * facePlayerSpeed);
    }

    public void takeDamage(int dmg)
    {
        if (!gameManager.instance.pauseMenu.activeSelf && !gameManager.instance.winMenu.activeSelf && !gameManager.instance.playerDeadMenu.activeSelf && !gameManager.instance.abilityMenu.activeSelf)
        {
            HP -= dmg;

            if (HP <= 0)
            {
                if(gameObject.CompareTag("King"))
                {
                    gameManager.instance.CheckWinCondition();
                }
                gameManager.instance.checkEnemyTotal();
                agent.enabled = false;
                col.enabled = false;
                anim.SetBool("Dead", true);
                gameManager.instance.playerScript.soulCount += numSouls;
                
            }
            else
                StartCoroutine(flashDamage());
        }
    }

    private void setHealthBar()
    {
        healthBarSlider.value = ((float)HP % (float)HP) * 100;
    }

   


    IEnumerator shoot()
    {
        isShooting = true;

        if (gameObject.CompareTag("Range"))
        {
            aud.PlayOneShot(bowSound, enemyGunShotAudVol);
            anim.SetTrigger("Shoot");
        }
        else if (gameObject.CompareTag("Melee"))
        {
            aud.PlayOneShot(swordSound, enemyGunShotAudVol);
            anim.SetTrigger("Attack");
        }

            Instantiate(bullet, shootPosition.transform.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator playSteps()
    {
        if(agent.speed != 0 && !playingSteps)
        {
            playingSteps = true;
            aud.PlayOneShot(enemyStepsAud[Random.Range(0, enemyStepsAud.Length - 1)], enemyStepsAudVol);
            yield return new WaitForSeconds(0.75f);
            playingSteps = false;
        }
    }

    IEnumerator flashDamage()
    {
        aud.PlayOneShot(enemyHurtAud[Random.Range(0, enemyHurtAud.Length - 1)], enemyHurtAudVol);
        anim.SetTrigger("Damage");
        model.material.color = Color.red;
        agent.enabled = false;
        yield return new WaitForSeconds(0.5f);
        model.material.color = Color.white;
        agent.enabled = true;
        agent.SetDestination(gameManager.instance.player.transform.position);
    }

    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
        agent.stoppingDistance = 0;
    }

    public void takeEffect(effect efct)
    {
        if (efct.lastingdmg == true)
        {
            StartCoroutine(LastingDamage(efct));
        }
        else
        {
            StartCoroutine(NoDamage(efct));
        }
    }
    IEnumerator LastingDamage(effect efct)
    {
        for (int i = 0; i < efct.efctdur; i++)
        {
            yield return new WaitForSeconds(efct.wait);
            takeDamage(efct.efctdmg);
        }
    }
    IEnumerator NoDamage(effect efct)
    {
        for (int i = 0; i < efct.efctdur; i++)
        {
            int temp = speedChase;
            speedChase = efct.lowspeed;
            yield return new WaitForSeconds(efct.wait);
            speedChase = temp;
        }
    }
}

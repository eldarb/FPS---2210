using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{

    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Animator anim;
    [SerializeField] Collider col;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int sightRange;
    [SerializeField] int speedChase; // new10/16/22 > 
    [SerializeField] int animLerpSpeed; // new10/16/22 > 
    [SerializeField] int viewAngle; // new10/16/22 > 
    [SerializeField] int roamDist; // new10/16/22 > 
    [SerializeField] GameObject headPosition;// new10/16/22

    [Header("----- Enemy Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shootPosition;

    bool isShooting;
    public bool playerInRange;
    Vector3 playerDirection;
    float stoppingDistOrig;
    Vector3 startingPos;
    float angle;
    float speedPatrol;

    // Start is called before the first frame update
    void Start()
    {
        
        gameManager.instance.enemyCount++; //take out after SPAWNER script created and implemented 10/16/22
        gameManager.instance.enemyCountText.text = gameManager.instance.enemyCount.ToString("F0");
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
        roam();
    }

    // Update is called once per frame
    void Update()//updated from lecture 5 10/16/22
    {
        if (HP > 0)
        {
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animLerpSpeed));
            //agent.SetDestination(gameManager.instance.player.transform.position);
            if (agent.enabled)
            {
                playerDirection = gameManager.instance.player.transform.position - headPosition.transform.position;
                angle = Vector3.Angle(playerDirection, transform.forward);

                canSeePlayer();
            }

            if (agent.remainingDistance < 0.1f && agent.destination != gameManager.instance.player.transform.position)
                roam();
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

        agent.CalculatePath(hit.position, path);
        agent.SetPath(path);
    }

    void canSeePlayer() //new10/16/22
    {
        RaycastHit hit;
        if (Physics.Raycast(headPosition.transform.position, playerDirection, out hit, sightRange))
        {
            Debug.DrawRay(headPosition.transform.position, playerDirection);
        }

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
    }

    void facePlayer()
    {
        playerDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * facePlayerSpeed);
    }

    public void takeDamage(int dmg)
    {
        //if (!gameManager.instance.pauseMenu.activeSelf && !gameManager.instance.winMenu.activeSelf && !gameManager.instance.playerDeadMenu.activeSelf)
        //{
        HP -= dmg;

        if (HP <= 0)
        {
            gameManager.instance.checkEnemyTotal();
            agent.enabled = false;
            col.enabled = false;
            anim.SetBool("Dead", true);
        }
        else
            StartCoroutine(flashDamage());
    }

    IEnumerator shoot()
    {
        isShooting = true;

        anim.SetTrigger("Shoot");
        Instantiate(bullet, shootPosition.transform.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator flashDamage()
    {
        anim.SetTrigger("Damage");
        model.material.color = Color.red;
        agent.enabled = false;
        yield return new WaitForSeconds(0.25f);
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
}

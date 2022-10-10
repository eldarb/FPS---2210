using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent enemyAgent;
    [SerializeField] Renderer model;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int targetingSpeed;
    [SerializeField] int sightRange;
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shootPosition;

    bool isShooting;
    bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyAgent.enabled && playerInRange)
        {
            enemyAgent.SetDestination(gameManager.instance.player.transform.position);

            if (!isShooting)
                StartCoroutine(shoot());
        }
    }

    public void takeDamage(int damage)
    {
        HP -= damage;
        //StartCoroutine();

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(bullet, shootPosition.transform.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        enemyAgent.enabled = false;
        yield return new WaitForSeconds(0.25f);
        model.material.color = Color.white;
        enemyAgent.enabled = true;
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInRange = true;
    //    }
    //}

   //void OnTriggerExit(Collider other)
   // {
   //     if (other.CompareTag("Player"))
   //     {
   //         playerInRange = false;
   //     }
   // }
}

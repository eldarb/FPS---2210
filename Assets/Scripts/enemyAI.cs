using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] int HP;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void takeDamage(int damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}

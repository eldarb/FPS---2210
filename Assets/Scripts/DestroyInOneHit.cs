using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInOneHit : MonoBehaviour, IDamage
{
    public void takeDamage(int damage)
    {
        Destroy(gameObject);
    }
    public void takeEffect(effect efct)
    {

    }
}

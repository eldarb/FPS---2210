using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ability : ScriptableObject
{
    public float cooldown;
    public int shootDist;
    public int shootDmg;
    // public Effect effect;
    public GameObject abilityModel;
    public AudioClip sound;
    public GameObject activateEffect;
}

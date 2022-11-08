using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    public string gunName;
    public string gunType;
    public float shootRate;
    public int shootDistance;
    public int shootDamage;
    public int ammoCount;
    public GameObject gunModel;
    public AudioClip sound;
    public AudioClip hitSound;
    public GameObject gitEffect;
    public GameObject muzzleEffect;

}

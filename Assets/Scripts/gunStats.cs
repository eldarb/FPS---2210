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
    public GameObject gunModel;
}

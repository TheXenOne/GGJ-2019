using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackType", menuName = "GGJ/Create Attack Type", order = 2)]
public class AttackType : ScriptableObject
{
    public float BaseDamage;
    public float Modifier;
    public float Multiplier;
    public float Damage { get => BaseDamage * Multiplier + Modifier}    
}
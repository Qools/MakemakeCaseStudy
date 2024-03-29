using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FightValues", menuName = "FightValues")]
public class FightValues : ScriptableObject
{
    public float healthPoints;

    public float attackCooldown;

    public float heavyAttackDamage;
    public float heavyAttackChange;

    public float lightAttackDamage;
    public float lightAttackChance;

    public float stunChance;
    public float stunDuration;
}

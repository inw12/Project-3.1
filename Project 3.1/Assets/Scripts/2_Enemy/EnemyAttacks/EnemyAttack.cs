using System;
using UnityEngine;
public struct EnemyAttackContext
{
    public Enemy Enemy;
    public Transform Origin;
    public ProjectilePool ProjectilePool;
    public LayerMask PlayerLayer;
}
public abstract class EnemyAttack : ScriptableObject
{
    [Header("Attack Info")]
    public string attackName;
    public int attackID;

    [Header("Attack Selection Settings")]
    public float baseWeight = 1f;
    public float weightIncrement = 0.5f;        // how much to gain when not used
    public float weightDecrement = 2f;          // how much to lose when used
    [NonSerialized] public float currentWeight; // current weight value of THIS attack

    void OnEnable() => currentWeight = baseWeight;

    public void IncreaseWeight() => currentWeight += weightIncrement;
    public void DecreaseWeight() => currentWeight = Mathf.Max(baseWeight - weightDecrement, 0f);

    // Attack implementation done by each attack
    public abstract void Attack(EnemyAttackContext context);
}

public abstract class EnemyRangedAttack : EnemyAttack {}
public abstract class EnemyFocusAttack  : EnemyAttack {}
public abstract class EnemyMeleeAttack  : EnemyAttack {}
public abstract class EnemyZoneAttack   : EnemyAttack {}
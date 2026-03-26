using UnityEngine;
public abstract class EnemyRangedAttack : EnemyAttack
{
    // Attack Stats
    [Header("Stats")]
    [SerializeField] protected float damage;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float range;
    [Space]
    [SerializeField] protected Enemy enemy;
    [SerializeField] protected Transform projectileSpawn;
    [SerializeField] protected ProjectilePool projectilePool;

    protected float _fireTimer;

    public override abstract void Attack();
}
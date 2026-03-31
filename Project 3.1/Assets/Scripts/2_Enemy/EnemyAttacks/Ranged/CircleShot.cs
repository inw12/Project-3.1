/// * Shoots a circle of projectiles outward around a given point
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy Attacks/Ranged/CircleShot")]
public class CircleShot : EnemyRangedAttack
{
    [Header("Stats")]
    public float damage;
    public float speed;
    public float range;
    public int projectileCount;

    public override void Attack(EnemyAttackContext context)
    {
        if (!attackActive) attackActive = true;

        var angleStep = 360f / projectileCount;
        for (int i = 0; i < projectileCount; i++)
        {
            var angle = i * angleStep;
            var rad = angle * Mathf.Deg2Rad;
            var direction = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad));

            var stats = new ProjectileStats
            {
                Damage = damage,
                Speed = speed,
                Range = range,
                Direction = direction.normalized
            };

            context.ProjectilePool.Get(stats, context.Origin);
        }

        attackActive = false;
    }
}

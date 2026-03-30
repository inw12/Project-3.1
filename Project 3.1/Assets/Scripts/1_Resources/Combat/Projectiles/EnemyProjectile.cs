using UnityEngine;
public abstract class EnemyProjectile : Projectile
{
    protected int playerHurtbox;
    protected int playerParrybox;
    protected LayerMask playerLayerMask;

    protected virtual void Awake()
    {
        playerHurtbox = LayerMask.NameToLayer("PlayerHurtbox");
        playerParrybox = LayerMask.NameToLayer("PlayerParrybox");
        playerLayerMask = (1 << playerHurtbox) | (1 << playerParrybox);
    }

    public override void OnHit(Collider other)
    {
        var layer = other.gameObject.layer;

        Debug.Log(layer);

        if (layer == playerParrybox)
            OnParryboxHit(other);
        else if (layer == playerHurtbox)
            OnHurtboxHit(other);
    }
    protected void OnHurtboxHit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable e))
        {
            e.DecreaseHealth(_stats.Damage);
            _pool.Release(gameObject);
        }
    }    
    protected void OnParryboxHit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IParry e))
        {
            e.TriggerParry();
            _pool.Release(gameObject);
        }
    }   

    protected abstract override void Move();
}

using UnityEngine;
public abstract class EnemyAttack : MonoBehaviour
{
    // Attack ID (identification for state machine and animator control)
    [SerializeField] protected int attackID;

    protected bool _attackActive;

    // Hit detection
    protected int playerHurtbox;
    protected int playerParrybox;
    protected LayerMask playerLayerMask;
    protected readonly Collider[] _hitBuffer = new Collider[10];

    protected virtual void Awake()
    {
        playerHurtbox = LayerMask.NameToLayer("PlayerHurtbox");
        playerParrybox = LayerMask.NameToLayer("PlayerParrybox");
        playerLayerMask = (1 << playerHurtbox) | (1 << playerParrybox);
    }

    protected void HandleHit(Collider hit, float damage)
    {
        var layer = hit.gameObject.layer;
        if (layer == playerParrybox)
            OnParryboxHit();
        else if (layer == playerHurtbox)
            OnHurtboxHit();
    }
    protected virtual void OnHurtboxHit() {}    
    protected virtual void OnParryboxHit() {}   

    // Called in 'Update()' in 'Enemy.cs'
    public abstract void Attack();  

    public virtual void Cancel() {}

    public int GetAttackID() => attackID;
}

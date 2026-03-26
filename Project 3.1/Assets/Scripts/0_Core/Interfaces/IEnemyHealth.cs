/// * EXCLUSIVE for 'Enemy' objects
/// 
/// * These objects have:
///     - HP Value  (current & max)
///     - DEF Value (current & max)
///     - Damage Reduction %
/// 
/// *** When taking damage, these objects will distribute the incoming
///     damage by 'DamageReduction'% between HP and DEF values
public interface IEnemyHealth
{
    float MaxHealth         { get; }
    float CurrentHealth     { get; }

    float MaxDefense        { get; }
    float CurrentDefense    { get; }

    float DamageReduction   { get; }

    bool IsAlive            { get; }

    void DecreaseHealth(float amount);
    void IncreaseHealth(float amount);
    void IncreaseDefense(float amount);
    void OnDeath();
}

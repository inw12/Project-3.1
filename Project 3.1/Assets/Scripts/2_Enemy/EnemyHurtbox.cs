using UnityEngine;
public class EnemyHurtbox : MonoBehaviour, IEnemyHealth
{
    [SerializeField] private EnemyHitFeedback hitFeedback;

    private float _maxHealth;
    private float _maxDefense;
    private float _defenseDamageReduction;

    private float _currentHealth;
    private float _currentDefense;
    private bool _isAlive;

    // Interface Variables
    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;
    public float MaxDefense => _maxDefense;
    public float CurrentDefense => _currentDefense;
    public float DamageReduction => _defenseDamageReduction;
    public bool IsAlive => _isAlive;

    public void Initialize(float health, float defense, float damageReduction)
    {
        _maxHealth = health;
        _maxDefense = defense;
        _defenseDamageReduction = damageReduction;

        _currentHealth = _maxHealth;
        _currentDefense = _maxDefense;
        _isAlive = _currentHealth > 0f;

        if (hitFeedback) hitFeedback.Initialize();
    }

    // Called by 'Enemy.cs' in 'Update()'
    public void UpdateState(ref EnemyState state)
    {
        // Only update state if a change has occured
        if (state.CurrentDefense != _currentDefense || state.CurrentHealth != _currentHealth)
        {
            state.CurrentHealth = _currentHealth;
            state.CurrentDefense = _currentDefense;
        }
    }

    // Called by 'Enemy.cs' in 'LateUpdate()'
    public void UpdateHitFeedback(float deltaTime)
    {
        hitFeedback.UpdateEnemyModel(deltaTime);
    }

    #region *-- 'IEnemyHealth' Methods --------------------*
    public void DecreaseHealth(float amount)
    {
        // Calculate damage taken
        var amountToDefense = amount * _defenseDamageReduction;
        var amountToHealth = amount - amountToDefense;

        // Apply to DEF
        _currentDefense -= amountToDefense;
        _currentDefense = Mathf.Clamp(_currentDefense, 0f, _maxDefense);

        // Apply to HP
        _currentHealth -= amountToHealth;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);

        // Check for death
        _isAlive = _currentHealth > 0f;

        // Trigger Hit Feedback
        if (hitFeedback) hitFeedback.TriggerHitFeedback();
    }
    public void IncreaseDefense(float amount)
    {
        _currentDefense += amount;
        _currentDefense = Mathf.Clamp(_currentDefense, 0f, _maxDefense);
    }
    public void IncreaseHealth(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
    }
    public void OnDeath() {}
    #endregion
}

/// * ENEMY Parent Class Responsibilities:
///     - HP/DEF Management
///     - Signals to 'CombatManager' for scene changes
///     - State Machine Management (enemy AI)
/// 
/// * Child Class Responsibilities:
///     - "What kinds of attacks?"
///     - "How does this enemy move?"
using System.Collections;
using UnityEngine;

public struct EnemyState
{
    public EnemyAction CurrentAction;
    public int CurrentAttack;
    public bool InHitstun;
}
public enum EnemyAction
{
    Idle            = 0,
    Move            = 1,
    AttackRanged    = 2,
    AttackFocus     = 3,
    AttackMelee     = 4,
    AttackZone      = 5,
    Knockback       = 6,
    Knockdown       = 7
}

[RequireComponent(typeof(CharacterController), typeof(CapsuleCollider))]
public abstract class Enemy : MonoBehaviour, IEnemyHealth, IHitstunnable, IKnockable
{
    [Header("Stats")]
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float maxDefense = 50f;
    [SerializeField] [Range(0f, 1f)] protected float defenseDamageReduction = 0.9f;
    protected float _currentHealth;
    protected float _currentDefense;
    protected bool _isAlive;
    [Space]
    [SerializeField] protected float moveSpeed = 10f;

    [Header("Parry Phase Trigger")]
    [SerializeField] [Range(0f, 1f)] protected float combatTransitionThreshold = 0.25f;
    [SerializeField] protected Signal transitionSignal;

    [Header("Animations")]
    [SerializeField] protected EnemyAnimationController animationController;
    [SerializeField] protected EnemyHitFeedback hitFeedback;

    [Header("Knockback/Knockdown Settings")]
    [SerializeField] protected float knockbackAmount;
    [SerializeField] protected float knockdownDuration;

    [Header("Attacks")]
    [SerializeField] protected EnemyAttack[] rangedAttacks;
    [SerializeField] protected EnemyAttack[] focusAttacks;
    [SerializeField] protected EnemyAttack[] meleeAttacks;
    [SerializeField] protected EnemyAttack[] zoneAttacks;

    // State Machine
    protected EnemyState _state;
    protected EnemyState _prevState;

    protected float _timeScale;

    protected bool _isKnockable;    // in a state where enemy can be either knocked back or knocked down

    // 'IEnemyHealth' Variables
    public float MaxHealth => maxHealth;
    public float CurrentHealth => _currentHealth;
    public float MaxDefense => maxDefense;
    public float CurrentDefense => _currentDefense;
    public float DamageReduction => defenseDamageReduction;
    public bool IsAlive => _isAlive;

    // 'IHitstunnable' Variables
    public float TimeScale => _timeScale;
    public bool InHitstun => _state.InHitstun;

    // 'IKnockable' Variables
    public float KnockbackAmount => knockbackAmount;
    public float KnockdownDuration => knockdownDuration;
    public bool IsKnockable => _isKnockable;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        _state.CurrentAction = EnemyAction.Idle;
        _state.CurrentAttack = 0;
        _state.InHitstun = false;
        _prevState = _state;

        animationController.Initialize();
        hitFeedback.Initialize();

        _currentHealth = maxHealth;
        _currentDefense = maxDefense;
        _isAlive = _currentHealth > 0f;

        _timeScale = 1f;
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void LateUpdate()
    {
        var deltaTime = Time.deltaTime * _timeScale;

        animationController.UpdateAnimator(_state);

        hitFeedback.UpdateEnemyModel(deltaTime);

        // Update State Machine
        _prevState = _state;
    }

    protected virtual void FixedUpdate()
    {
        
    }

    protected virtual void Move() {}

    protected virtual void Attack() {}

    #region *-- 'IEnemyHealth' Methods --------------------*
    public void DecreaseHealth(float amount)
    {
        // Calculate damage taken
        var amountToDefense = amount * defenseDamageReduction;
        var amountToHealth = amount - amountToDefense;

        // Apply to DEF
        _currentDefense -= amountToDefense;
        _currentDefense = Mathf.Clamp(_currentDefense, 0f, maxDefense);

        // Apply to HP
        _currentHealth -= amountToHealth;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, maxHealth);

        // Check for death
        _isAlive = _currentHealth > 0f;

        // Trigger Hit Feedback
        hitFeedback.TriggerHitFeedback();
    }
    public void IncreaseDefense(float amount)
    {
        _currentDefense += amount;
        _currentDefense = Mathf.Clamp(_currentDefense, 0f, maxDefense);
    }
    public void IncreaseHealth(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, maxHealth);
    }
    public void OnDeath() {}
    #endregion

    #region *-- 'IHitstunnable' Methods -------------------*
    public IEnumerator TriggerHitstun(float duration)
    {
        _timeScale = 0f;
        _state.InHitstun = true;
        yield return new WaitForSeconds(duration);
        _timeScale = 1f;
        _state.InHitstun = false;
    }
    #endregion

    #region *-- 'IKnockable' Methods ----------------------*
    public void TriggerKnockback()
    {
        throw new System.NotImplementedException();
    }
    public void TriggerKnockdown()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    public void SetTimeScale(float t) => _timeScale = t;
}

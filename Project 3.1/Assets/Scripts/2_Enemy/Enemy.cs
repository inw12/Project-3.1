/// * ENEMY Parent Class Responsibilities:
///     - HP/DEF Management
///     - Signals to 'CombatManager' for scene changes
///     - State Machine Management (enemy AI)
/// 
/// * Child Class Responsibilities:
///     - "What kinds of attacks?"
///     - "How does this enemy move?"
using UnityEngine;
using System.Collections;

public struct EnemyState
{
    public float CurrentHealth;
    public float CurrentDefense;
    public bool IsAlive;

    public EnemyAction CurrentAction;
    public int CurrentAttack;
    public Vector3 MovementTarget;

    public bool InHitstun;
    public bool IsKnockable;
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
    Knockdown       = 7,
    InCutscene      = 8
}

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public abstract class Enemy : MonoBehaviour, IHitstunnable, IKnockable
{
    [Header("Game Components")]
    [SerializeField] protected EnemyAnimationController animationController;
    [SerializeField] protected EnemyHurtbox hurtbox;

    [Header("Stats")]
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float maxDefense = 50f;
    [SerializeField] [Range(0f, 1f)] protected float defenseDamageReduction = 0.9f;
    [Space]
    [SerializeField] protected float moveSpeed = 5f;

    [Header("Parry Phase Trigger")]
    [SerializeField] [Range(0f, 1f)] protected float combatTransitionThreshold = 0.25f;
    [SerializeField] protected Signal transitionSignal;

    [Header("Knockback/Knockdown Settings")]
    [SerializeField] protected float knockbackAmount;
    [SerializeField] protected float knockdownDuration;

    [Header("State Machine Control")]
    [SerializeField] protected float stateChangeCooldown = 5f;
    protected float _cooldownTimer;
    protected EnemyAttack _requestedAttack;

    [Header("Attacks")]
    [SerializeField] protected EnemyAttack[] rangedAttacks;
    [SerializeField] protected EnemyAttack[] focusAttacks;
    [SerializeField] protected EnemyAttack[] meleeAttacks;
    [SerializeField] protected EnemyAttack[] zoneAttacks;

    // State Machine
    protected EnemyState _state;
    protected EnemyState _prevState;

    // Local TimeScale
    protected float _timeScale;

    // RigidBody
    protected Rigidbody _rb;

    // 'IEnemyHealth' Variables
    public float MaxHealth => maxHealth;
    public float CurrentHealth => _state.CurrentHealth;
    public float MaxDefense => maxDefense;
    public float CurrentDefense => _state.CurrentDefense;
    public float DamageReduction => defenseDamageReduction;
    public bool IsAlive => _state.IsAlive;

    // 'IHitstunnable' Variables
    public float TimeScale => _timeScale;
    public bool InHitstun => _state.InHitstun;

    // 'IKnockable' Variables
    public float KnockbackAmount => knockbackAmount;
    public float KnockdownDuration => knockdownDuration;
    public bool IsKnockable => _state.IsKnockable;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        // State Machine Initialization
        _state.CurrentHealth = maxHealth;
        _state.CurrentDefense = maxDefense;
        _state.IsAlive = _state.CurrentHealth > 0f;
        _state.CurrentAction = EnemyAction.Idle;
        _state.CurrentAttack = 0;
        _state.InHitstun = false;
        _state.IsKnockable = true;
        _prevState = _state;

        // Enemy Component Initialization
        animationController.Initialize();
        hurtbox.Initialize(_state.CurrentHealth, _state.CurrentDefense, defenseDamageReduction);
        _rb = GetComponent<Rigidbody>();

        _timeScale = 1f;
    }

    protected virtual void Update()
    {
        var deltaTime = Time.deltaTime * _timeScale;

        // Update HP/DEF Stats
        hurtbox.UpdateState(ref _state);

        // Check for combat phase transition
        if (_state.CurrentDefense / maxDefense <= combatTransitionThreshold)
        {
            transitionSignal.Raise();
        }

        // State Machine Control
        _cooldownTimer += deltaTime;
        if (_cooldownTimer >= stateChangeCooldown)
        {
            // Random movement target
            var target = Random.insideUnitSphere * 25;
            target = Vector3.ProjectOnPlane(target, Vector3.up);
            _state.MovementTarget = target;

            _state.CurrentAction = EnemyAction.Move;

            _cooldownTimer = 0f;
        }
    }

    protected virtual void LateUpdate()
    {
        var deltaTime = Time.deltaTime * _timeScale;

        // Update hurtbox HitFeedback
        hurtbox.UpdateHitFeedback(deltaTime);

        // Update Animator
        animationController.UpdateAnimator(_state);

        // Debug Messages
        if (_state.CurrentHealth != _prevState.CurrentHealth || _state.CurrentDefense != _prevState.CurrentDefense)
        {
            Debug.Log("HP: " + _state.CurrentHealth + " / " + maxHealth);
            Debug.Log("DEF: " + _state.CurrentDefense + " / " + maxDefense);
        }

        // Update State Machine
        _state.IsKnockable = _state.CurrentAction is EnemyAction.Idle or EnemyAction.Move;
        _prevState = _state;
    }

    protected virtual void FixedUpdate()    // Reserved for movement implementation
    {
        var deltaTime = Time.fixedDeltaTime * _timeScale;

        // Movement Implementation
        if (_state.CurrentAction is EnemyAction.Move)
        {
            // If destination reached, exit movement state
            if (_rb.position == _state.MovementTarget)
            {
                _state.CurrentAction = EnemyAction.Idle;
                return;
            }

            // Rotate towards movement target
            var direction = (_state.MovementTarget - Vector3.ProjectOnPlane(transform.position, Vector3.up)).normalized;
            transform.rotation = Quaternion.LookRotation(direction);

            // Calculate amount to move this frame
            var next = Vector3.MoveTowards
            (
                _rb.position,
                _state.MovementTarget,
                1f - Mathf.Exp(-moveSpeed * deltaTime)
            );

            // Apply movement
            _rb.MovePosition(next);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_state.MovementTarget != Vector3.zero) Gizmos.DrawSphere(_state.MovementTarget, 0.5f);
    }

    protected virtual void TryEnterNewState()
    {
        
    }

    protected virtual void Move() {}
    protected virtual void Attack() {}

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

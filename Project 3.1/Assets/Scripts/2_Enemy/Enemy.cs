using System.Collections;
using UnityEngine;
[RequireComponent(typeof(CharacterController), typeof(CapsuleCollider))]
public abstract class Enemy : MonoBehaviour, IEnemyHealth, IHitstunnable, IKnockbackable
{
    [Header("Stats")]
    [SerializeField] protected float maxHealth = 50f;
    [SerializeField] protected float maxDefense = 100f;
    [SerializeField] [Range(0f, 1f)] protected float damageReduction = 0.5f;
    [SerializeField] protected float moveSpeed = 10f;
    protected float _currentHealth;
    protected float _currentDefense;

    protected float _timeScale;
    protected CapsuleCollider _hurtbox;
    protected bool _inHitstun;
    protected bool _isAlive;

    #region *--- Interface Variables --------------------*
    // HP
    public float MaxHealth => maxHealth;
    public float MaxDefense => maxDefense;
    // DEF
    public float CurrentHealth => _currentHealth;
    public float CurrentDefense => _currentDefense;
    // Damage Reduction
    public float DamageReduction => damageReduction;
    // IsAlive (?)
    public bool IsAlive => _isAlive;
    // InHitstun (?)
    public bool InHitstun => _inHitstun;
    #endregion

    protected virtual void Start()
    {
        _currentHealth = maxHealth;
        _currentDefense = maxDefense;

        _timeScale = 1;
        _hurtbox = GetComponent<CapsuleCollider>();
        _inHitstun = false;
    }

    protected virtual void Update()
    {
        var deltaTime = Time.deltaTime * _timeScale;
    }

    #region *--- IEnemyHealth Methods ---------------*
    public virtual void DecreaseHealth(float amount)
    {
        // Calculate damage distribution
        var defenseReduction = amount * damageReduction;
        var healthReduction = amount - defenseReduction;

        // Apply changes
        _currentDefense -= defenseReduction;
        _currentHealth -= healthReduction;

        // Clamp values
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, maxHealth);
        _currentDefense = Mathf.Clamp(_currentDefense, 0f, maxDefense);

        Debug.Log("HP: " + _currentHealth);
        Debug.Log("DEF: " + _currentDefense);
    }
    public virtual void IncreaseHealth(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, maxHealth);
    }
    public virtual void IncreaseDefense(float amount)
    {
        _currentDefense += amount;
        _currentDefense = Mathf.Clamp(_currentDefense, 0f, maxDefense);
    }
    public virtual void OnDeath()
    {
        throw new System.NotImplementedException();
    }
    #endregion


    #region *--- IHitstunnable Methods ---------------*
    public void TriggerHitstun(float duration)
    {
        // ** add a conditional here later to ensure enemy can be hitstunned *
        StartCoroutine(HitstunRoutine(duration));
    }
    private IEnumerator HitstunRoutine(float duration)
    {
        _timeScale = 0f;
        yield return new WaitForSeconds(duration);
        _timeScale = 1f;
    }
    #endregion

    #region *--- IKnockbackable Methods ---------------*
    public void Knockback(Vector3 direction, float amount, float duration)
    {
        
    }
    #endregion

    public virtual void SetTimeScale(float timeScale) => _timeScale = timeScale;
}

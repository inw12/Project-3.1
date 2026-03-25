using UnityEngine;
public struct EnemyState
{
    public EnemyAction CurrentAction;       // what action is CURRENTLY happening
    public EnemyAttackType CurrentAttack;   // what ATTACK is the enemy currently performing?
}
public enum EnemyAction
{
    Idle    = 0,
    Move    = 1,
    Attack  = 2,
    Stagger = 3
}
public enum EnemyAttackType
{
    None            = 0,
    Ranged          = 1,
    FocusedRanged   = 2,
    Melee           = 3,
    Zone            = 4
}
public class Enemy : MonoBehaviour
{
    public static Enemy Instance { get; private set; }

    [Header("Enemy Components")]
    [SerializeField] private EnemyHurtbox hurtbox;
    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxDefense = 100f;

    private float _timeScale;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        hurtbox.Initialize(maxHealth);
        _timeScale = 1f;
    }

    void Update()
    {
        var deltaTime = Time.deltaTime * _timeScale;

        hurtbox.UpdateModel(deltaTime);
    }

    public void SetTimeScale(float timeScale) => _timeScale = timeScale;
}

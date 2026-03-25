using UnityEngine;
public class Enemy : MonoBehaviour
{
    public static Enemy Instance { get; private set; }

    [Header("Enemy Components")]
    [SerializeField] private EnemyHurtbox hurtbox;
    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;

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

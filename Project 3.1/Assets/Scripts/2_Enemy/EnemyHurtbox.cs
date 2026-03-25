using UnityEngine;
public class EnemyHurtbox : MonoBehaviour, IDamageable, IKnockbackable
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform enemyModel;

    [Header("Damage Feedback | Emission")]
    [SerializeField] private Color hitColor;
    [SerializeField] private float hitBrightness;
    [SerializeField] private float effectSpeed;
    private Color _defaultColor;
    private Material _material;

    [Header("Damage Feedback | Scale Pulse")]
    [SerializeField] private float pulseAmount = 0.15f;
    [SerializeField] private float shrinkSpeed = 25f;
    [SerializeField] private float growSpeed = 25f;
    private Vector3 _defaultScale;
    private Vector3 _scaleOffset;

    private float _maxHealth;
    private float _currentHealth;

    // Interface Variables
    public CharacterController CharacterController => controller;
    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;

    public void Initialize(float health)
    {
        // HP Stats
        _maxHealth = health;
        _currentHealth = _maxHealth;

        // Damage Feedback
        if (enemyModel.TryGetComponent(out MeshRenderer mesh))
        {
            _material = mesh.material;
        }
        _material.EnableKeyword("_EMISSION");
        _defaultColor = new(0, 0, 0);
        _defaultScale = transform.localScale;
    }

    public void UpdateModel()
    {
        // Emission Lerp
        Color current = _material.GetColor("_EmissionColor");
        Color next = Color.Lerp(current, _defaultColor, Time.deltaTime * effectSpeed);
        _material.SetColor("_EmissionColor", next);

        // Scale Lerp
        enemyModel.localScale = _defaultScale + _scaleOffset;
        _scaleOffset = Vector3.Lerp(_scaleOffset, Vector3.zero, Time.deltaTime * growSpeed);
    }

    // 'IDamagable' methods
    public void DecreaseHealth(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);

        // Trigger hit feedback
        _scaleOffset = Vector3.one * -pulseAmount;
        _material.SetColor("_EmissionColor", hitColor * hitBrightness);

        Debug.Log("Enemy Health: " + _currentHealth);
    }
    public void IncreaseHealth(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);

        Debug.Log("Enemy Health: " + _currentHealth);
    }
    public void Death()
    {
        throw new System.NotImplementedException();
    }    

    // 'IKnockbackable' methods
    public void Knockback(float amount, float duration)
    {
        throw new System.NotImplementedException();
    }
}

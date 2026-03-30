using UnityEngine;
public class PlayerHurtbox : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    private float _currentHealth; 
    private bool _isAlive; 

    public float MaxHealth => maxHealth;
    public float CurrentHealth => _currentHealth;
    public bool IsAlive => _isAlive;

    public void Initialize()
    {
        _currentHealth = maxHealth;
        _isAlive = _currentHealth > 0f;
    }

    public void DecreaseHealth(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, maxHealth);
        _isAlive = _currentHealth > 0f;
    }

    public void IncreaseHealth(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, maxHealth);
        _isAlive = _currentHealth > 0f;
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }
}

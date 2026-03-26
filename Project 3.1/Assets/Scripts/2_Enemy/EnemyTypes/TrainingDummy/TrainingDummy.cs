using UnityEngine;
public class TrainingDummy : Enemy
{
    [Space]
    [SerializeField] private TrainingDummyHitFeedback hitFeedback;
    [Space]
    [SerializeField] private EnemyAttack[] attacks;
    [SerializeField] private float attackCooldown;
    private float _cooldownTimer;

    // attack state control
    private EnemyAttack _currentAttack;
    private bool _attackSelected;

    protected override void Update()
    {
        var deltaTime = Time.deltaTime * _timeScale;
        hitFeedback.UpdateModel(deltaTime);

        if (_cooldownTimer >= attackCooldown)
        {
            _state = EnemyState.Attack;
            _cooldownTimer = 0f;
        }

        if (_state is EnemyState.Attack)
        {
            if (!_attackSelected)
            {
                _attackSelected = true;
                _currentAttack = attacks[Random.Range(0, attacks.Length - 1)];
            }

            if (_currentAttack)
            {
                _currentAttack.Attack();
            }
        }
        else
        {
            _cooldownTimer += deltaTime;
        }
    }

    public override void DecreaseHealth(float amount)
    {
        hitFeedback.TriggerHitFeedback();
        base.DecreaseHealth(amount);
    }
}

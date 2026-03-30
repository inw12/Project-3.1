using UnityEngine;
public class TrainingDummy : Enemy
{
    [SerializeField] private TrainingDummyHitFeedback hitFeedback;
    [SerializeField] private TrainingDummyAnimationController animationController;
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

        if (_enemyActive)
        {
            if (_cooldownTimer >= attackCooldown)
            {
                _state.CurrentAction = EnemyAction.Attack;
                _cooldownTimer = 0f;
            }

            if (_state.CurrentAction is EnemyAction.Attack)
            {
                if (!_attackSelected)
                {
                    _attackSelected = true;
                    if (attacks.Length > 0) _currentAttack = attacks[Random.Range(0, attacks.Length - 1)];
                }

                if (_currentAttack && _state.AttackActive)
                {
                    _currentAttack.Attack();
                }
            }
            else
            {
                _cooldownTimer += deltaTime;
            }
        }
    }

    void LateUpdate()
    {
        animationController.UpdateAnimation(ref _state);
    }

    public override void SetToIdle()
    {
        base.SetToIdle();

    }

    public override void DecreaseHealth(float amount)
    {
        hitFeedback.TriggerHitFeedback();
        base.DecreaseHealth(amount);
    }
}

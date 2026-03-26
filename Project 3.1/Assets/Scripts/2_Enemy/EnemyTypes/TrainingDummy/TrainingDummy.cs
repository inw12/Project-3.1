using UnityEngine;
public class TrainingDummy : Enemy
{
    [Space]
    [SerializeField] private TrainingDummyHitFeedback hitFeedback;

    protected override void Update()
    {
        var deltaTime = Time.deltaTime * _timeScale;
        hitFeedback.UpdateModel(deltaTime);
    }

    public override void DecreaseHealth(float amount)
    {
        hitFeedback.TriggerHitFeedback();
        base.DecreaseHealth(amount);
    }
}

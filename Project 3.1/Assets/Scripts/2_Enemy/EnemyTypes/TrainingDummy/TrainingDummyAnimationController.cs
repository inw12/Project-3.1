using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TrainingDummyAnimationController : MonoBehaviour
{
    private Animator _animator;

    public void Initialize()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateAnimation(ref EnemyState state)
    {
        _animator.SetInteger("CurrentAction", (int)state.CurrentAction);
    }

    public void SetToIdle()
    {
        _animator.SetInteger("CurrentAction", 0);
        _animator.SetBool("AttackActive", false);
    }
}

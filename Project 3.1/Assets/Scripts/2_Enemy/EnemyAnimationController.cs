/// =============================================================
/// * ALL Enemies will have the following animator parameters:
///     - "CurrentAction"   (int)
///     - "AttackID"   (int)
///     - "AttackActive"    (bool)
///     - "InHitstun"       (bool)
/// =============================================================
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class EnemyAnimationController : MonoBehaviour
{
    private Animator _animator;

    public void Initialize()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateAnimator(EnemyState state)
    {
        _animator.SetInteger("CurrentAction", (int)state.CurrentAction);
        _animator.SetInteger("AttackID", state.CurrentAttack);
        _animator.SetBool("AttackActive", state.AttackActive);
        _animator.SetBool("InHitstun", state.InHitstun);
    }
}
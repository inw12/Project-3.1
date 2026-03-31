/// =============================================================
/// * ALL Enemies will have the following animator parameters:
///     - "CurrentAction"   (int)
///     - "CurrentAttack"   (int)
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
        _animator.SetInteger("CurrentAttack", state.CurrentAttack);
        _animator.SetBool("InHitstun", state.InHitstun);
    }
}
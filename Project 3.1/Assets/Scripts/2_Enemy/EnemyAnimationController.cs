/// =============================================================
/// * ALL Enemies will have the following animator parameters:
///     - "CurrentAction"   (int)
///     - "AttackID"        (int)
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

    public void UpdateAnimator(ref EnemyState state)
    {
        _animator.SetInteger("CurrentAction", (int)state.CurrentAction);
        _animator.SetInteger("AttackID", state.CurrentAttack);
        _animator.SetBool("InHitstun", state.InHitstun);
    }

    public bool GetBool(string s) => _animator.GetBool(s);
    public void SetBool(string s, bool b) => _animator.SetBool(s, b);
}
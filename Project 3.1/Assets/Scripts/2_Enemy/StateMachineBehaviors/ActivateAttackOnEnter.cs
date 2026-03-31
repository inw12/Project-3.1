/// * This StateMachineBehavior is reserved for Enemy objects to control attack animations + logic
using UnityEngine;
public class ActivateAttackOnEnter : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("AttackActive", true);
    }
}

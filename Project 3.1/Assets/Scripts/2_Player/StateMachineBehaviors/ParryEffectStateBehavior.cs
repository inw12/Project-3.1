using UnityEngine;
public class ParryEffectStateBehavior : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerCombat.Instance.ParryActive(false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("ParryActive", false);
        PlayerCombat.Instance.ExitCombatState();
    }
}

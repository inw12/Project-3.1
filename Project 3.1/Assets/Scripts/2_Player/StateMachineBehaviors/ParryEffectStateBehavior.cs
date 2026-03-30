using UnityEngine;
public class ParryEffectStateBehavior : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerCombat.Instance.ExitCombatState();
        animator.SetBool("ParryActive", false);
    }
}

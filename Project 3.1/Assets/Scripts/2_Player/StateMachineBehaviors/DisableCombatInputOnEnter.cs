using UnityEngine;
public class DisableCombatInputOnEnter : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerCombat.Instance.DisableCombatInput();
    }
}

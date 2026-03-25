using UnityEngine;
public class EnableCombatInputOnExit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerCombat.Instance.EnableCombatInput();
    }
}

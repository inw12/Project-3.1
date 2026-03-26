using UnityEngine;
public class DisableMovementInputOnEnter : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement.Instance.DisableMovementInput();
    }
}

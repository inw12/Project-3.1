using UnityEngine;
public class EnableMovementInputOnExit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement.Instance.EnableMovementInput();
    }
}

using UnityEngine;
public class ParryStateBehavior : StateMachineBehaviour
{
    private int _currentFrame;
    private int _activeParryFrames;
    private float _frameRate;
    private int _totalFrames;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset frame counter
        _currentFrame = 0;

        // Get active parry frames
        _activeParryFrames = PlayerCombat.Instance.GetParryFrames();

        // Get total frames
        float frameRate = animator.GetCurrentAnimatorClipInfo(layerIndex)[0].clip.frameRate;
        _totalFrames = Mathf.RoundToInt(stateInfo.length * frameRate);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _currentFrame = Mathf.FloorToInt(stateInfo.normalizedTime % 1f * _totalFrames);
        if (_currentFrame >= _activeParryFrames)
        {
            animator.SetBool("ParryActive", false);
            animator.Play("Parry - END");
            PlayerCombat.Instance.ExitCombatState();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _currentFrame = 0;
        animator.SetBool("ParryActive", false);
        PlayerCombat.Instance.ExitCombatState();
    }
}

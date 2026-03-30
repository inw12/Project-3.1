using UnityEngine;
public class PlayerParrybox : MonoBehaviour, IParry
{
    private PlayerAnimationController _animationController;

    public void Initialize(PlayerAnimationController controller)
    {
        _animationController = controller;
    }

    public void Parry()
    {
        _animationController.SetParryEffectTrigger();
    }
}

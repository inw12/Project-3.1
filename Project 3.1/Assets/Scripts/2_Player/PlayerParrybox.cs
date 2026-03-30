using UnityEngine;
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerParrybox : MonoBehaviour, IParry
{
    [SerializeField] private int activeParryFrames = 5;
    [Space]
    [SerializeField] private PlayerAnimationController animationController;
    [SerializeField] private CapsuleCollider hurtbox;
    private CapsuleCollider _parrybox;


    public void Initialize()
    {
        // Capsule Collider Initialization
        _parrybox = GetComponent<CapsuleCollider>();
        _parrybox.enabled = false;
    }

    public void SetParryActive(bool b)
    {
        _parrybox.enabled = b;
        hurtbox.enabled = !b;
        animationController.ParryActive(b);
    }

    public void Parry()
    {
        animationController.ParryTrigger();
    }

    public int GetParryFrames() => activeParryFrames;
}

using UnityEngine;
public interface IKnockbackable
{
    CharacterController CharacterController { get; }
    void Knockback(float amount, float duration);
}
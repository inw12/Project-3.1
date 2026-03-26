using UnityEngine;
public interface IKnockbackable
{
    void Knockback(Vector3 direction, float amount, float duration);
}
/// * Objects that:
///     - Scan for collisions against 'IDamageable' objects
///     - Trigger 'IDamageable' methods to affect object's HP value
using UnityEngine;
public interface IHitbox
{
    void OnHit(Collider other);
}
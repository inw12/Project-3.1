/// * Objects that:
///     - Collide with other objects to deal damage
using UnityEngine;
public interface IHitbox
{
    void OnHit(Collider other);
}
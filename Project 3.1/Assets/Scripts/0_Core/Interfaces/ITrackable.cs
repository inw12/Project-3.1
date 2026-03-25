/// * Objects whose position can be tracked
using UnityEngine;
public interface ITrackable
{
    Vector3 Position { get; }
}
using UnityEngine;
public class TrackingCamera : CameraState
{
    [SerializeField] private Vector3 cameraPosition;
    [SerializeField] private Vector3 cameraRotation;
    [SerializeField] private float zoomMin;
    [SerializeField] private float zoomMax;

    public override Vector3 GetTargetPosition() 
    {
        var targetPosition = cameraPosition;

        // 1. Get positions of player and enemy
        // 2. Find point between the two
        // 3. Fixate camera position there
        // 4. Adjust FOV accordingly        
        
        return targetPosition;
    }
    public override Quaternion GetTargetRotation() => Quaternion.Euler(cameraRotation);
}
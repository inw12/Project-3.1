using UnityEngine;
public class TrackingCamera : CameraState
{
    [SerializeField] private Vector3 cameraPosition;
    [SerializeField] private Vector3 cameraRotation;
    [SerializeField] private float zoomMin;
    [SerializeField] private float zoomMax;

    public override Vector3 GetTargetPosition() 
    {
        // 1. Get positions of player and enemy
        // 2. Find point between the two
        // 3. Fixate camera position there
        // 4. Adjust FOV accordingly     

        Vector3 player = Player.Instance.transform.position;
        Vector3 enemy = Enemy.Instance.transform.position;

        Vector3 midpoint = (player + enemy) / 2f;
        return midpoint + cameraPosition;
    }
    public override Quaternion GetTargetRotation() => Quaternion.Euler(cameraRotation);
}
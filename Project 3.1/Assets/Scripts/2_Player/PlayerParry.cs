using UnityEngine;
public class PlayerParry : MonoBehaviour
{
    [SerializeField] private float parryDuration = 0.2f;
    private float _parryTimer;

    public void Initialize()
    {
        
    }

    public void UpdateParry(ref CombatState state, ref bool parryStarted)
    {
        if (parryStarted)
        {
            _parryTimer += Time.deltaTime;
            if (_parryTimer >= parryDuration)
            {
                parryStarted = false;
                state.CurrentAction = CombatAction.None;
                _parryTimer = 0f;
            }
        }
    }
}

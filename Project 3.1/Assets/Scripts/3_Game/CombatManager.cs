using UnityEngine;
public class CombatManager : MonoBehaviour
{
    [Header("Main Enemy")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform enemySpawn;
    private Enemy _enemy;

    [Header("Cutscene Components")]
    [SerializeField] private CutsceneSequencer cutsceneSequencer;
    private bool _phaseChangeTriggered;

    void Start()
    {
        _enemy = enemy.GetComponent<Enemy>();
    }

    void EnterParryPhase()
    {
        CameraManager.Instance.SwitchTo<BehindPlayerCamera>();
    }

    void ExitParryPhase()
    {
        CameraManager.Instance.SwitchTo<DefaultCamera>();
    }
}

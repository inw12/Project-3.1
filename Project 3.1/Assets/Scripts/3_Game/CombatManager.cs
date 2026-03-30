/// * Listens for phase change signal sent by enemy after DEF reaches a certain amount
/// * Upon phase change:
///     - Disable Enemy AI
///     - Trigger Enemy Animation
///     - Trigger cutscene via 'CutsceneSequencer'
using UnityEngine;
using UnityEngine.Timeline;
public class CombatManager : MonoBehaviour
{
    [Header("Main Enemy")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform enemySpawn;
    private Enemy _enemy;

    [Header("Combat Variables")]
    [SerializeField] [Range(0f, 1f)] private float phaseChangeThreshold = 0.25f;    // % of DEF the enemy has before changing combat states

    [Header("Cutscene Components")]
    [SerializeField] private CutsceneSequencer cutsceneSequencer;
    [SerializeField] private TimelineAsset cutscene;
    private bool _phaseChangeTriggered;

    void Start()
    {
        _enemy = enemy.GetComponent<Enemy>();
    }

    void Update()
    {
        var enemyDefense = _enemy.CurrentDefense / _enemy.MaxDefense;
        if (enemyDefense < phaseChangeThreshold && !_phaseChangeTriggered)
        {
            _phaseChangeTriggered = true;
            EnterParryPhase();
            cutsceneSequencer.PlayFinisher(enemy, cutscene);
        }
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

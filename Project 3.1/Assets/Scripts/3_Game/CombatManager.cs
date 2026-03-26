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
            cutsceneSequencer.PlayFinisher(enemy, cutscene);
            EnterParryPhase();
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

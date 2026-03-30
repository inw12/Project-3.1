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

    [Header("Cutscene Components")]
    [SerializeField] private CutsceneSequencer cutsceneSequencer;
    [SerializeField] private TimelineAsset cutscene;

    void Start()
    {
        _enemy = enemy.GetComponent<Enemy>();
    }

    public void EnterParryPhase()
    {
        CameraManager.Instance.SwitchTo<BehindPlayerCamera>();
        cutsceneSequencer.PlayFinisher(enemy, cutscene);
    }

    void ExitParryPhase()
    {
        CameraManager.Instance.SwitchTo<DefaultCamera>();
    }
}

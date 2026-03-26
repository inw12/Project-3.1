/// * This script prepares scene objects for cutscenes.
/// 
/// * ParryPhase cutscene: position player and enemy in fixed locations
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
[RequireComponent(typeof(PlayableDirector))]
public class CutsceneSequencer : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform enemyPos;

    private PlayableDirector _playableDirector;

    void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    public void PlayFinisher(GameObject enemy, TimelineAsset asset)
    {
        Player.Instance.DisablePlayerInput();
        if (enemy.TryGetComponent(out Enemy e))
        {
            e.DeactivateEnemyAI();
        }

        // Snap both characters into position first
        PositionCharacters(enemy);

        // Then bind and play
        BindTimeline(asset, enemy);
        _playableDirector.Play();
    }

    private void PositionCharacters(GameObject enemy)
    {
        // Place the Singleton player at their spawn point
        Player.Instance.transform.SetPositionAndRotation(
            playerPos.position,
            playerPos.rotation
        );

        // Place the enemy at theirs
        enemy.transform.SetPositionAndRotation(
            enemyPos.position,
            enemyPos.rotation
        );
    }

    private void BindTimeline(TimelineAsset asset, GameObject enemy)
    {
        _playableDirector.playableAsset = asset;

        foreach (var track in asset.GetOutputTracks())
        {
            switch (track.name)
            {
                case "PlayerAnimationTrack":
                    _playableDirector.SetGenericBinding(track,
                        Player.Instance.GetComponent<Animator>());
                    break;

                case "EnemyAnimationTrack":
                    _playableDirector.SetGenericBinding(track,
                        enemy.GetComponent<Animator>());
                    break;
            }
        }

        _playableDirector.RebindPlayableGraphOutputs();
    }
}

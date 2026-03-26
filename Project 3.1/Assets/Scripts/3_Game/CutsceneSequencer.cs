using UnityEngine;
using UnityEngine.Playables;
[RequireComponent(typeof(PlayableDirector))]
public class CutsceneSequencer : MonoBehaviour
{
    private PlayableDirector _playableDirector;

    void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }
}

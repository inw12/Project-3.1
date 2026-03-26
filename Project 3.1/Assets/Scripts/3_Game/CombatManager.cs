using UnityEngine;
public class CombatManager : MonoBehaviour
{
    [Header("Main Enemy")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform enemySpawn;
    private Enemy _enemy;

    [Header("Combat Variables")]
    [SerializeField] [Range(0f, 1f)] private float phaseChangeThreshold = 0.25f;    // % of DEF the enemy has before changing combat states

    void Awake()
    {
        //_enemy = Instantiate(enemy, enemySpawn).GetComponent<Enemy>();
    }
}

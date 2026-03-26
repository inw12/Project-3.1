using UnityEngine;
public class CombatManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform enemySpawn;
    private Enemy _enemy;

    void Awake()
    {
        _enemy = Instantiate(enemy, enemySpawn).GetComponent<Enemy>();
    }
}

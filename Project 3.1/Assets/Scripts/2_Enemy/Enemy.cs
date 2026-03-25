using UnityEngine;
public class Enemy : MonoBehaviour
{
    [Header("Enemy Components")]
    [SerializeField] private EnemyHurtbox hurtbox;
    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;

    void Start()
    {
        hurtbox.Initialize(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        hurtbox.UpdateModel();
    }
}

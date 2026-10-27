using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    private Enemy_Melee enemy;
    [SerializeField] private int durability;
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy_Melee>();
    }

    public void ReduceDuarbility()
    {
        durability--;

        if (durability <= 0)  
        {
            enemy.anim.SetFloat("ChaseIndex", 0); // enable default chase animation
            Destroy(gameObject);
        }
    }
}

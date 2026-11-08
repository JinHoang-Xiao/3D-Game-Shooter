using UnityEngine;

public class Enemy_Hitbox : HitBox
{
    private Enemy enemy;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponentInParent<Enemy>();
    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        enemy.GetHit();
    }
}

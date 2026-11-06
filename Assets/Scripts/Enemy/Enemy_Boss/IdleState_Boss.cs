using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Boss : EnemyState
{
    private Enemy_Boss enemy;

    public IdleState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Update()
    {
        base.Update();
    }
}

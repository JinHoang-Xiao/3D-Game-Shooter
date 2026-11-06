using UnityEngine;

public class MoveState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    private Vector3 destination;

    private float actionTimer;
    private float timeBeforeSpeedUp = 15;

    private bool speedUpActivated;
    public MoveState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
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

using UnityEngine;

public class ChaseState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private float lastTimeUpdatedDestination;

    public ChaseState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        //CheckChaseAnimation();

        base.Enter();

        enemy.agent.speed = enemy.runSpeed;
        enemy.agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerInAttackRange())
            stateMachine.ChangeState(enemy.attackState);

        enemy.FaceTarget(GetNextPathPoint());

        if(CanUpdateDestination())
        {
            enemy.agent.destination = enemy.player.transform.position;
        }
    }

    private bool CanUpdateDestination()
    {
        if (Time.time > lastTimeUpdatedDestination + .25f)
        {
            lastTimeUpdatedDestination = Time.time;
            return true;
        }

        return false;
    }

    /*private void CheckChaseAnimation()
    {
        if(enemy.melee_Type == EnemyMelee_Type.Shield && enemy.shieldTransform == null)
        {
            enemy.anim.SetFloat("ChaseIndex", 0);
        }
    }*/
}

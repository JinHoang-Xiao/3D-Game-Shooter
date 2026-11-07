using UnityEngine;

public enum BossWeaponType { Flamethrower, Hummer }

public class Enemy_Boss : Enemy
{
    public float attackRange;
  
    //[SerializeField] private LayerMask whatToIngore;
    public IdleState_Boss idleState { get; private set; }
    public MoveState_Boss moveState { get; private set; }
    public AttackState_Boss attackState { get; private set; }
    //public Enemy_BossVisuals bossVisuals { get; private set; }
    protected override void Awake()
    {
        base.Awake();

        //bossVisuals = GetComponent<Enemy_BossVisuals>();

        idleState = new IdleState_Boss(this, stateMachine, "Idle");
        moveState = new MoveState_Boss(this, stateMachine, "Move");
        attackState = new AttackState_Boss(this, stateMachine, "Attack");
        
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        /* DEBUG CHO RANDOM ANIMATION
        for(int i =0; i<1000; i++)
        {
            Debug.Log(Random.Range(0, 1));
        }*/
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if (ShouldEnterBattleMode())
            EnterBattleMode();
    }
    public override void EnterBattleMode()
    {
        base.EnterBattleMode();
        stateMachine.ChangeState(moveState);
    }

    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackRange;

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position,attackRange);
    }
}

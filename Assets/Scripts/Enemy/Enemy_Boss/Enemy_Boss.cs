using UnityEngine;

public enum BossWeaponType { Flamethrower, Hummer }

public class Enemy_Boss : Enemy
{
  
    //[SerializeField] private LayerMask whatToIngore;
    public IdleState_Boss idleState { get; private set; }
    public MoveState_Boss moveState { get; private set; }

    //public Enemy_BossVisuals bossVisuals { get; private set; }
    protected override void Awake()
    {
        base.Awake();

        //bossVisuals = GetComponent<Enemy_BossVisuals>();

        idleState = new IdleState_Boss(this, stateMachine, "Idle");
        moveState = new MoveState_Boss(this, stateMachine, "Move");
        
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        //if (ShouldEnterBattleMode())
        //    EnterBattleMode();
    }
}

using UnityEngine;

public enum BossWeaponType { Flamethrower, Hummer }

public class Enemy_Boss : Enemy
{
    [Header("Boss details")]
    public float actionCooldown = 10;
    public float attackRange;

    [Header("Ability")]
    public ParticleSystem flamethrower;
    public float abilityCooldown;
    private float lastTimeUsedAbility;
    public float flameThrowDuration;
    public bool flamethrowActive { get; private set; }


    [Header("Jump attack")]
    public float jumpAttackCooldown = 10;
    private float lastTimeJumped;
    public float travelTimeToTarget = 1;
    public float minJumpDistanceRequired;

    [Space]
    [SerializeField] private LayerMask whatToIngore;

    public IdleState_Boss idleState { get; private set; }
    public MoveState_Boss moveState { get; private set; }
    public AttackState_Boss attackState { get; private set; }
    public JumpAttackState_Boss jumpAttackState { get; private set; }
    public AbilityState_Boss abilityState { get; private set; }
    //public Enemy_BossVisuals bossVisuals { get; private set; }
    protected override void Awake()
    {
        base.Awake();

        //bossVisuals = GetComponent<Enemy_BossVisuals>();

        idleState = new IdleState_Boss(this, stateMachine, "Idle");
        moveState = new MoveState_Boss(this, stateMachine, "Move");
        attackState = new AttackState_Boss(this, stateMachine, "Attack");
        jumpAttackState = new JumpAttackState_Boss(this, stateMachine, "JumpAttack");
        abilityState = new AbilityState_Boss(this, stateMachine, "Ability");
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

    public void ActivateFlameThrower(bool activate)
    {
        flamethrowActive = activate;

        if (!activate)
        {
            flamethrower.Stop();
            anim.SetTrigger("StopFlamethrower");
            Debug.Log("Flame Stopped");
            return;
        }

        var mainModule = flamethrower.main;
        var extraModule = flamethrower.transform.GetChild(0).GetComponent<ParticleSystem>().main;

        mainModule.duration = flameThrowDuration;
        extraModule.duration = flameThrowDuration;

        flamethrower.Clear();
        flamethrower.Play();
    }


    public bool CanDoAbility()
    {
        if (Time.time > lastTimeUsedAbility + abilityCooldown)
        {
            return true;
        }

        return false;
    }

    public void SetAbilityOnCooldown() => lastTimeUsedAbility = Time.time;

    public bool CanDoJumpAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < minJumpDistanceRequired)
        {
            return false;
        }

        if (Time.time > lastTimeJumped + jumpAttackCooldown && IsPlayerInClearSight())
        {
            return true;
        }
        return false;
    }

    public void SetJumpAttackOnCooldown() => lastTimeJumped = Time.time;

    public bool IsPlayerInClearSight()
    {
        Vector3 myPos = transform.position + new Vector3(0, 1.5f, 0);
        Vector3 playerPos = player.position + Vector3.up;
        Vector3 directionToPlayer = (playerPos - myPos).normalized;

        if (Physics.Raycast(myPos, directionToPlayer, out RaycastHit hit, 100, ~whatToIngore))
        {
            if (hit.transform == player || hit.transform.parent == player)
            {
                return true;
            }
        }
        return false;
    }

    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackRange;

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (player != null)
        {
            Vector3 myPos = transform.position + new Vector3(0, 1.5f, 0);
            Vector3 playerPos = player.position + Vector3.up;

            Gizmos.color = Color.red;

            Gizmos.DrawLine(myPos, playerPos);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minJumpDistanceRequired);
    }
}

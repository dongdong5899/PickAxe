using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using UnityEngine;

public class ShootEnemyIdleState : EntityState
{
    private Enemy _enemy;

    private EntityMovement _movement;
    private StatCompo _statCompo;

    public ShootEnemyIdleState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
    {
        _enemy = entity as Enemy;
    }

    public override void Enter()
    {
        base.Enter();
        _movement = _enemy.GetCompo<EntityMovement>();
        _statCompo = _enemy.GetCompo<StatCompo>();
    }

    public override void Update()
    {
        base.Update();

        if (_enemy.AttackRangeInPlayer())
        {
            if (_enemy.lastAttackTime + _statCompo.GetValue(EStatType.AttackCooltime) < Time.time)
                _enemy.ChangeState(_enemy.enemyFSM[FSMState.Attack]);
            else
            {
                _renderer.FlipController(Mathf.Sign(_enemy.PlayerDirection().x));
                _movement.StopImmediately();
            }
        }
        else
        {
            _enemy.ChangeState(_enemy.enemyFSM[FSMState.Chase]);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyChaseState : EntityState
{
    private Enemy _enemy;

    private EntityMovement _movement;
    private StatCompo _statCompo;

    public EnemyChaseState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
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
                if (_enemy.enemyFSM[FSMState.Idle] == null)
                {
                    _renderer.FlipController(Mathf.Sign(_enemy.PlayerDirection().x));
                    _movement.StopImmediately();
                }
                else
                {
                    _enemy.ChangeState(_enemy.enemyFSM[FSMState.Idle]);
                }
            }
        }
        else
        {
            Vector2 movement = _enemy.PlayerDirection();
            _movement.SetMovement(movement);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

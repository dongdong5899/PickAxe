using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using System;
using UnityEngine;

public class NearEnemyAttackState : EntityState
{
    private NearEnemy _nearEnemy;

    private EntityMovement _movement;
    private StatCompo _statCompo;

    public NearEnemyAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
    {
        _nearEnemy = entity as NearEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.Instance.PlaySound(SoundEnum.NearAttack,_nearEnemy.transform);
        _movement = _nearEnemy.GetCompo<EntityMovement>();
        _statCompo = _nearEnemy.GetCompo<StatCompo>();

        _nearEnemy.lastAttackTime = Time.time;

        _movement.StopImmediately();


        _nearEnemy.GetCompo<EntityRenderer>().OnAnimationTriggerEvent += HandleAnimationTriggerEvent;
    }

    private void HandleAnimationTriggerEvent(TriggertType type)
    {
        if (type == TriggertType.End)
            _nearEnemy.ChangeState(_nearEnemy.enemyFSM[FSMState.Chase]);
        else if (type == TriggertType.Attack)
        {
            _nearEnemy.Attack();
        }
    }

    public override void Update()
    {
        base.Update();


    }

    public override void Exit()
    {
        base.Exit();
        _nearEnemy.GetCompo<EntityRenderer>().OnAnimationTriggerEvent -= HandleAnimationTriggerEvent;
    }
}

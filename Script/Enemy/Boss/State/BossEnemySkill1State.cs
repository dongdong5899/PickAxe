using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using System;
using UnityEngine;

public class BossEnemySkill1State : EntityState
{
    private Boss _boss;

    public BossEnemySkill1State(Entity entity, AnimParamSO animParam) : base(entity, animParam)
    {
        _boss = entity as Boss;
    }

    public override void Enter()
    {
        base.Enter();

        _boss.Skill1();
        _boss.OnSkill1EndEvent += HandleSkill1EndEvent;
    }

    private void HandleSkill1EndEvent()
    {
        _boss.ChangeState(_boss.enemyFSM[FSMState.Idle]);
    }
}

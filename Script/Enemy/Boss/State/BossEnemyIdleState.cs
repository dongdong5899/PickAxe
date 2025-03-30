using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using UnityEngine;

public class BossEnemyIdleState : EntityState
{
    private float _idleStartTime;

    private Boss _boss;

    public BossEnemyIdleState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
    {
        _boss = entity as Boss;
    }

    public override void Enter()
    {
        base.Enter();

        _idleStartTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_idleStartTime + _entity.GetCompo<StatCompo>().GetValue(EStatType.AttackCooltime) < Time.time)
        {
            int skillIdx = Random.Range(0, 2);
            switch(skillIdx)
            {
                case 0:
                    _boss.ChangeState(_boss.enemyFSM[FSMState.Skill1]);
                    break;
                case 1:
                    _boss.ChangeState(_boss.enemyFSM[FSMState.Skill2]);
                    break;
                case 2:
                    _boss.ChangeState(_boss.enemyFSM[FSMState.Skill3]);
                    break;
                default:
                    break;
            }
        }
    }
}

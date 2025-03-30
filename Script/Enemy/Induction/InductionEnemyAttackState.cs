using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using UnityEngine;

public class InductionEnemyAttackState : EntityState
{
    private InductionEnemy _inductionEnemy;

    private EntityMovement _movement;
    private StatCompo _statCompo;

    public InductionEnemyAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
    {
        _inductionEnemy = entity as InductionEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.Instance.PlaySound(SoundEnum.InductionAttack,_inductionEnemy.transform);
        _movement = _inductionEnemy.GetCompo<EntityMovement>();
        _statCompo = _inductionEnemy.GetCompo<StatCompo>();

        _inductionEnemy.lastAttackTime = Time.time;

        _movement.StopImmediately();

        _inductionEnemy.GetCompo<EntityRenderer>().OnAnimationTriggerEvent += HandleAnimationTriggerEvent;
    }

    private void HandleAnimationTriggerEvent(TriggertType type)
    {
        if (type == TriggertType.End)
            _inductionEnemy.ChangeState(_inductionEnemy.enemyFSM[FSMState.Chase]);
        else if (type == TriggertType.Attack)
        {
            for (int i = 0; i < 2; i++)
            {
                Induction induction = GameObject.Instantiate(_inductionEnemy.induction, _inductionEnemy.transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));

                int damage = Mathf.RoundToInt(_statCompo.GetValue(EStatType.Damage));
                induction.Init(_inductionEnemy, GameManager.Instance.Player, _inductionEnemy.whatIsTarget, _statCompo.GetValue(EStatType.ProjectileSpeed), damage);
            }
        }
    }

    public override void Update()
    {
        base.Update();


    }

    public override void Exit()
    {
        base.Exit();
        _inductionEnemy.GetCompo<EntityRenderer>().OnAnimationTriggerEvent -= HandleAnimationTriggerEvent;
    }
}

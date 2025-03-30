using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using System;
using UnityEngine;

public class ShootEnemyAttackState : EntityState
{
    private ShootEnemy _shootEnemy;

    private EntityMovement _movement;
    private StatCompo _statCompo;

    public ShootEnemyAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
    {
        _shootEnemy = entity as ShootEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        _movement = _shootEnemy.GetCompo<EntityMovement>();
        _statCompo = _shootEnemy.GetCompo<StatCompo>();

        _shootEnemy.lastAttackTime = Time.time;

        _movement.StopImmediately();

        _shootEnemy.GetCompo<EntityRenderer>().OnAnimationTriggerEvent += HandleAnimationTriggerEvent;
        AudioManager.Instance.PlaySound(SoundEnum.ShootAttack, _shootEnemy.transform);
    }

    private void HandleAnimationTriggerEvent(TriggertType type)
    {
        if (type == TriggertType.End)
            _shootEnemy.ChangeState(_shootEnemy.enemyFSM[FSMState.Chase]);
        if (type == TriggertType.Attack)
        {
            Quaternion dir = Quaternion.LookRotation(Vector3.back, GameManager.Instance.Player.transform.position - _shootEnemy.transform.position) * Quaternion.Euler(0, 0, 90);
            Projectile projectile = GameObject.Instantiate(_shootEnemy.projectile, _shootEnemy.transform.position, dir);

            int damage = Mathf.RoundToInt(_statCompo.GetValue(EStatType.Damage));
            projectile.Init(_shootEnemy, _shootEnemy.whatIsTarget, _statCompo.GetValue(EStatType.ProjectileSpeed), damage);
        }
    }

    public override void Update()
    {
        base.Update();


    }

    public override void Exit()
    {
        base.Exit();
        _shootEnemy.GetCompo<EntityRenderer>().OnAnimationTriggerEvent -= HandleAnimationTriggerEvent;
    }
}

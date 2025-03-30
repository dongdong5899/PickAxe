using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using System;
using UnityEngine;

namespace RPG.Players
{
    public class PlayerMoveState : EntityState
    {
        private Player _player;
        private EntityMovement _mover;
        private StatCompo _stat;
        private float _delayAttack = 0.2f;
        private bool _isThrowing;
        public PlayerMoveState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = _player.GetCompo<EntityMovement>();
        }

        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.AttackEvent += HandleAttackEvent;
            _player.PlayerInput.ParryingEvent += HandleParryingEvent;
            _stat = _player.GetCompo<StatCompo>();
            _delayAttack = _stat.GetValue(EStatType.ThrowAttackDelay);
            AudioManager.Instance.PlaySound(SoundEnum.PlayerWalk, _player.transform);
        }

        private void HandleParryingEvent()
        {
            _player.ChangeState(_player.playerFSM[FSMState.Parrying]);
        }

        public override void Exit()
        {
            _player.PlayerInput.AttackEvent -= HandleAttackEvent;
            _player.PlayerInput.ParryingEvent -= HandleParryingEvent;
            AudioManager.Instance.StopSound(SoundEnum.PlayerWalk, _player.transform);
            base.Exit();
        }

        private void HandleAttackEvent(bool isPerformed)
        {
            if (_player.hasPickax)
            {
                if (!_isThrowing && !isPerformed)
                {
                    _player.ChangeState(_player.playerFSM[FSMState.Attack]);
                }
                if(_isThrowing && isPerformed)
                {
                    _player.ChangeState(_player.playerFSM[FSMState.ThrowStart]);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            Vector2 input = _player.PlayerInput.InputDirection.normalized;
            
            _mover.SetMovement(input);

            if (input.magnitude < 0.05f)
            {
                _player.ChangeState(_player.playerFSM[FSMState.Idle]);
            }

            if (_delayAttack > 0)
            {
                _delayAttack -= Time.deltaTime;
                _isThrowing = false;
            }
            if (_delayAttack <= 0 )
            {
                _isThrowing = true;
            }
        }
    }
}

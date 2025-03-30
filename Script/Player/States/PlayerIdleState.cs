using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Players
{
    public class PlayerIdleState : EntityState
    {
        private Player _player;
        private EntityMovement _mover;
        private StatCompo _stat;
        private float _delayAttack;
        private bool _isThrowing;
        public PlayerIdleState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = _player.GetCompo<EntityMovement>();
            _stat = _player.GetCompo<StatCompo>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately();
            _player.PlayerInput.AttackEvent += HandleAttackEvent;
            _delayAttack = _stat.GetValue(EStatType.ThrowAttackDelay);
        }

        

        public override void Exit()
        {
            base.Exit();
            _player.PlayerInput.AttackEvent -= HandleAttackEvent;
        }

        private void HandleAttackEvent(bool isPerformed)
        {
            if (_player.hasPickax)
            {
                if (!_isThrowing && !isPerformed)
                {
                    _player.ChangeState(_player.playerFSM[FSMState.Attack]);
                }
                if (_isThrowing&&isPerformed)
                {
                    _player.ChangeState(_player.playerFSM[FSMState.ThrowStart]);
                }
            }
        }

        public override void Update()
        {
            base.Update();
            float input = _player.PlayerInput.InputDirection.magnitude;
            if (Mathf.Abs(input) > 0.05f)
            {
                _player.ChangeState(_player.playerFSM[FSMState.Move]);
            }

            if (_delayAttack > 0)
            {
                _delayAttack -= Time.deltaTime;
                _isThrowing = false;
            }
            if (_delayAttack <= 0)
            {
                _isThrowing = true;
            }
        }
    }
}

using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using System;
using UnityEngine;

namespace RPG.Players
{
    public class PlayerThrowStartState : EntityState
    {
        private Player _player;
        private EntityMovement _mover;
        public PlayerThrowStartState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = _player.GetCompo<EntityMovement>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.SpeedMultiplier = 0.7f;
            _player.GetCompo<EntityRenderer>().OnAnimationTriggerEvent += HandleAnimationTriggerEvent;
            _player.PlayerInput.AttackEvent += HandleAttackEvent;
        }

        private void HandleAttackEvent(bool isClick)
        {
            if (!isClick)
            {
                _player.ChangeState(_player.playerFSM[FSMState.Attack]);
                _mover.SpeedMultiplier = 1f;
            }
        }

        private void HandleAnimationTriggerEvent(TriggertType type)
        {
            if (type == TriggertType.End)
                _player.ChangeState(_player.playerFSM[FSMState.ThrowMaintain]);
        }

        public override void Update()
        {
            base.Update();
            Vector2 input = _player.PlayerInput.InputDirection.normalized;

            _mover.SetMovement(input);
        }

        public override void Exit()
        {
            _player.PlayerInput.AttackEvent -= HandleAttackEvent;
            _player.GetCompo<EntityRenderer>().OnAnimationTriggerEvent -= HandleAnimationTriggerEvent;
            base.Exit();
        }
    }
}


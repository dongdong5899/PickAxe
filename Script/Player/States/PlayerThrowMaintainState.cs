using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using System;
using UnityEngine;
namespace RPG.Players
{
    public class PlayerThrowMaintainState : EntityState
    {
        private Player _player;
        private EntityMovement _mover;

        private float _chargingtime;

        public PlayerThrowMaintainState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = _player.GetCompo<EntityMovement>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.SpeedMultiplier = 0.4f;
            _player.PlayerInput.AttackEvent += HandleAttackEvent;
        }

        private void HandleAttackEvent(bool isClicked)
        {
            if (!isClicked)
            {
                _player.ChangeState(_player.playerFSM[FSMState.Throw]);
            }
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
            base.Exit();
        }
    }
}


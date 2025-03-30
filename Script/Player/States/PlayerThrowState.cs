using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using System;
using UnityEngine;

namespace RPG.Players
{
    public class PlayerThrowState : EntityState
    {
        private Player _player;
        private EntityMovement _mover;
        public PlayerThrowState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = _player.GetCompo<EntityMovement>();
        }

        public override void Enter()
        {
            base.Enter();
            _player.GetCompo<EntityRenderer>().OnAnimationTriggerEvent += HandleAnimationTriggerEvent;
        }

        private void HandleAnimationTriggerEvent(TriggertType type)
        {
            if(type == TriggertType.Attack)
            {
                Debug.Log("Attack");
                Pickax pickax =  GameObject.Instantiate(_player.pickAxPrafab, _player.transform.position,Quaternion.Euler(new Vector3(0,0,-45)));
                pickax.Init(_player,_player.EnemyLayerMask,_player.GetCompo<StatCompo>().GetValue(EStatType.ThrowAttackSpeed),Mathf.RoundToInt(_player.GetCompo<StatCompo>().GetValue(EStatType.Damage)*1.5f));
                _player.pickAx = pickax;
                _player.HandWeaponActive(false);
            }
            if (type == TriggertType.End)
                _player.ChangeState(_player.playerFSM[FSMState.Idle]);
        }

        public override void Exit()
        {
            _mover.SpeedMultiplier = 1f;
            _player.GetCompo<EntityRenderer>().OnAnimationTriggerEvent -= HandleAnimationTriggerEvent;
            base.Exit();
        }
    }
}

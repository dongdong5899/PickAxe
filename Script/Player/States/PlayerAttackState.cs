using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace RPG.Players
{
    public class PlayerAttackState :EntityState
    {
        private Player _player;
        private float _delayStop;
        private EntityMovement _mover;
        private bool _isAtkStart = false;

        

        public PlayerAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = _player.GetCompo<EntityMovement>();
        }

        public override void Enter()
        {

            base.Enter();


            _delayStop = _player.GetCompo<StatCompo>().GetValue(EStatType.AttackCooltime);
            _isAtkStart = true;

            float atkDirection = _renderer.FacingDirection;
            float xInput = _player.PlayerInput.InputDirection.x;
            AudioManager.Instance.PlaySound(SoundEnum.PlayerAttack,_player.transform);


            _player.GetCompo<EntityRenderer>().OnAnimationTriggerEvent += HandleAnimationTriggerEvent;
        }

        private void HandleAnimationTriggerEvent(TriggertType type)
        {
            if (type == TriggertType.End)
                _player.ChangeState(_player.playerFSM[FSMState.Idle]);
            else if (type == TriggertType.Attack)
            {
                float facing = _player.GetCompo<EntityRenderer>().FacingDirection;
                ParticleSystem obj = GameObject.Instantiate(_player.sliceParticle[facing > 0 ? 0 : 1], _player.transform.position + Vector3.right * facing, Quaternion.identity);

                _player.CastDamage();
            }
        }

        

        public override void Update()
        {
            base.Update();

            Vector2 input = _player.PlayerInput.InputDirection.normalized;

            _mover.SetMovement(input);

            if (_delayStop > 0)
                _delayStop -= Time.deltaTime;

            if (_delayStop <= 0 && _isAtkStart)
            {
                _isAtkStart = false;
            }

        }

        public override void Exit()
        {
            _player.GetCompo<EntityRenderer>().OnAnimationTriggerEvent -= HandleAnimationTriggerEvent;
            AudioManager.Instance.PlaySound(SoundEnum.PlayerAttack, _player.transform);
            base.Exit();
        }

        
    }
}

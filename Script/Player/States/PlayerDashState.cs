using DG.Tweening;
using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Players
{
    public class PlayerDashState : EntityState
    {
        private Player _player;
        private EntityMovement _mover;

        private readonly float _dashDistance = 4.5f, _dashTime = 0.25f;
        public PlayerDashState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMovement>();
        }

        public override void Enter()
        {
            base.Enter();
            Vector2 inputDir = _player.PlayerInput.InputDirection;
            Vector2 dashDir = inputDir.normalized;

            _mover.CanManualMove = false;
            _mover.StopImmediately();
            Vector3 destination = _player.transform.position + (Vector3)dashDir * (_dashDistance - 0.5f);
            float dashTime = _dashTime;
            float distance = _dashDistance;

            if(_mover.CheckColliderInFront(dashDir, ref distance))
            {
                destination = _player.transform.position + (Vector3)dashDir * (distance - 0.5f);
                dashTime = distance * _dashTime / _dashDistance;
            }
            _player.transform.DOMove(destination, _dashTime).SetEase(Ease.OutQuad).OnComplete(EndDash);

            _player.GetCompo<HealthCompo>().InvincibleSetting(true);
            AudioManager.Instance.PlaySound(SoundEnum.PlayerDash, _player.transform);
        }

        public override void Exit()
        {
            _mover.StopImmediately();
            _mover.CanManualMove = true; //이동 잠그고 중력 끄기
            _player.GetCompo<HealthCompo>().InvincibleSetting(false);
            AudioManager.Instance.StopSound(SoundEnum.PlayerDash, _player.transform);
            base.Exit();
        }

        
        private void EndDash()
        {
            _player.ChangeState(_player.playerFSM[FSMState.Idle]);
        }
    }
}

using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using RPG.Players;
using System;
using UnityEngine;

public class PlayerParryingState : EntityState
{
    private Player _player;
    private EntityMovement _mover;
    public PlayerParryingState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
    {
        _player = entity as Player;
        _mover = _player.GetCompo<EntityMovement>();
    }

    public override void Enter()
    {
        base.Enter();
        _mover.CanManualMove = false;
        _mover.StopImmediately();
        RaycastHit2D[] colliders = Physics2D.CircleCastAll(_player.transform.position, _player.parryingRadius, Vector2.zero);
        if (colliders.Length > 0)
        {
            CameraManager.Instance.ShakeCamera(8, 10, 0.1f);
        }
        foreach (var col in colliders)
        {
            Debug.Log("¿÷¿Ω");
            if (col.transform.TryGetComponent(out IParryingable compo))
            {
                compo.Parrying();
                GameObject.Instantiate(_player.parryingParticle, _player.transform.position, Quaternion.identity);
                AudioManager.Instance.PlaySound(SoundEnum.PlayerParry, _player.transform);

                Debug.Log("Parryingµ ");
            }
            else
            {
                Debug.Log("¿Ã∞Õ¿∫ æ∆¥‘");
            }
        }
        _renderer.OnAnimationTriggerEvent+=HandleAnimationTriggerEvent;
    }

    private void HandleAnimationTriggerEvent(TriggertType type)
    {
        if (type == TriggertType.End)
        {
            _player.ChangeState(_player.playerFSM[FSMState.Idle]);
        }
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.Instance.PlaySound(SoundEnum.PlayerParry, _player.transform);
        _mover.CanManualMove = true;
        _renderer.OnAnimationTriggerEvent -= HandleAnimationTriggerEvent;
    }

}

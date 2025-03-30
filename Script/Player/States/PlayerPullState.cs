using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using RPG.Players;
using System;
using UnityEngine;

public class PlayerPullState : EntityState
{
    Player _player;
    EntityMovement _mover;
    public PlayerPullState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
    {
        _player = entity as Player;
        _mover = _player.GetCompo<EntityMovement>();
    }

    public override void Enter()
    {
        base.Enter();
        _player.isPullable = true;
        AudioManager.Instance.PlaySound(SoundEnum.PlayerPull, _player.transform);
        _player.PlayerInput.PullEvent += HandlePullEvent;
    }

    

    private void HandlePullEvent()
    {
        _player.pickAx.Pulling();
        _player.ChangeState(_player.playerFSM[FSMState.Idle]);
    }

    public override void Exit()
    {
        _player.isPullable = false;
        AudioManager.Instance.PlaySound(SoundEnum.PlayerPull, _player.transform);
        _player.PlayerInput.PullEvent -= HandlePullEvent;
        base.Exit();
    }
}

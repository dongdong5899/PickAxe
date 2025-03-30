using RPG.Entities;
using RPG.Players;
using UnityEngine;

public class PlayerAnimatorTrigger : MonoBehaviour,IEntityComponent
{
    private Player _player;
    public void Initialize(Entity entity)
    {
        _player = entity as Player;
    }

    private void AnimationEnd()
    {
        _player.AnimationFinishTrigger();
    }
}

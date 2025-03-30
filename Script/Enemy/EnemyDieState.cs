using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyDieState : EntityState
{
    private float _dieTime;
    private float _dissolveTime = 1.5f;


    public EnemyDieState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _dieTime = Time.time;
        _entity.GetCompo<EntityRenderer>().Dissolve(_dissolveTime);
        _entity.GetCompo<EntityMovement>()?.StopImmediately();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_dieTime + _dissolveTime < Time.time)
        {
            GameObject.Destroy(_entity.gameObject);
        }
    }
}

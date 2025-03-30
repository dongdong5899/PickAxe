using RPG.Entities;
using RPG.FSM;
using RPG.Players;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Enemy : Entity
{
    public EntityStateListSO enemyFSM;

    private StateMachine _stateMachine;
    private Dictionary<StateSO, EntityState> _stateDictionary;

    [SerializeField] private float _attackRange;
    private Player _player;

    [HideInInspector] public float lastAttackTime;

    public LayerMask whatIsTarget;

    //public AnimParamSO comboCounterParam; 만약 근접공격이면 추가

    protected override void Awake()
    {
        _player = GameManager.Instance.Player;
        base.Awake();
        #region StateMachine
        _stateMachine = new StateMachine();
        _stateDictionary = new Dictionary<StateSO, EntityState>();

        foreach (StateSO state in enemyFSM.states)
        {
            try
            {
                Type t = Type.GetType(state.className);
                var playerState = Activator.CreateInstance(t, this, state.animParam) as EntityState;
                _stateDictionary.Add(state, playerState);
            }
            catch (Exception ex)
            {
                Debug.LogError($"{state.className} loading Error, Message : {ex.Message}");
            }
        }
        #endregion

        GetCompo<HealthCompo>().OnDieEvent += HandleDieEvent;
    }

    private void HandleDieEvent()
    {
        ChangeState(enemyFSM[FSMState.Die]);
    }

    protected override void AfterInitComponents()
    {
        base.AfterInitComponents();
    }

    private void OnDestroy()
    {

    }

    private void Start()
    {
        if (enemyFSM[FSMState.Chase] != null)
            _stateMachine.Initialize(GetState(enemyFSM[FSMState.Chase]));
        else
            _stateMachine.Initialize(GetState(enemyFSM[FSMState.Idle]));
    }

    public void ChangeState(StateSO newState) => _stateMachine.ChangeState(GetState(newState));
    private EntityState GetState(StateSO stateSo) => _stateDictionary.GetValueOrDefault(stateSo);
    public bool AttackRangeInPlayer() => Vector3.Distance(_player.transform.position, transform.position) < _attackRange;
    public Vector3 PlayerDirection() => (_player.transform.position - transform.position).normalized;

    private void Update()
    {
        _stateMachine.UpdateStateMachine();
    }
}

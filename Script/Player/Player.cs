using RPG.Animators;
using RPG.Entities;
using RPG.FSM;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Players
{
    public class Player : Entity
    {
        public EntityStateListSO playerFSM;
        public Vector2 atkMovement;

        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [field: SerializeField] public LayerMask EnemyLayerMask { get; private set; }
        [field: SerializeField] public LayerMask WhatIsProj { get; private set; }
        private StateMachine _stateMachine;
        private Dictionary<StateSO, EntityState> _stateDictionary;

        private EntityMovement _mover;
        private EntityRenderer _renderer;
        private StatCompo _stat;
        [field: SerializeField] private float radius;
        [field: SerializeField] public float parryingRadius;
        [field: SerializeField] public Vector2 offset;
        [field: SerializeField] public Pickax pickAxPrafab;
        [field: SerializeField] public GameObject parryingParticle;
        [field: SerializeField] public ParticleSystem[] sliceParticle;
        [SerializeField] private GameObject _handWeapon;
        [SerializeField] public GameObject _attackParticle;
        public bool isPullable = false;
        public Pickax pickAx;

        public List<IPullable> pickedObject;


        [SerializeField] private AnimParamSO _attackSpeedSO;
        private float _currentDashCooltime;
        private float _currenPullCoolTime;
        public bool hasPickax { get; private set; } = true;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine();
            _stateDictionary = new Dictionary<StateSO, EntityState>();
            _currentDashCooltime = Time.time;
            foreach (StateSO state in playerFSM.states)
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
            _renderer = GetCompo<EntityRenderer>();
            _stat = GetCompo<StatCompo>();
            pickedObject = new List<IPullable>();
            PlayerInput.ParryingEvent += HandleParryingEvent;
            PlayerInput.PullEvent += HandlePullEvent;
        }

        private void HandlePullEvent()
        {
            if (_currenPullCoolTime < Time.time)
            {
                ChangeState(playerFSM[FSMState.Pull]);
                _currenPullCoolTime = Time.time + GetCompo<StatCompo>().GetValue(EStatType.ParryingCooldown);
            }
        }

        protected override void AfterInitComponents()
        {
            base.AfterInitComponents();

            GetCompo<HealthCompo>().OnDieEvent += HandleDieEvent;
            PlayerInput.DashEvent += HandleDash;
        }

        private void HandleDieEvent()
        {
            Time.timeScale = 0f;
            UIManager.Instance.OnGameOver();
        }

        private void OnDestroy()
        {
            PlayerInput.ParryingEvent -= HandleParryingEvent;
            PlayerInput.PullEvent -= HandlePullEvent;
            GetCompo<HealthCompo>().OnDieEvent -= HandleDieEvent;
            PlayerInput.DashEvent -= HandleDash;
        }

        private void HandleDash()
        {
            if (PlayerInput.InputDirection.sqrMagnitude > 0.05f && _currentDashCooltime < Time.time)
            {
                ChangeState(playerFSM[FSMState.Dash]);
                _currentDashCooltime = Time.time + GetCompo<StatCompo>().GetValue(EStatType.DashCooltime);
            }
        }
        private void HandleParryingEvent()
        {
            ChangeState(playerFSM[FSMState.Parrying]);
        }

        public void HandWeaponActive(bool active)
        {
            _handWeapon.SetActive(active);
            hasPickax = active;
        }
        private void Start()
        {
            _stateMachine.Initialize(GetState(playerFSM[FSMState.Idle]));
        }

        public void ChangeState(StateSO newState) => _stateMachine.ChangeState(GetState(newState));
        private EntityState GetState(StateSO stateSo) => _stateDictionary.GetValueOrDefault(stateSo);

        private void Update()
        {
            _stateMachine.UpdateStateMachine();

            if (hasPickax)
            {
                if (!_handWeapon.activeInHierarchy)
                    _handWeapon.SetActive(true);
            }
            else
            {
                if (_handWeapon.activeInHierarchy)
                    _handWeapon.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                GetCompo<HealthCompo>().InvincibleSetting(true);
            }
            //_renderer.SetParam(_attackSpeedSO, _stat.GetValue(EStatType.AttackSpeed));
        }

        public void AnimationFinishTrigger() => _stateMachine.currentState.AnimationEndTrigger();

        public void CastDamage()
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position + (Vector3)offset * GetCompo<EntityRenderer>().FacingDirection, radius, EnemyLayerMask);
            foreach (var col in collider)
            {
                if (col != null && col.TryGetComponent(out HealthCompo health))
                {
                    Debug.Log("µ•πÃ¡ˆ ¡‹");
                    health.ApplyDamage(Mathf.RoundToInt(GetCompo<StatCompo>().GetValue(EStatType.Damage)));
                    GameObject.Instantiate(_attackParticle, health.transform.position, Quaternion.identity);
                }
            }

        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + (Vector3)offset, radius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, parryingRadius);
        }
    }
}

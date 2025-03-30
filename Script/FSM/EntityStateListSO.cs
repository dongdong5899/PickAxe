using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.FSM
{
    public enum FSMState
    {
        Idle, Move, Attack, Dash, Chase, Die,ThrowStart,ThrowMaintain,Throw,Parrying,Pull,
        Skill1, Skill2, Skill3
    }
    
    [CreateAssetMenu(fileName = "EntityStateListSO", menuName = "SO/FSM/EntityStateList")]
    public class EntityStateListSO : ScriptableObject
    {
        public List<StateSO> states;
        private Dictionary<FSMState, StateSO> _stateDictionary;

        public StateSO this[FSMState stateName] => _stateDictionary.GetValueOrDefault(stateName);
        
        private void OnEnable()
        {
            _stateDictionary = new Dictionary<FSMState, StateSO>();
            foreach (var state in states)
            {
                _stateDictionary.Add(state.stateName, state);
            }
        }
    }
}

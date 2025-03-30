using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Entities
{
    public class Entity : MonoBehaviour
    {
        protected Dictionary<Type, IEntityComponent> _components;

        protected virtual void Awake()
        {
            _components = new Dictionary<Type, IEntityComponent>();
            GetComponentsInChildren<IEntityComponent>(true).ToList()
                .ForEach(component => _components.Add(component.GetType(), component));

            InitComponents();
            AfterInitComponents();
        }

        private void InitComponents()
        {
            _components.Values.ToList().ForEach(component => component.Initialize(this));
        }

        protected virtual void AfterInitComponents()
        {
            _components.Values.ToList().ForEach(component =>
            {
                if (component is IAfterInitable afterInitable)
                {
                    afterInitable.AfterInit();
                }
            });
        }

        public T GetCompo<T>(bool isDerived = false) where T : IEntityComponent
        {
            if (_components.TryGetValue(typeof(T), out IEntityComponent component))
            {
                return (T)component;
            }
            
            if(isDerived == false)
                return default;

            Type findType = _components.Keys.FirstOrDefault(t => t.IsSubclassOf(typeof(T)));
            if(findType != null) 
                return (T)_components[findType];
            
            return default;
        }
    }
}

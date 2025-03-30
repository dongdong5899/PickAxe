using RPG.Entities;
using System;
using System.ComponentModel;
using System.Data;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.Rendering.DebugUI;

public class HealthCompo : MonoBehaviour, IEntityComponent, IAfterInitable
{
    public int Health { get; private set; }

    private Entity _owner;
    private StatElement _maxHealth;
    private bool _isInvincible;
    private bool _isDie;

    public int MaxHealth => Mathf.RoundToInt(_maxHealth.Value);
    public event Action<int, int, bool> OnHealthChangedEvent;
    public event Action OnDieEvent;

    [SerializeField] private bool _isNonFeedback;

    public void InvincibleSetting(bool isInvincible)
    {
        _isInvincible = isInvincible;
    }

    public void Initialize(Entity entity)
    {
        _owner = entity;
    }

    public void AfterInit()
    {
        _maxHealth = _owner.GetCompo<StatCompo>().GetElement(EStatType.MaxHealth);
        _isInvincible = _maxHealth == null;
        Health = MaxHealth;
    }

    public void ApplyDamage(int damage, bool isChangeVisible = true)
    {
        if (_isDie) return;
        if (_isInvincible)
        {
            if (_isNonFeedback == false)
                UIManager.Instance.CreateDamageFeedback(transform.position, -1);
            return;
        }
        if (_isNonFeedback == false)
            UIManager.Instance.CreateDamageFeedback(transform.position, damage);

        _owner.GetCompo<EntityRenderer>().Blink(0.15f);

        int prev = Health;
        Health -= damage;
        if (Health < 0)
            Health = 0;
        OnHealthChangedEvent?.Invoke(prev, Health, isChangeVisible);



        if (Health == 0) Die();
    }

    public void ApplyRecovery(int recovery, bool isChangeVisible = true)
    {
        if (_isDie) return;

        int prev = Health;
        Health += recovery;
        if (Health > MaxHealth)
            Health = MaxHealth;
        OnHealthChangedEvent?.Invoke(prev, Health, isChangeVisible);
    }

    public void Resurrection()
    {
        _isDie = false;
        ApplyRecovery(MaxHealth, false);
    }

    public void Die()
    {
        _isDie = true;
        OnDieEvent?.Invoke();
    }
}

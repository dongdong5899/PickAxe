using RPG.Entities;
using UnityEngine;

public enum EStatType
{
    MaxHealth,
    Defense,
    ReceiveDamagePercent,
    Damage,
    Critical,
    CriticalPercent,
    InflictDamagePercent,
    Speed,
    Jump,
    AttackCooltime,
    ProjectileSpeed,
    DashCooltime,
    ThrowAttackSpeed,
    AttackSpeed,
    ThrowAttackDelay,
    ParryingCooldown
}

[System.Serializable]
public struct StatInfo
{
    public EStatType statType;
    public float defaultValue;
}

public class StatCompo : MonoBehaviour, IEntityComponent
{
    [SerializeField] private StatSO _stat;

    private Entity _owner;

    public void Initialize(Entity entity)
    {
        _owner = entity;
    }

    public StatElement GetElement(EStatType statType)
    {
        return _stat.GetStatElement(statType);
    }

    public float GetValue(EStatType statType)
    {
        StatElement stat = _stat.GetStatElement(statType);
        if (stat != null)
        {
            return stat.Value;
        }
        else
        {
            string statName = statType.ToString();
            if (statName.IndexOf("Percent") != -1)
                return 1;
            else
                return 0;
        }
    }
}

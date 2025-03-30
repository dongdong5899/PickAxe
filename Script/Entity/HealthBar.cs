using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthCompo _healthCompo;
    [SerializeField] private Transform _health, _changedHealth;

    [Header("Health changing setting")]
    [SerializeField] private float _donwSpeed = 8;
    [SerializeField] private float _changedBarWaitTime = 0.5f;

    private int _targetHealth;
    private float _targetHealthAmount;
    private float _lastDownTime;

    private void Awake()
    {
        _healthCompo.OnHealthChangedEvent += HandleHealthChangedEvent;
        _lastDownTime = Time.time;
    }

    private void Start()
    {
        HandleHealthChangedEvent(_healthCompo.Health, _healthCompo.Health, false);
    }

    private void HandleHealthChangedEvent(int prevHealth, int newHealth, bool isChangeVisible)
    {
        _targetHealth = newHealth;
        _targetHealthAmount = (float)newHealth / _healthCompo.MaxHealth;

        if (isChangeVisible == false)
        {
            _health.localScale = new Vector3(_targetHealthAmount, 1, 1);
            _changedHealth.localScale = new Vector3(_targetHealthAmount, 1, 1);
        }

        if (prevHealth > newHealth)
        {
            _lastDownTime = Time.time;
        }
    }

    private void Update()
    {
        if (Mathf.Abs(_health.localScale.x - _targetHealthAmount) > Mathf.Epsilon)
        {
            float healthChangeAmount = Mathf.Lerp(_health.localScale.x,
                _targetHealthAmount, Time.deltaTime * _donwSpeed);
            _health.localScale = new Vector3(healthChangeAmount, 1, 1);
        }
        if (Mathf.Abs(_changedHealth.localScale.x - _targetHealthAmount) > Mathf.Epsilon
            && _lastDownTime + _changedBarWaitTime < Time.time)
        {
            float healthChangeAmount = Mathf.Lerp(_changedHealth.localScale.x,
                _targetHealthAmount, Time.deltaTime * _donwSpeed);
            _changedHealth.localScale = new Vector3(healthChangeAmount, 1, 1);
        }
    }
}

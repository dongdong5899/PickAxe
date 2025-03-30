using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : Enemy
{
    [SerializeField] private Transform _projectilePointTrm;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Transform _inductioneRightPointTrm;
    [SerializeField] private Transform _inductioneLeftPointTrm;
    [SerializeField] private Induction _inductione;

    public event Action OnSkill1EndEvent;
    public event Action OnSkill2EndEvent;

    protected override void Awake()
    {
        base.Awake();
        GetCompo<HealthCompo>().OnDieEvent += () => UIManager.Instance.OnGameClear();
    }

    public void Skill1()
    {
        StartCoroutine(Skill1Co());
    }
    public void Skill2()
    {
        StartCoroutine(Skill2Co());
    }

    private IEnumerator Skill1Co()
    {
        for (int i = 0; i < 30; i++)
        {
            Projectile projectile = Instantiate(_projectile, _projectilePointTrm.position,
                Quaternion.LookRotation(Vector3.back, GameManager.Instance.Player.transform.position - _projectilePointTrm.position) * Quaternion.Euler(0, 0, 90));
            projectile.Init(this, whatIsTarget, 10, Mathf.RoundToInt(GetCompo<StatCompo>().GetValue(EStatType.Damage)));
            yield return new WaitForSeconds(0.1f);
        }
        OnSkill1EndEvent?.Invoke();
    }
    private IEnumerator Skill2Co()
    {
        for (int i = 0; i < 5; i++)
        {
            Induction projectile1 = Instantiate(_inductione, _inductioneRightPointTrm.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            projectile1.Init(this, GameManager.Instance.Player, whatIsTarget, 6, Mathf.RoundToInt(GetCompo<StatCompo>().GetValue(EStatType.Damage)));

            Induction projectile2 = Instantiate(_inductione, _inductioneLeftPointTrm.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            projectile2.Init(this, GameManager.Instance.Player, whatIsTarget, 6, Mathf.RoundToInt(GetCompo<StatCompo>().GetValue(EStatType.Damage)));

            yield return new WaitForSeconds(0.1f);
        }
        OnSkill1EndEvent?.Invoke();
    }
}

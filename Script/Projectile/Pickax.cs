using DG.Tweening;
using RPG.Entities;
using RPG.Players;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pickax : MonoBehaviour, IParryingable, IPullable
{
    private Entity _owner;
    private LayerMask _whatIsTarget;
    private float _speed;
    private int _damage;

    [SerializeField] private float _pickUpTime = 0.5f;
    [SerializeField] private float _lifetime = 2f;
    [SerializeField] private float _invingibleTime = 0.2f;
    private float _createTime;

    [SerializeField] private bool _isProjectile;
    private SpriteRenderer _spriteRender;
    private CapsuleCollider2D _damageCollider;
    private Player _player;
    private Vector2 _fireDir;
    private bool _isPick;
    private bool _isInvingible;

    private void Awake()
    {
        _spriteRender = GetComponentInChildren<SpriteRenderer>();
        _damageCollider = GetComponent<CapsuleCollider2D>();
    }

   

    public void Init(Entity owner, LayerMask whatIsTarget, float speed, int damage)
    {
        _owner = owner;
        _player = _owner as Player;
        _whatIsTarget = whatIsTarget;
        _speed = speed;
        _damage = damage;

        _isProjectile = true;
        _createTime = Time.time;
        transform.DORotate(new Vector3(0, 0, transform.rotation.z-360), _lifetime, RotateMode.FastBeyond360);
        _fireDir = (_player.PlayerInput.MousePos - (Vector2)_player.transform.position).normalized;
    }

    private void FixedUpdate()
    {
        if (_isProjectile)
            ProjectileFire();
    }

    private void ProjectileFire()
    {
        if (_lifetime + _createTime < Time.time)
        {
            _isProjectile = false;
        }
        if (_invingibleTime + _createTime < Time.time)
        {
            _isInvingible = true;
        }


        Vector3 movement = _fireDir * (Time.fixedDeltaTime * _speed);
        transform.position += movement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isInvingible) return;

        if (_isProjectile)
        {
            if (collision.TryGetComponent<Entity>(out Entity unit))
            {
                
                unit.GetCompo<HealthCompo>().ApplyDamage(_damage);
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                transform.DOMove(_owner.transform.position, _pickUpTime);
                transform.DOScale(0, _pickUpTime).OnComplete(() =>
                {
                    Destroy(gameObject);
                    _player.HandWeaponActive(true);
                });
            }
        }
    }

    public void Parrying()
    {
        if (_isProjectile)
        {
            _fireDir = (_player.PlayerInput.MousePos - (Vector2)_player.transform.position).normalized;
            Init(_player,_player.EnemyLayerMask, _player.GetCompo<StatCompo>().GetValue(EStatType.ThrowAttackSpeed), Mathf.RoundToInt(_player.GetCompo<StatCompo>().GetValue(EStatType.Damage) * 1.5f));
        }
    }


    

    

    public void Pulling()
    {
        _fireDir = ((Vector2)_player.transform.position - (Vector2)transform.position).normalized;
        _isProjectile = true;
        _damage = 1;
        _createTime = Time.time;
        transform.DORotate(new Vector3(0, 0, 360), _lifetime, RotateMode.FastBeyond360);
    }
}

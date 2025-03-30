using RPG.Entities;
using RPG.Players;
using UnityEngine;
using UnityEngine.EventSystems;

public class Induction : MonoBehaviour, IPullable, IParryingable, IPointerDownHandler
{
    private ProjectileBoxCollider2D _collider;
    private Entity _owner, _target;
    private LayerMask _whatIsTarget;
    private float _speed;
    private int _damage;
    private bool _isEnable;
    private bool _isPull;
    private bool _isPick;

    [SerializeField] private float _lifetime = 5f;
    [SerializeField] private ParticleSystem _hitEffect;
    private float _createTime;

    private void Awake()
    {
        _collider = GetComponent<ProjectileBoxCollider2D>();
    }

    public void Init(Entity owner, Entity target, LayerMask whatIsTarget, float speed, int damage)
    {
        _owner = owner;
        _target = target;
        _whatIsTarget = whatIsTarget;
        _speed = speed;
        _damage = damage;

        _isEnable = true;
        _createTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (_lifetime + _createTime < Time.time)
        {
            Instantiate(_hitEffect, transform.position, Quaternion.identity);
            Die();
            _isEnable = false;
        }

        if (_isEnable == false) return;

        Vector3 movement = _isPull? (_target.transform.position - transform.position).normalized : transform.up * (Time.fixedDeltaTime * _speed);
        if (_collider.CheckCollision(_whatIsTarget, movement))
        {
            foreach (var hit in _collider.GetHits())
            {
                if (hit.transform.TryGetComponent<Entity>(out Entity unit))
                {
                    unit.GetCompo<HealthCompo>().ApplyDamage(_damage);
                    CameraManager.Instance.ShakeCamera(6, 6, 0.15f);
                    Instantiate(_hitEffect, hit.point, Quaternion.identity);
                    Die();
                }
                _isEnable = false;
                transform.position += transform.up * hit.distance;
                break;
            }
        }
        else
        {
            transform.position += movement;
        }

        if(!_isPull)
            transform.up = transform.up * 15 + (_target.transform.position - transform.position).normalized;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void Pulling()
    {
        _createTime = Time.time;
        _damage = 1;
        _isPull = true;
    }

    public void Parrying()
    {
        _isPull = false;
        _damage = Mathf.RoundToInt(_target.GetCompo<StatCompo>().GetValue(EStatType.Damage) * 1.5f);
        _createTime = Time.time;
        Player player = GameManager.Instance.Player;
        _whatIsTarget = player.EnemyLayerMask;
        Entity temp = _owner;
        _owner = _target;
        _target = temp;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Player player = GameManager.Instance.Player;
        if (player.isPullable)
        {
            IPullable pool = GetComponent<IPullable>();
            if (_isPick)
            {
                player.pickedObject.Remove(pool);
                Debug.Log("눌림");
                _isPick = false;
            }
            else
            {
                player.pickedObject.Add(pool);
                Debug.Log("해제");
                _isPick = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Map"))
        {
            Destroy(gameObject);
        }
    }
}

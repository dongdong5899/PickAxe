using RPG.Entities;
using RPG.Players;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile : MonoBehaviour, IPullable, IParryingable, IPointerDownHandler
{
    private ProjectileBoxCollider2D _collider;
    private Entity _owner;
    private LayerMask _whatIsTarget;
    private float _speed;
    private int _damage;
    private bool _isEnable;
    private Vector2 _dir;
    private bool _isPick;

    [SerializeField] private float _lifetime = 5f;
    [SerializeField] private ParticleSystem _hitEffect;
    private float _createTime;

    private void Awake()
    {
        _collider = GetComponent<ProjectileBoxCollider2D>();
    }

    public void Init(Entity owner, LayerMask whatIsTarget, float speed, int damage)
    {
        _owner = owner;
        _whatIsTarget = whatIsTarget;
        _speed = speed;
        _damage = damage;

        _isEnable = true;
        _createTime = Time.time;
        _dir = transform.right;
    }

    private void FixedUpdate()
    {
        if (_lifetime + _createTime < Time.time)
        {
            Die();
            _isEnable = false;
        }

        if (_isEnable == false) return;

        Vector3 movement = _dir * (Time.fixedDeltaTime * _speed);
        if (_collider.CheckCollision(_whatIsTarget, movement))
        {
            foreach (var hit in _collider.GetHits())
            {
                if (hit.transform.TryGetComponent<Entity>(out Entity unit))
                {
                    unit.GetCompo<HealthCompo>().ApplyDamage(_damage);
                    CameraManager.Instance.ShakeCamera(4, 4, 0.15f);
                    Instantiate(_hitEffect, hit.point, Quaternion.identity);
                    Die();
                }
                _isEnable = false;
                transform.position += transform.right * hit.distance;
                break;
            }
        }
        else
        {
            transform.position += movement;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void Pulling()
    {
        _damage = 1;
        _createTime = Time.time;
        _dir = (GameManager.Instance.Player.transform.position - transform.position).normalized;
    }

    public void Parrying()
    {
        Player player = GameManager.Instance.Player;
        _createTime = Time.time;
        _damage = Mathf.RoundToInt(player.GetCompo<StatCompo>().GetValue(EStatType.Damage) * 1.5f);
        _whatIsTarget = player.EnemyLayerMask;
        _dir = (player.PlayerInput.MousePos - (Vector2)player.transform.position).normalized;
        transform.right = _dir;
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
}

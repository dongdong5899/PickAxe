using System;
using RPG.Animators;
using UnityEngine;

namespace RPG.Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EntityMovement : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private LayerMask _whatIsWall;
        private Vector2 _movement;

        public Vector2 Velocity => _rbCompo.linearVelocity;
        public bool CanManualMove { get; set; } = true; //키보드로 움직임 가능
        public float SpeedMultiplier { get; set; } = 1f;

        private Rigidbody2D _rbCompo;
        private Entity _entity;
        private EntityRenderer _renderer;
        private StatCompo _statCompo;


        private Collider2D _collider;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _rbCompo = _entity.GetComponent<Rigidbody2D>();
            _renderer = _entity.GetCompo<EntityRenderer>();
            _collider = _entity.GetComponent<Collider2D>();
            _statCompo = _entity.GetCompo<StatCompo>();
        }


        public void AddForceToEntity(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse)
        {
            _rbCompo.AddForce(force, mode);
        }

        public bool CheckColliderInFront(Vector2 direction, ref float distance)
        {
            Vector2 center = _collider.bounds.center;
            Vector2 size = _collider.bounds.size;
            size.y -= 0.3f; //빼는 이유는 안빼면 바닥하고 충돌해.

            var hit = Physics2D.BoxCast(center, size, 0f, direction, distance, _whatIsWall);
            if (hit)
                distance = hit.distance;
            return hit;
        }


        public void StopImmediately()
        {
            _rbCompo.linearVelocity = Vector2.zero;
            _movement = Vector2.zero;
        }

        public void SetMovement(Vector2 movement) => _movement = movement;

        private void FixedUpdate()
        {
            MoveCharacter();
        }

        private void MoveCharacter()
        {
            if (CanManualMove)
                _rbCompo.linearVelocity = _movement * _statCompo.GetValue(EStatType.Speed) * SpeedMultiplier;

            _renderer.FlipController(_movement.x);
        }
    }
}

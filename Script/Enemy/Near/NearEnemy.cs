using RPG.Entities;
using RPG.Players;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class NearEnemy : Enemy
{
    [SerializeField] private ParticleSystem attackEffect;

    [SerializeField] private float _radius;
    [SerializeField] private Vector2 _offset;

    public void Attack()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position + (Vector3)_offset * GetCompo<EntityRenderer>().FacingDirection, _radius, whatIsTarget);
        if (collider != null && collider.TryGetComponent(out HealthCompo health))
        {
            CameraManager.Instance.ShakeCamera(4, 4, 0.15f);
            health.ApplyDamage(Mathf.RoundToInt(GetCompo<StatCompo>().GetValue(EStatType.Damage)));
            GameObject.Instantiate(attackEffect, health.transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (Vector3)_offset, _radius);
    }
}

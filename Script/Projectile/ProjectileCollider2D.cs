using UnityEngine;

public abstract class ProjectileCollider2D : MonoBehaviour
{
    protected RaycastHit2D[] _hits;

    public abstract bool CheckCollision(LayerMask whatIsTarget, Vector2 moveTo = default);
    public RaycastHit2D[] GetHits()
    {
        return _hits;
    }
}

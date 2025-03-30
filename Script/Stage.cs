using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private Collider2D _outPointWallColl;
    [SerializeField] private Transform _startTrm;
    private bool _isClear;

    private void OnEnable()
    {
        GameManager.Instance.Player.transform.position = _startTrm.position;
    }

    private void Update()
    {
        if (_isClear) return;

        bool flag = true;
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null) flag = false;
        }

        if (flag)
        {
            _isClear = true;
            _outPointWallColl.enabled = false;
        }
    }
}

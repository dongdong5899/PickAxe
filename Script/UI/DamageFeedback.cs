using TMPro;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    [SerializeField] private float _lifetime = 1.5f;
    [SerializeField] private float _upSpeed = 1f;
    private float _currentTime;
    private TextMeshPro _text;

    public void Init(int damage, float spread)
    {
        _text = GetComponent<TextMeshPro>();

        if (damage == -1)
        {
            _text.text = "È¸ÇÇ";
            _text.color = Color.gray;
        }
        else
        {
            _text.text = damage.ToString();
            _text.color = Color.white;
        }
        _currentTime = 0;
        transform.position += Random.insideUnitSphere * spread;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > _lifetime)
        {
            Destroy(gameObject);
        }

        transform.position += Vector3.up * _upSpeed * Time.deltaTime;
        _text.alpha = (_lifetime - _currentTime) / (_lifetime + 0.5f);
    }
}

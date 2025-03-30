using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _Instance;
    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindFirstObjectByType<T>();
                if (_Instance == null)
                {
                    Debug.LogWarning($"{_Instance.name}을 안넣었단다...");
                }
            }
            return _Instance;
        }
    }
}

using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<T>();
                if (instance == null)
                {
                    var newObj = new GameObject(typeof(T).ToString()); // 오브젝트 추가
                    instance = newObj.AddComponent<T>(); // 스크립트 추가
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
            instance = this as T;
    }
}

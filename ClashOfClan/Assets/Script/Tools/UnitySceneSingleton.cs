using UnityEngine;
using System.Collections;

// 场景单例,该单例只在本场景有效,场景切换后会被销毁;
public class UnitySceneSingleton<T> : MonoBehaviour
where T:Component{
    private static T _Instance;
    public static T Instance
    {
        get
        {
            if ( _Instance == null )
            {
                _Instance = FindObjectOfType(typeof(T)) as T;
                if ( _Instance == null )
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _Instance = obj.AddComponent(typeof(T)) as T;
                }
            }

            return _Instance;
        } 
    }
}

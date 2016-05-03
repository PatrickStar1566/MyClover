using UnityEngine;
using System.Collections;

public class UnityAllSceneSingleton<T> : MonoBehaviour
    // 保证所有静态类必须是控件;
    where T:Component{
    private static T _Instance;
    public static T Instance
    {
        get
        {
            if ( _Instance == null )
            {
                // 找到所有T类型控件;
                _Instance = FindObjectOfType(typeof(T)) as T;
                if ( _Instance == null )
                {
                    GameObject obj = new GameObject();
                    // 隐藏方式;
                    // DontSave 不保存,切场景不丢失;
                    // HideAndDontSave 不保存,并且隐藏;
                    // HideInHierarchy 对象列表隐藏;
                    // HideInInspector 属性栏隐藏;
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _Instance = (T)obj.AddComponent(typeof(T));
                }
            }

            return _Instance;
        }
    }

    public virtual void Awake()
    {
        // 切换场景后不删除;
        DontDestroyOnLoad(this.gameObject);

        if ( _Instance == null )
        {
            // 如果单例为空,将this赋值;
            _Instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;
using System.Collections;

namespace Core
{
    // partial 类可拆分,易于扩展;
    public partial class EventSystem
    {
        public delegate void NoParamDelegate();
        public delegate void OneParamDelegate<T>(T t);
        // other delegate
        public delegate void TargetChangeHandle(Vector3 pos, bool isTerrain);
    }
}

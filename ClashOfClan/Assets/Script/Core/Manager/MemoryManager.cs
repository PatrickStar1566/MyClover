using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class MemoryManager : UnityAllSceneSingleton<MemoryManager>, IMessageObject {
    List<object> structs = new List<object>();
    Dictionary<string, List<object>> structLists = new Dictionary<string, List<object>>();

    public object CreateNativeStruct(string className)
    {
        this.START_METHOD("CreateNativeStruct");

        if ( structs.Count <= 100 )
        {
            if (!structLists.ContainsKey(className))
            {
                structLists.Add(className, structs);
            }
            else
                structs = structLists[className];

            // 实例化;
            object ob = Activator.CreateInstance(Type.GetType(className));
            structs.Add(ob);
            this.END_METHOD("CreateNativeStruct");
            return ob;
        }
        throw new UnityException("try to create wrong struct");
    }

    public override void Awake()
    {
        base.Awake();
        // 启动后延迟120秒,每300秒执行一次;
        // InvokeRepeating("ResizeDic", 120, 300);
    }
    // ResizeDic()
    //{
    //}
}

using UnityEngine;
using System.Collections;

public interface IMessageObject
{

}

public static class MessageObject
{
    public static void START_METHOD<T>(this T t, string methodName) where T : IMessageObject
    {
#if NEEDLOGMETHOD
        Debug.Log("start method : " + t.GetType().Name + ".Method:" + methodName + "======");
#endif
    }

    public static void END_METHOD<T>(this T t, string methodName) where T : IMessageObject
    {
#if NEEDLOGMETHOD
        Debug.Log("end method : " + t.GetType().Name + ".Method:" + methodName + "======");
#endif
    }

    public static void PRINT<T>(this T t, string msg) where T : IMessageObject
    {
#if NEEDLOGMETHOD
        Debug.Log("method : " + t.GetType().Name + ".Message : " + msg);
#endif
    }
}

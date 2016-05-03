using UnityEngine;
using System.Collections;

public static class PlatformUtil
{
    public static bool IsTouchDevice
    {
        get
        {
            return Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.Android;
        }
    }
}

public interface IGameInput
{
    bool IsClickDown { get; }

    bool IsClickUp { get; }

    bool IsClicking { get; }

    bool HasTouch { get; }

    bool IsMove { get; }

    Vector3 MousePosition { get; }

    int TouchCount { get; }
}

public class WinGameInput:IGameInput
{
    public static float EdgeLeftX = 20.2f;
    public static float EdgeRightX = 38.3f;
    public static float EdgeDownY = 7.6f;
    public static float EdgeUpY = 21.6f;
    public static float CameraMoveSpeed = 0.3f;
    public static float EdgeWidth = 0.5f;

    public bool IsClickDown { get { return Input.GetMouseButtonDown(0); } }

    public bool IsClickUp { get { return Input.GetMouseButtonUp(0); } }

    public bool IsMove { get { return IsClicking; } }

    public bool IsClicking { get { return Input.GetMouseButton(0); } }

    public Vector3 MousePosition { get { return Input.mousePosition; } }

    public bool HasTouch { get { return true; } }

    public int TouchCount { get { return 1; } }
}

public class SingleTouchGameInput:IGameInput
{
    public bool IsClickDown
    {
        get
        {
            return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began;
        }
    }

    public bool IsClickUp
    {
        get
        {
            return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended;
        }
    }

    public bool IsMove
    {
        get
        {
            return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved;
        }
    }

    public bool IsClicking
    {
        get
        {
            return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Stationary;
        }
    }

    public Vector3 MousePosition
    {
        get
        {
            if ( Input.touchCount == 1 )
            {
                return Input.GetTouch(0).position;
            }
            else
            {
                return Input.mousePosition;
            }
        }
    }

    public bool HasTouch
    {
        get
        {
            return Input.touchCount > 0;
        }
    }

    public int TouchCount
    {
        get
        {
            return Input.touchCount;
        }
    }
}
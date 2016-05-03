using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InputListener : UnitySceneSingleton<InputListener>
{
	private static IGameInput _gameInput;
	float xSpeed = -1f;
	float ySpeed = -1f;

	// Use this for initialization
	private static InputListener _Inst;
	public static void Init()
	{
		_Inst = InputListener.Instance;
	}

	void Start () {
		UICamera.genericEventHandler = this.gameObject;
		if ( PlatformUtil.IsTouchDevice )
		{
			_gameInput = new SingleTouchGameInput();
		}
		else
		{
			_gameInput = new WinGameInput();
		}
	}
	float deltaX = 0;
	float deltaY = 0;
	Vector3 lastPosition;
	float lastScale;

	// Update is called once per frame
	void Update () {
	
	}

	void OnDrag(Vector2 delta)
	{
		Vector3 newPos = new Vector3(Camera.main.transform.position.x + delta.x * xSpeed * 0.02f
					, Camera.main.transform.position.y + delta.y * ySpeed * 0.02f
					, Camera.main.transform.position.z);

		if (!(newPos.x > WinGameInput.EdgeRightX || newPos.x < WinGameInput.EdgeLeftX ||
					newPos.y < WinGameInput.EdgeDownY || newPos.y > WinGameInput.EdgeUpY))
		{
			Camera.main.transform.position = newPos;
		}
	}

	float lastMoveSpeedX = 0.0f;
	float lastMoveSpeedY = 0.0f;
	float ax = 0.0f;
	float ay = 0.0f;
	bool isTwing = false;

	void LateMove()
	{
		if ( Mathf.Abs(lastMoveSpeedX) < WinGameInput.EdgeWidth )
		{
			lastMoveSpeedX = 0.0f;
			ax = 0.0f;
			isTwing = false;
		}

		if ( Mathf.Abs(lastMoveSpeedY) < WinGameInput.EdgeWidth )
		{
			lastMoveSpeedY = 0.0f;
			ay = 0.0f;
			isTwing = false;
		}

		if ( ax != 0.0f )
		{
			if (lastMoveSpeedX < 0)
				lastMoveSpeedX += ax * Time.deltaTime;
			else
				lastMoveSpeedX -= ax * Time.deltaTime;

			if ( Camera.main.transform.localPosition.x > WinGameInput.EdgeLeftX
				&& Camera.main.transform.localPosition.x < WinGameInput.EdgeRightX )
			{
				Camera.main.transform.Translate(lastMoveSpeedX, 0, 0);
			}
			else
			{
				lastMoveSpeedX = 0.0f;
				isTwing = false;
			}
		}
		else if ( ay != 0.0f )
		{
			if (lastMoveSpeedY < 0)
				lastMoveSpeedY += ay * Time.deltaTime;
			else
				lastMoveSpeedY -= ay * Time.deltaTime;

			if (Camera.main.transform.localPosition.y > WinGameInput.EdgeDownY
				&& Camera.main.transform.localPosition.y < WinGameInput.EdgeUpY)
			{
				Camera.main.transform.Translate(0, lastMoveSpeedY, 0);
			}
			else
			{
				lastMoveSpeedY = 0.0f;
				isTwing = false;
			}
		}
	}

	void LateUpdate()
	{
		if ( _gameInput.IsClickDown )
		{
			if (!PlatformUtil.IsTouchDevice)
			{
				lastPosition = _gameInput.MousePosition;
			}
		}
		if ( _gameInput.IsClickUp )
		{
			if (!PlatformUtil.IsTouchDevice)
				lastPosition = _gameInput.MousePosition;
			else
			{
				// 计算加速度(移动速度-初始速度)/时间;
				ax = (Mathf.Abs(WinGameInput.CameraMoveSpeed) - 0.0f) / 1.0f;
				ay = (Mathf.Abs(WinGameInput.CameraMoveSpeed) - 0.0f) / 1.0f;
				isTwing = true;
			}
		}
		// twing;
		LateMove();

        // 这里屏蔽是因为引入NGUI插件;
        if ( false && _gameInput.IsMove )
		{
			if ( !PlatformUtil.IsTouchDevice )
			{
				deltaX = -(_gameInput.MousePosition - lastPosition).x;
				deltaY = -(_gameInput.MousePosition - lastPosition).y;
				float realDeltaX = 0;
				float realDeltaY = 0;
				if (Mathf.Abs(deltaX) > 9.99999944E-11f)
					realDeltaX = deltaX;
				if (Mathf.Abs(deltaY) > 9.99999944E-11f)
					realDeltaY = deltaY;

				Vector3 newPos = new Vector3(Camera.main.transform.position.x + realDeltaX * xSpeed * 0.02f
					, Camera.main.transform.position.y + realDeltaY * ySpeed * 0.02f
					, Camera.main.transform.position.z);

				if ( !(newPos.x > WinGameInput.EdgeRightX || newPos.x < WinGameInput.EdgeLeftX ||
					newPos.y < WinGameInput.EdgeDownY || newPos.y > WinGameInput.EdgeUpY ))
				{
					Camera.main.transform.position = newPos;
				}
				lastPosition = _gameInput.MousePosition;
			}
		}
		else
		{
			if ( !isTwing && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
				if ( Mathf.Abs(touchDeltaPosition.x) > Mathf.Abs(touchDeltaPosition.y) )
				{
					lastMoveSpeedY = 0.0f;
					float deltaX = Mathf.Abs(touchDeltaPosition.x * 0.2f);
					if (touchDeltaPosition.x < 0 && Camera.main.transform.localPosition.x < WinGameInput.EdgeRightX )
					{
						lastMoveSpeedX = WinGameInput.CameraMoveSpeed * deltaX;
						Camera.main.transform.Translate(lastMoveSpeedX, 0, 0);
					}
					else if (Camera.main.transform.localPosition.x > WinGameInput.EdgeLeftX)
					{
						lastMoveSpeedX = -WinGameInput.CameraMoveSpeed * deltaX;
						Camera.main.transform.Translate(lastMoveSpeedX, 0, 0);
					}
				}
				else
				{
					lastMoveSpeedX = 0.0f;
					float deltaY = Mathf.Abs(touchDeltaPosition.y * 0.2f);
					if (touchDeltaPosition.y > 0 && Camera.main.transform.localPosition.y > WinGameInput.EdgeDownY)
					{
						lastMoveSpeedY = -WinGameInput.CameraMoveSpeed * deltaY;
						Camera.main.transform.Translate(0, lastMoveSpeedY, 0);
					}
					else if (Camera.main.transform.localPosition.y < WinGameInput.EdgeUpY)
					{
						lastMoveSpeedY = WinGameInput.CameraMoveSpeed * deltaY;
						Camera.main.transform.Translate(0, WinGameInput.CameraMoveSpeed * deltaY, 0);
					}
				}
			}

			if ( _gameInput.TouchCount > 1 )
			{
				if ( Input.GetTouch(0).phase == TouchPhase.Moved
					&& Input.GetTouch(1).phase == TouchPhase.Moved )
				{
					Vector2 curDist = Input.GetTouch(0).position - Input.GetTouch(1).deltaPosition;
					Vector2 preDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));

					float touchDelta = curDist.magnitude - preDist.magnitude;
					if ( touchDelta > 0 )
					{
						if (Camera.main.orthographicSize - 0.5f > 5.0f
							&& Camera.main.orthographicSize - 0.5f < 12.0f)
							Camera.main.orthographicSize += 0.5f;
					}

					if (touchDelta < 0)
					{
						if (Camera.main.orthographicSize + 0.5f > 5.0f
							&& Camera.main.orthographicSize + 0.5f < 12.0f)
							Camera.main.orthographicSize += 0.5f;
					}
				}
			}
		}
	}
}

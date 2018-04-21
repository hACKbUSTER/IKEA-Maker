using UnityEngine;
using UnityEngine.Events;
using System;

namespace FinGameWorks.Scripts.View
{
	
	public class WindowRatioMonitor : Singleton<WindowRatioMonitor>
	{
		public class RectEvent : UnityEvent<Rect>
		{
			
		}
		public Rect CurrentRect;

		public RectEvent ScreenSizeChangedAction
		{
			get
			{
				if (_screenSizeChangedAction == null)
				{
					_screenSizeChangedAction = new RectEvent();
				}
				return _screenSizeChangedAction;
			}
			set { _screenSizeChangedAction = value; }
		}
		private RectEvent _screenSizeChangedAction;

		private void Awake()
		{
			CurrentRect = Camera.main.pixelRect;
			ScreenSizeChangedAction.Invoke(CurrentRect);
		}

		private void Start()
		{
			CurrentRect = Camera.main.pixelRect;
			ScreenSizeChangedAction.Invoke(CurrentRect);
		}

		private void FixedUpdate()
		{
			if ( Math.Abs(Camera.main.pixelRect.width - CurrentRect.width) > 0.1 ||  Math.Abs(Camera.main.pixelRect.height - CurrentRect.height) > 0.1)
			{
//				Debug.Log("ScreenSizeChangedAction");
				CurrentRect = Camera.main.pixelRect;
				ScreenSizeChangedAction.Invoke(CurrentRect);
			}
		}

		private void OnDisable()
		{
			WindowRatioMonitor.Instance.ScreenSizeChangedAction.RemoveAllListeners();
		}
	}
}

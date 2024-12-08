using System;
using System.Collections.Generic;
using Wave.UI;

namespace Wave.Services
{
	public class UiService : IService
	{
		private Dictionary<Type, UIScreen> _screens = new Dictionary<Type, UIScreen>();
		private UIScreen _activeScreen;
		private Type _lastCalledScreenType;

		public void RegisterScreen<T>(T screen) where T : UIScreen
		{
            _screens[typeof(T)] = screen;

			if (_lastCalledScreenType != null && _lastCalledScreenType == typeof(T))
				ChangeScreen(screen);
			else
				screen.Exit();
        }

		public void UnregisterScreen<T>()
		{
			if (!_screens.ContainsKey(typeof(T)))
				return;

			_screens.Remove(typeof(T));
		}

		public void ShowScreen<T>() where T : UIScreen
		{
			if (!_screens.TryGetValue(typeof(T), out UIScreen screen))
			{
				_lastCalledScreenType = typeof(T);
				return;
			}

			ChangeScreen(screen);
		}

		public bool IsScreenActive<T>() where T : UIScreen
		{
			return _activeScreen != null && _activeScreen.GetType() == typeof(T);
		}

		private void ChangeScreen(UIScreen screen)
		{
            if (_activeScreen != null)
            {
                _activeScreen.Exit();
                _activeScreen = null;
            }

            _activeScreen = screen;
            _activeScreen.Enter();
        }
	} 
}

using System;
using Wave.Services;

namespace Wave.States
{
	public class StateMachine : IDisposable
	{
        private readonly UpdateService _updateService;
        private IState _currentState;

        public StateMachine()
        {
            _updateService = ServiceLocator.Instance.Get<UpdateService>();
            _updateService.Update.Add(Update);
        }

        public void SetState(IState state)
        {
            if (state == null)
                return;

            if (_currentState != null)
            {
                if (_currentState == state)
                {
                    _currentState.Enter();
                    return;
                }

                _currentState.Exit();
                _currentState = null;
            }

            _currentState = state;
            _currentState.Enter();
        }

        public bool IsInState<T>() where T : IState
        {
            return _currentState is T;
        }

        private void Update(float dt)
        {
            if (_currentState != null)
                _currentState.Execute();
        }

        public void Dispose()
        {
            _updateService.Update.Remove(Update);
        }
    } 
}

using System;
using Wave.Services;

namespace Wave.States
{
	public class StateMachine : IDisposable
	{
        private readonly UpdateService _updateService;
        public IState CurrentState { get; private set; }

        public StateMachine()
        {
            _updateService = ServiceLocator.Instance.Get<UpdateService>();
            _updateService.Update.Add(Update);
        }

        public void SetState(IState state)
        {
            if (state == null)
                return;

            if (CurrentState != null)
            {
                if (CurrentState == state)
                    return;

                CurrentState.Exit();
            }

            CurrentState = state;
            CurrentState.Enter();
        }

        private void Update(float dt)
        {
            if (CurrentState != null)
                CurrentState.Execute();
        }

        public void Dispose()
        {
            _updateService.Update.Remove(Update);
        }
    } 
}

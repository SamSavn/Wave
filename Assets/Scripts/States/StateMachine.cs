namespace Wave.States
{
	public class StateMachine
	{
        public IState CurrentState { get; private set; }

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

        public void Update()
        {
            if (CurrentState != null)
                CurrentState.Execute();
        }
    } 
}

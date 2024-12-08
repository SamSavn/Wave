using UnityEngine;

namespace Wave.States.PlayerStates
{
    public class PlayerPausedState : IState
    {
        private Rigidbody _rigidbody;

        public PlayerPausedState(Rigidbody rigidbody)
        {
            _rigidbody = rigidbody;
        }

        public void Enter()
        {
            _rigidbody.Sleep();
        }

        public void Execute()
        {
            
        }

        public void Exit()
        {
            _rigidbody.WakeUp();
            _rigidbody = null;
        }
    }
}

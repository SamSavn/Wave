using System;
using UnityEngine;

namespace Wave.States.PlayerStates
{
    public class FallingState : IPlayerState
    {
        private Rigidbody _playerBody;
        private Action[] _onExecute;

        public FallingState(Rigidbody rigidbody, params Action[] onExecute)
        {
            _playerBody = rigidbody;
            _onExecute = onExecute;
        }

        public void Enter()
        {
            
        }

        public void Execute()
        {
            if (_playerBody.isKinematic)
                return;

            _playerBody.linearVelocity += Physics.gravity * Time.fixedDeltaTime * 2;

            foreach (Action action in _onExecute)
                action?.Invoke();
        }

        public void Exit()
        {
            _playerBody = null;
            _onExecute = null;
        }
    }
}

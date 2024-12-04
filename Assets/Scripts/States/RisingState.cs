using System;
using UnityEngine;
using Wave.Actors;

namespace Wave.States.PlayerStates
{
    public class RisingState : IPlayerState
    {
        private readonly Rigidbody _playerBody;
        private readonly Action[] _onExecute;
        private readonly float _force;

        public RisingState(Rigidbody rigidbody, float force, params Action[] onExecute)
        {
            _playerBody = rigidbody;
            _onExecute = onExecute;
            _force = force;
        }

        public void Enter()
        {

        }

        public void Execute()
        {
            if (_playerBody.isKinematic)
                _playerBody.isKinematic = false;

            _playerBody.AddForce(Vector3.up * _force, ForceMode.Force);

            foreach (Action action in _onExecute)
                action?.Invoke();
        }

        public void Exit()
        {

        }
    } 
}

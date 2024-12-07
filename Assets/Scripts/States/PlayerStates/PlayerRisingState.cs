using System;
using UnityEngine;
using Wave.Extentions;

namespace Wave.States.PlayerStates
{
    public class PlayerRisingState : IState
    {
        private readonly Rigidbody _playerBody;
        private readonly Action[] _onExecute;
        private readonly float _force;

        public PlayerRisingState(Rigidbody rigidbody, float force, params Action[] onExecute)
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
            _onExecute.Foreach(action => action?.Invoke());
        }

        public void Exit()
        {

        }
    } 
}

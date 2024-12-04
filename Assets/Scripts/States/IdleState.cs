using UnityEngine;
using Wave.Actors;

namespace Wave.States.PlayerStates
{
	public class IdleState : IPlayerState
	{
		private Transform _playerTransform;
        private Rigidbody _playerBody;

		public IdleState(Transform playerTransform, Rigidbody rigidbody)
		{
			_playerTransform = playerTransform;
            _playerBody = rigidbody;
		}

        public void Enter()
        {
            _playerBody.isKinematic = true;
            _playerTransform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public void Execute()
        {
            
        }

        public void Exit()
        {
            
        }
    } 
}

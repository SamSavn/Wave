using DG.Tweening;
using UnityEngine;

namespace Wave.States.PlayerStates
{
	public class PlayerIdleState : IState
	{
		private Transform _playerTransform;
        private Rigidbody _playerBody;
        private Tweener _tweener;

        private float _floatingValue = 5f;
        private float _floatingDuration = 1f;

		public PlayerIdleState(Transform playerTransform, Rigidbody rigidbody)
		{
			_playerTransform = playerTransform;
            _playerBody = rigidbody;
		}

        public void Enter()
        {
            _playerBody.isKinematic = true;
            _playerTransform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            _tweener = _playerTransform.DOMoveY(_floatingValue, _floatingDuration)
                                       .SetLoops(-1, LoopType.Yoyo)
                                       .SetEase(Ease.InOutSine);
        }

        public void Execute()
        {
            
        }

        public void Exit()
        {
            _tweener.Kill();
            _tweener = null;

            _playerTransform = null;
            _playerBody = null;
        }
    } 
}

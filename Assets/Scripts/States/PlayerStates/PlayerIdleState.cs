using DG.Tweening;
using System.Collections;
using UnityEngine;
using Wave.Services;

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
            ServiceLocator.Instance.Get<CoroutineService>().StartCoroutine(StartTween());
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

        private IEnumerator StartTween()
        {
            yield return new WaitForEndOfFrame();
            _tweener = _playerTransform.DOMoveY(_floatingValue, _floatingDuration)
                                       .SetLoops(-1, LoopType.Yoyo)
                                       .SetEase(Ease.InOutSine);
        }
    } 
}

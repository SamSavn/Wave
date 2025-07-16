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

        private CoroutineService _coroutineService;

        private Tweener _tweener;
        private Coroutine _coroutine;

        private Vector3 _startPosition;

        private float _floatingValue = 5f;
        private float _floatingDuration = 1f;

		public PlayerIdleState(Transform playerTransform, Rigidbody rigidbody, Vector3 startPosition)
		{
			_playerTransform = playerTransform;
            _playerBody = rigidbody;
            _startPosition = startPosition;

            _coroutineService = ServiceLocator.Instance.Get<CoroutineService>();

        }

        public void Enter()
        {
            _playerBody.isKinematic = true;
            _playerTransform.SetPositionAndRotation(_startPosition, Quaternion.identity);
            _coroutine = _coroutineService.StartCoroutine(StartTween());
        }

        public void Execute()
        {
            
        }

        public void Exit()
        {
            _coroutineService.StopCoroutine(_coroutine);
            _coroutine = null;

            _tweener?.Rewind();
            _tweener?.Kill();
            _tweener = null;

            _playerTransform = null;
            _playerBody = null;
        }

        private IEnumerator StartTween()
        {
            if (_tweener != null && _tweener.IsPlaying())
                yield break;

            yield return new WaitForEndOfFrame();
            _tweener = _playerTransform.DOMoveY(_floatingValue, _floatingDuration)
                                       .SetLoops(-1, LoopType.Yoyo)
                                       .SetEase(Ease.InOutSine);
        }
    } 
}

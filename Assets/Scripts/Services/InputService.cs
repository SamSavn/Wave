using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Wave.Events;
using Wave.Input;

namespace Wave.Services
{
    public class InputService : IService
    {
        private readonly PlayerInputActions _inputActions;
        private readonly CoroutineService _coroutineService;

        public EventDisparcher OnGameInputDown { get; } = new();
        public EventDisparcher OnGameInputUp { get; } = new();

        public InputService(CoroutineService coroutineService)
        {
            _coroutineService = coroutineService;

            _inputActions = new PlayerInputActions();
            _inputActions.Enable();

            _inputActions.GameInput.Action.performed += ctx => ProcessInput(true);
            _inputActions.GameInput.Action.canceled += ctx => ProcessInput(false);
        }

        private void ProcessInput(bool isDown)
        {
            _coroutineService.StartCoroutine(DeferredInputCheck(isDown));
        }

        private IEnumerator DeferredInputCheck(bool isDown)
        {
            yield return null;

            if (IsPointerOverUI())
                yield break;

            if (isDown) OnGameInputDown?.Invoke();
            else OnGameInputUp?.Invoke();
        }

        private bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }
    }
}

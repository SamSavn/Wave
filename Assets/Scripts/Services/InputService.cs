using UnityEngine;
using UnityEngine.InputSystem;
using Wave.Events;
using Wave.Input;

namespace Wave.Services
{
    public class InputService : IService
    {
        private readonly UpdateService _updateService;
        public EventDisparcher OnGameInputDown { get; } = new();
        public EventDisparcher OnGameInputUp { get; } = new();

        private PlayerInputActions inputActions;

        public InputService(UpdateService updateService)
        {
            _updateService = updateService;
            _updateService.Update.Add(Update);

            inputActions = new PlayerInputActions();
            inputActions.Enable();

            inputActions.GameInput.Action.performed += ctx => OnGameInputDown?.Invoke();
            inputActions.GameInput.Action.canceled += ctx => OnGameInputUp?.Invoke();
        }

        private void Update(float dt)
        {
            
        }
    }
}

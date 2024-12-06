using UnityEngine;
using Wave.Events;

namespace Wave.Services
{
    public class InputService : IService
    {
        private readonly UpdateService _updateService;
        public EventDisparcher OnGameInputDown { get; } = new ();
        public EventDisparcher OnGameInputUp { get; } = new ();

        public InputService(UpdateService updateService)
        {
            _updateService = updateService;
            _updateService.Update.Add(Update);
        }

        private bool GameInputDown
        {
            get
            {
#if !UNITY_EDITOR
                return Input.GetTouch(0).phase == TouchPhase.Began;
#else
                return Input.GetKeyDown(KeyCode.Space);
#endif
            }
        }

        private bool GameInputUp
        {
            get
            {
#if !UNITY_EDITOR
                return Input.GetTouch(0).phase is TouchPhase.Ended or TouchPhase.Canceled;
#else
                return Input.GetKeyUp(KeyCode.Space);
#endif
            }
        }

        private void Update(float dt)
        {
            if (GameInputDown) OnGameInputDown?.Invoke();
            else if (GameInputUp) OnGameInputUp?.Invoke();
        }
    }
}

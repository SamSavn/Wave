using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wave.Services;
using Wave.UI;

namespace Wave.Input
{
    public class UiInputMediator
    {
        private readonly Dictionary<object, Action> _buttonsRegister = new ();
        private readonly Dictionary<object, UnityAction> _buttonsValidations = new ();

        private readonly CoroutineService _coroutineService;
        private readonly WaitForSeconds _waitForSeconds;

        private const float WAIT_TIME = .5f;

        public bool IsWaiting { get; private set; } = false;

        public UiInputMediator(CoroutineService coroutineService)
        {
            _coroutineService = coroutineService;
            _waitForSeconds = new WaitForSeconds(WAIT_TIME);
        }

        public void RegisterButton(TextButton button, Action onClick)
        {
            RegisterButton(button.Button, onClick);
        }

        public void RegisterButton(Button button, Action onClick)
        {
            if (button == null)
            {
                Debug.LogError("Button cannot be null");
                return;
            }

            if (onClick == null)
            {
                Debug.LogWarning("Action cannot be null");
                return;
            }

            _buttonsRegister[button] = onClick;
            _buttonsValidations[button] = () => ValidateInput(button);
            button.onClick.AddListener(_buttonsValidations[button]);
        }

        public void UnregisterButton(TextButton button)
        {
            UnregisterButton(button.Button);
        }

        public void UnregisterButton(Button button)
        {
            if (button == null)
            {
                Debug.LogError("Button cannot be null");
                return;
            }

            if (!_buttonsRegister.ContainsKey(button))
            {
                Debug.LogWarning("Button is not registered");
                return;
            }

            if (_buttonsValidations.TryGetValue(button, out UnityAction action))
            {
                button.onClick.RemoveListener(action);
                _buttonsValidations.Remove(button);
            }
            
            _buttonsRegister.Remove(button);
        }

        private void ValidateInput(object button)
        {
            if (IsWaiting)
            {
                Debug.LogWarning("Input is currently waiting, ignoring button press.");
                return;
            }

            if (!_buttonsRegister.TryGetValue(button, out Action action))
                return;

            action?.Invoke();
            _coroutineService.StartCoroutine(StartWaiting());
        }

        private IEnumerator StartWaiting()
        {
            IsWaiting = true;
            yield return _waitForSeconds;
            IsWaiting = false;
        }
    }
}
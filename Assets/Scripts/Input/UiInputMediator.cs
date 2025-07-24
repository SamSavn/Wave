using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wave.Services;

namespace Wave.Input
{
    public class UiInputMediator
    {
        private readonly Dictionary<Button, Action> _buttonsRegister = new ();
        private readonly CoroutineService _coroutineService;
        private readonly WaitForSeconds _waitForSeconds;

        private const float WAIT_TIME = .5f;

        public bool IsWaiting { get; private set; } = false;

        public UiInputMediator(CoroutineService coroutineService)
        {
            _coroutineService = coroutineService;
            _waitForSeconds = new WaitForSeconds(WAIT_TIME);
        }

        public void RegisterButton(Button button, Action onClick)
        {
            if (button == null || onClick == null)
            {
                Debug.LogError("Button or onClick action cannot be null.");
                return;
            }

            if (_buttonsRegister.ContainsKey(button))
            {
                Debug.LogWarning("Button is already registered.");
                return;
            }

            _buttonsRegister[button] = onClick;
            button.onClick.AddListener(() => ValidateInput(button));
        }

        public void UnregisterButton(Button button)
        {
            if (button == null)
            {
                Debug.LogError("Button cannot be null.");
                return;
            }

            if (!_buttonsRegister.ContainsKey(button))
            {
                Debug.LogWarning("Button is not registered.");
                return;
            }

            button.onClick.RemoveListener(() => ValidateInput(button));
            _buttonsRegister.Remove(button);
        }

        private void ValidateInput(Button button)
        {
            if (IsWaiting)
            {
                Debug.LogWarning("Input is currently waiting, ignoring button press.");
                return;
            }

            if (!_buttonsRegister.TryGetValue(button, out var action))
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
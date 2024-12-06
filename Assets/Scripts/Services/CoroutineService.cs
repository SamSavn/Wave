using System.Collections;
using UnityEngine;

namespace Wave.Services
{
	public class CoroutineService : IService
	{
        private class CoroutineRunner : MonoBehaviour { }

        private static CoroutineRunner _runner;

        public CoroutineService()
        {
            GameObject runner = new GameObject("[Coroutine Runner]");
            _runner = runner.AddComponent<CoroutineRunner>();
            GameObject.DontDestroyOnLoad(runner);
        }

        public Coroutine StartCoroutine(IEnumerator enumerator) => _runner.StartCoroutine(enumerator);
        public void StopCoroutine(Coroutine coroutine) => _runner.StopCoroutine(coroutine);
        public void StopAllCoroutines() => _runner.StopAllCoroutines();
    } 
}

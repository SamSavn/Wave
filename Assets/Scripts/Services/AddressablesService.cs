using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Wave.Services
{
	public class AddressablesService : IService
	{
        private CoroutineService _routineService;

        public AddressablesService(CoroutineService routineService)
        {
            _routineService = routineService;
        }

        public void LoadSingle<T>(string key, Action<T, bool> onLoaded)
        {
            _routineService.StartCoroutine(LoadAddressableAsync(key, onLoaded));
        }
        public void LoadAll<T>(string key, Action<List<T>> onLoaded)
        {
            _routineService.StartCoroutine(LoadAddressablesAsync(key, onLoaded));
        }

        private IEnumerator LoadAddressableAsync<T>(string key, Action<T, bool> callback)
        {
            AsyncOperationHandle<T> opHandle = Addressables.LoadAssetAsync<T>(key);
            yield return opHandle;

            if (opHandle.Status == AsyncOperationStatus.Succeeded)
            {
                T obj = opHandle.Result;
                callback?.Invoke(obj, true);
            }
            else
            {
                callback?.Invoke(default, false);
            }
        }

        private IEnumerator LoadAddressablesAsync<T>(string key, Action<List<T>> callback)
        {
            List<T> _results = new List<T>();
            AsyncOperationHandle<IList<T>> loadHandle = Addressables.LoadAssetsAsync<T>(key, (data) =>
            {
                _results.Add(data);
            });

            yield return loadHandle;

            callback?.Invoke(_results);
        }
    } 
}

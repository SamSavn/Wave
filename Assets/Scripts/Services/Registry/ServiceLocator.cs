using System;
using System.Collections.Generic;

namespace Wave.Services
{
    public class ServiceLocator
    {
        private Dictionary<Type, IService> _registry = new Dictionary<Type, IService>();
        private static ServiceLocator _instance;

        public bool IsReady { get; private set; }

        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ServiceLocator();

                return _instance;
            }
        }

        public ServiceLocator()
        {
            ClearAllServices();
            IsReady = false;
        }

        public void Register<T>(T instance) where T : IService
        {
            if (_registry.ContainsKey(typeof(T)))
                UnityEngine.Debug.LogWarning($"<color=green>ServiceLocator</color>: type <b>{typeof(T)}</b> already registered");

            _registry[typeof(T)] = instance;
        }


        public T Get<T>() where T : IService
        {
            if (!_registry.TryGetValue(typeof(T), out IService service))
                UnityEngine.Debug.LogError($"<color=green>ServiceLocator</color>: service <b>{typeof(T)}</b> has not been registered!");

            if (service is not T)
                UnityEngine.Debug.LogError($"<color=green>ServiceLocator</color>: service <b>{typeof(T)}</b> is null or has wrong type");

            return (T)service;
        }

        public bool TryRemoveService<T>() => _registry.Remove(typeof(T));
        public void ClearAllServices() => _registry.Clear();
        public void SetAsReady() => IsReady = true;
    } 
}

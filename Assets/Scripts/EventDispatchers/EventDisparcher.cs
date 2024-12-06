using System;

namespace Wave.Events
{
    public class EventDisparcher : IDisposable
    {
        private event Action _event;

        public void Add(Action action)
        {
            Remove(action);
            _event += action;
        }

        public void Remove(Action action) => _event -= action;
        public void Invoke() => _event?.Invoke();
        public void Dispose() => _event = null;
    }

    public class EventDisparcher<T> : IDisposable
    {
        private event Action<T> _event;

        public void Add(Action<T> action)
        {
            Remove(action);
            _event += action;
        }

        public void Remove(Action<T> action) => _event -= action;
        public void Invoke(T arg) => _event?.Invoke(arg);
        public void Dispose() => _event = null;
    } 
}

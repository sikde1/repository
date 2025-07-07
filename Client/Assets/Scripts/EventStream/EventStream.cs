using System;
using System.Collections.Generic;

namespace Project.EventStream
{
    /// <summary>
    /// 매개변수 없는 이벤트를 위한 EventStream
    /// 타입별 분리 저장으로 런타임 타입 체크 없이 빠른 성능 제공
    /// </summary>
    public class SimpleEventStream
    {
        private readonly List<Action> _listeners = new();
        private readonly List<Action> _pendingRemove = new();
        private bool _isDispatching = false;
        private bool _pendingClear = false;

        public int ListenerCount => _listeners.Count;
        public bool HasListeners => _listeners.Count > 0;

        public EventStreamSubscription Subscribe(Action listener)
        {
            if (listener == null) return null;

            _listeners.Add(listener);
            return new EventStreamSubscription(this, listener);
        }

        public void Unsubscribe(Action listener)
        {
            if (listener == null) return;

            if (_isDispatching)
                _pendingRemove.Add(listener);
            else
                _listeners.Remove(listener);
        }

        public void Clear()
        {
            if (_isDispatching)
                _pendingClear = true;
            else
                _listeners.Clear();
        }

        public void Dispatch()
        {
            _isDispatching = true;

            var currentListeners = new List<Action>(_listeners);

            foreach (var listener in currentListeners)
            {
                try
                {
                    listener.Invoke();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"EventStream 리스너 실행 중 예외 발생: {ex.Message}");
                }
            }

            _isDispatching = false;
            ProcessPendingOperations();
        }

        private void ProcessPendingOperations()
        {
            if (_pendingClear)
            {
                _listeners.Clear();
                _pendingClear = false;
            }
            else
            {
                foreach (var listener in _pendingRemove)
                    _listeners.Remove(listener);
            }

            _pendingRemove.Clear();
        }
    }

    /// <summary>
    /// 매개변수 있는 이벤트를 위한 제네릭 EventStream
    /// 완전히 독립적인 타입별 리스너 관리
    /// </summary>
    public class EventStream<T>
    {
        private readonly List<Action<T>> _listeners = new();
        private readonly List<Action<T>> _pendingRemove = new();
        private bool _isDispatching = false;
        private bool _pendingClear = false;

        public int ListenerCount => _listeners.Count;
        public bool HasListeners => _listeners.Count > 0;

        public EventStreamSubscription<T> Subscribe(Action<T> listener)
        {
            if (listener == null) return null;

            _listeners.Add(listener);
            return new EventStreamSubscription<T>(this, listener);
        }

        public void Unsubscribe(Action<T> listener)
        {
            if (listener == null) return;

            if (_isDispatching)
                _pendingRemove.Add(listener);
            else
                _listeners.Remove(listener);
        }

        public void Clear()
        {
            if (_isDispatching)
                _pendingClear = true;
            else
                _listeners.Clear();
        }

        public void Dispatch(T value)
        {
            _isDispatching = true;

            var currentListeners = new List<Action<T>>(_listeners);

            foreach (var listener in currentListeners)
            {
                try
                {
                    listener.Invoke(value);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"EventStream<{typeof(T).Name}> 리스너 실행 중 예외 발생: {ex.Message}");
                }
            }

            _isDispatching = false;
            ProcessPendingOperations();
        }

        private void ProcessPendingOperations()
        {
            if (_pendingClear)
            {
                _listeners.Clear();
                _pendingClear = false;
            }
            else
            {
                foreach (var listener in _pendingRemove)
                    _listeners.Remove(listener);
            }

            _pendingRemove.Clear();
        }
    }
}


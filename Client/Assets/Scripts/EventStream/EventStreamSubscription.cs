using System;

namespace Project.EventStream
{
    /// <summary>
    /// 기본 EventStream의 구독 관리
    /// 매개변수 없는 이벤트용
    /// </summary>
    public class EventStreamSubscription : IDisposable
    {
        private SimpleEventStream _eventStream;
        private Action _listener;
        private bool _disposed = false;
        public bool IsDisposed => _disposed;

        public EventStreamSubscription(SimpleEventStream eventStream, Action listener)
        {
            _eventStream = eventStream;
            _listener = listener;
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                _eventStream?.Unsubscribe(_listener);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"EventStreamSubscription Dispose 중 예외 발생: {ex.Message}");
            }
            finally
            {
                _eventStream = null;
                _listener = null;
                _disposed = true;
            }
        }
        public EventStreamSubscription AddTo(IEventStreamDisposable disposable)
        {
            disposable.Add(this);
            return this;
        }
    }

    /// <summary>
    /// 제네릭 EventStream의 구독 관리
    /// 매개변수 있는 이벤트용
    /// </summary>
    public class EventStreamSubscription<T> : IDisposable
    {
        private EventStream<T> _eventStream;
        private Action<T> _listener;
        private bool _disposed = false;
        public bool IsDisposed => _disposed;

        public EventStreamSubscription(EventStream<T> eventStream, Action<T> listener)
        {
            _eventStream = eventStream;
            _listener = listener;
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                _eventStream?.Unsubscribe(_listener);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"EventStreamSubscription<{typeof(T).Name}> Dispose 중 예외 발생: {ex.Message}");
            }
            finally
            {
                _eventStream = null;
                _listener = null;
                _disposed = true;
            }
        }

        public EventStreamSubscription<T> AddTo(IEventStreamDisposable disposable)
        {
            disposable.Add(this);
            return this;
        }
    }
}

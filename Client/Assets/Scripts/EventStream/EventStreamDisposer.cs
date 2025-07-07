using System;
using System.Collections.Generic;

namespace Project.EventStream
{
    /// <summary>
    /// EventStream 구독들을 일괄 관리하는 Disposer
    /// 비제네릭과 제네릭 구독 모두 지원
    /// </summary>
    public class EventStreamDisposer : IDisposable
    {
        private readonly List<IDisposable> _subscriptions = new();
        private bool _disposed = false;

        /// <summary>
        /// IDisposable 구독 추가 (범용)
        /// </summary>
        public void Add(IDisposable subscription)
        {
            if (_disposed || subscription == null) return;
            _subscriptions.Add(subscription);
        }

        /// <summary>
        /// 모든 구독 해제
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            foreach (var subscription in _subscriptions)
            {
                try
                {
                    subscription?.Dispose();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"EventStreamDisposer에서 구독 해제 중 예외 발생: {ex.Message}");
                }
            }

            _subscriptions.Clear();
            _disposed = true;
        }

        public int SubscriptionCount => _subscriptions.Count;
        public bool IsDisposed => _disposed;
    }

    /// <summary>
    /// EventStream 구독을 쉽게 관리하기 위한 확장 메서드들
    /// </summary>
    public static class EventStreamDisposerExtensions
    {
        /// <summary>
        /// 비제네릭 EventStreamSubscription을 Disposer에 추가
        /// </summary>
        public static void AddTo(this EventStreamSubscription subscription, EventStreamDisposer disposer)
        {
            disposer?.Add(subscription);
        }

        /// <summary>
        /// 제네릭 EventStreamSubscription을 Disposer에 추가
        /// </summary>
        public static void AddTo<T>(this EventStreamSubscription<T> subscription, EventStreamDisposer disposer)
        {
            disposer?.Add(subscription);
        }
    }
}

using Cysharp.Threading.Tasks;
using Project.EventStream;
using Project.UI.Interface;
using Project.UI.Type;
using System;
using UnityEngine;

namespace Project.Presenter
{
    public abstract class PresenterBase : IPresenter, IDisposable
    {
        protected EventStreamDisposer _disposer = new EventStreamDisposer();
        protected bool _disposed = false;

        // 상속받은 클래스에서 반드시 설정해야 하는 자신의 PresenterType
        protected abstract PresenterType MyPresenterType { get; }

        /// <summary>
        /// 1. PresenterManager.ClosePresenter() 호출
        /// 2. ClosePopup() 자동 호출
        /// 3. Dispose() 자동 호출  
        /// 4. Dictionary에서 자동 제거
        /// </summary>
        protected void RequestClose()
        {
            if (_disposed) return;

            try
            {
                // PresenterManager에게 자신을 정리하도록 요청
                // 이렇게 하면 ClosePopup() -> Dispose() -> Dictionary에서 제거가 모두 자동으로 처리됨
                MainSystem.Instance.PresenterManager.ClosePresenter(MyPresenterType);
            }
            catch (Exception ex)
            {
                Debug.LogError($"RequestClose 실패 [{MyPresenterType}]: {ex.Message}");
                
                // 실패 시 직접 정리 시도
                try
                {
                    ClosePopup();
                    Dispose();
                }
                catch (Exception disposeEx)
                {
                    Debug.LogError($"직접 정리도 실패 [{MyPresenterType}]: {disposeEx.Message}");
                }
            }
        }

        // IPresenter 구현 - 상속받은 클래스에서 구현
        public abstract UniTask OpenPopupAsync();
        public abstract void ClosePopup();
        public virtual void Refresh() { }

        // IDisposable 구현
        public virtual void Dispose()
        {
            if (_disposed) return;

            // 이벤트 구독 해제
            _disposer?.Dispose();

            _disposed = true;
        }
    }
}

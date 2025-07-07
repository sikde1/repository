using Cysharp.Threading.Tasks;
using Project.UI.Factory;
using Project.UI.Interface;
using Project.UI.Type;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI.Manager
{
    public class PresenterManager : IDisposable
    {
        private readonly Dictionary<PresenterType, IPresenter> _activePresenters = new();
        private readonly PresenterFactory _presenterFactory;
        private bool _disposed = false;

        public PresenterManager(PresenterFactory presenterFactory = null)
        {
            _presenterFactory = presenterFactory ?? new PresenterFactory();
        }

        public async UniTask<bool> OpenPresenterAsync(PresenterType presenterType)
        {
            if (_disposed)
            {
                Debug.LogWarning("PresenterManager가 이미 Dispose되었습니다.");
                return false;
            }

            // 이미 열려있는 경우
            if (IsPresenterOpen(presenterType))
            {
                Debug.LogWarning($"{presenterType} Presenter가 이미 열려있습니다.");
                return true;
            }

            // 생성 가능한지 확인
            if (!_presenterFactory.CanCreate(presenterType))
            {
                Debug.LogError($"{presenterType} Presenter는 아직 구현되지 않았습니다.");
                return false;
            }

            try
            {
                var presenter = await _presenterFactory.CreatePresenterAsync(presenterType);
                
                if (presenter != null)
                {
                    _activePresenters[presenterType] = presenter;
                    return true;
                }
                else
                {
                    Debug.LogError($"{presenterType} Presenter 생성 실패");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"{presenterType} Presenter 생성 중 오류: {ex.Message}");
                return false;
            }
        }

        public void ClosePresenter(PresenterType presenterType)
        {
            if (_activePresenters.TryGetValue(presenterType, out var presenter))
            {
                try
                {
                    presenter.ClosePopup();
                    presenter.Dispose();
                    _activePresenters.Remove(presenterType);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"{presenterType} Presenter 닫기 중 오류: {ex.Message}");
                }
            }
        }

        public void CloseAllPresenters()
        {
            var presenterTypes = new List<PresenterType>(_activePresenters.Keys);
            foreach (var presenterType in presenterTypes)
            {
                ClosePresenter(presenterType);
            }
        }

        public bool IsPresenterOpen(PresenterType presenterType)
        {
            return _activePresenters.ContainsKey(presenterType);
        }

        public T GetPresenter<T>(PresenterType presenterType) where T : class, IPresenter
        {
            if (_activePresenters.TryGetValue(presenterType, out var presenter))
            {
                return presenter as T;
            }
            return null;
        }

        public void Dispose()
        {
            if (_disposed) return;

            CloseAllPresenters();
            _activePresenters.Clear();
            _disposed = true;
        }
    }
} 
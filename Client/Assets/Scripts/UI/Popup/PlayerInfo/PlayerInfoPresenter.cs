using Cysharp.Threading.Tasks;
using Project.Data;
using Project.EventStream;
using Project.Presenter;
using Project.UI.Type;
using System;
using UnityEngine;

namespace Project.PlayerInfo
{
    public class PlayerInfoPresenter : PresenterBase
    {
        private PlayerInfoView _view;

        protected override PresenterType MyPresenterType => PresenterType.PlayerInfo;

        // 생성과 동시에 팝업 열기
        public static async UniTask<PlayerInfoPresenter> CreateAndOpenAsync()
        {
            var presenter = new PlayerInfoPresenter();
            await presenter.OpenPopupAsync();
            return presenter;
        }

        public override async UniTask OpenPopupAsync()
        {
            if (_disposed)
                return;

            try
            {
                // 1. UIViewType으로 팝업 열기 (AddressableResourceManager에서 자동 처리)
                _view = await MainSystem.Instance.AddressableResourceManager
                    .OpenPopupAsync<PlayerInfoView>(UIViewType.PlayerInfo);

                if (_view == null)
                {
                    Debug.LogError("PlayerInfoView 생성 실패");
                    return;
                }

                // 2. Repository Pattern으로 데이터 가져오기
                var dataUIPopup = MainSystem.Instance.GameDataRepository.GetData<DataUIPopup>(DataType.UIPopup);
                if (dataUIPopup?.UIPopups.TryGetValue(UIViewType.PlayerInfo, out var popupData) == true)
                {
                    _view.Initialize(popupData);
                }
                else
                {
                    Debug.LogWarning("Repository에서 PlayerInfo 데이터를 찾지 못했습니다. 기본값으로 초기화합니다.");
                    _view.Initialize(null);
                }

                // 3. 이벤트 구독
                SubscribeViewEvents();
            }
            catch (Exception ex)
            {
                Debug.LogError($"PlayerInfoPresenter 열기 실패: {ex.Message}");
            }
        }

        private void SubscribeViewEvents()
        {
            if (_view == null) return;

            _view.OnCloseClicked.Subscribe(OnCloseClicked).AddTo(_disposer);
            _view.OnRefreshClicked.Subscribe(OnRefreshClicked).AddTo(_disposer);
            
        }

        private void OnCloseClicked()
        {
            RequestClose();
        }

        private void OnRefreshClicked()
        {
            Refresh();
        }

        public override void ClosePopup()
        {
            if (_view != null)
            {
                _view.ClosePopup();
                _view = null;
            }
        }

        public override void Refresh()
        {
            if (_view == null) return;

            var dataUIPopup = MainSystem.Instance.GameDataRepository.GetData<DataUIPopup>(DataType.UIPopup);
            if (dataUIPopup?.UIPopups.TryGetValue(UIViewType.PlayerInfo, out var popupData) == true)
            {
                _view.UpdateData(popupData);
            }
            else
            {
                Debug.LogWarning("Repository에서 새로고침할 PlayerInfo 데이터를 찾지 못했습니다.");
            }
        }

        public override void Dispose()
        {
            ClosePopup();
            base.Dispose();
        }
    }
}


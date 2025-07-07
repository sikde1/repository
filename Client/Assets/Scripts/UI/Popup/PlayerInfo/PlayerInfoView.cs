using FlatBufferData;
using Project.EventStream;
using Project.UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace Project.PlayerInfo
{
    public class PlayerInfoView : PopupBase
    {
        [SerializeField] private Button _exit;
        [SerializeField] private Button _refreshButton;

        public SimpleEventStream OnCloseClicked = new SimpleEventStream();
        public SimpleEventStream OnRefreshClicked = new SimpleEventStream();

        private FBUIPopup? _popupData;

        private void Start()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            _exit?.onClick.AddListener(() => {
                OnCloseClicked.Dispatch();
            });
            
            _refreshButton?.onClick.AddListener(() => {
                OnRefreshClicked.Dispatch();
            });
        }

        public void Initialize(FBUIPopup? popupData)
        {
            _popupData = popupData;

            if (_popupData.HasValue)
            {
                SetupUI();
            }
            else
            {
                Debug.LogWarning("PlayerInfoView를 기본값으로 초기화합니다.");
                SetupDefaultUI();
            }
        }

        public void UpdateData(FBUIPopup popupData)
        {
            _popupData = popupData;
            SetupUI();
        }

        private void SetupUI()
        {
            // FBUIPopup 데이터 기반 UI 설정
            // 예: 타이틀, 설명, 버튼 텍스트 등
        }

        private void SetupDefaultUI()
        {
            // 기본 UI 설정
        }

        public void ClosePopup()
        {
            Close();
        }

        protected virtual void OnDestroy()
        {
            OnCloseClicked?.Clear();
            OnRefreshClicked?.Clear();
        }
    }
}

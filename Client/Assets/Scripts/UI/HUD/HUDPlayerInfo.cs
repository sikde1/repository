using Cysharp.Threading.Tasks;
using Project.UI.Type;
using UnityEngine;
using UnityEngine.UI;

namespace Project.HUD
{
    public class HUDPlayerInfo : MonoBehaviour
    {
        [SerializeField] private Button _popupOpen;

        private void Start()
        {
            _popupOpen.onClick.AddListener(OpenPopup);
        }

        private void OpenPopup()
        {
            // 이미 열려있으면 닫기, 없으면 열기 (토글 방식)
            if (MainSystem.Instance.PresenterManager.IsPresenterOpen(PresenterType.PlayerInfo))
            {
                MainSystem.Instance.PresenterManager.ClosePresenter(PresenterType.PlayerInfo);
            }
            else
            {
                // 새 팝업 열기
                OpenPopupAsync().Forget();
            }
        }

        private async UniTaskVoid OpenPopupAsync()
        {
            try
            {
                bool success = await MainSystem.Instance.PresenterManager.OpenPresenterAsync(PresenterType.PlayerInfo);
                if (!success)
                {
                    Debug.LogError("PlayerInfo 팝업 열기 실패");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"팝업 열기 실패: {ex.Message}");
            }
        }

        private void OnDestroy()
        {
            // PresenterManager가 알아서 정리하므로 별도 처리 불필요
        }
    }
}

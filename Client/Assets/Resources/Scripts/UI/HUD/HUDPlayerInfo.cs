using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HUDPlayerInfo : MonoBehaviour
{
    [SerializeField] private Button _popupOpen;
    private void Start()
    {
        _popupOpen.onClick.AddListener(OpenPopup);
    }
    private void OpenPopup()
    {
        MainSystem.Instance.AddressableResourceManager.OpenPopupAsync<PopupPlayerInfo>().Forget();
    }
}

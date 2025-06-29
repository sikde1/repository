using Project.UI.Popup;
using UnityEngine;
using UnityEngine.UI;

public class PopupPlayerInfo : PopupBase
{
    [SerializeField] private Button _exit;

    private void Start()
    {
        _exit.onClick.AddListener(Close);
    }
}

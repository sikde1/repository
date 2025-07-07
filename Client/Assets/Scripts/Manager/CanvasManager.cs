using UnityEngine;

namespace Project.Managers
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Instance { get; private set; }
        [SerializeField] private Transform _canvasUI;
        [SerializeField] private Transform _canvasPopup;
        [SerializeField] private Transform _canvasTop;
        private void Awake()
        {
            Instance = this;
        }
        public Transform GetPopupTransform()
        {
            return _canvasPopup;
        }
    }
}


using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Project.UI.Popup
{
    public class PopupBase : MonoBehaviour
    {
        private AsyncOperationHandle<GameObject>? _handle;
        public void SetHandle(AsyncOperationHandle<GameObject> handle)
        {
            _handle = handle;
        }
        protected virtual void Close()
        {
            Destroy(gameObject);
            if (_handle.HasValue)
                Addressables.Release(_handle.Value);
        }
    }
}


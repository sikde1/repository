using Cysharp.Threading.Tasks;
using Project.UI.Popup;
using Project.UI.Type;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Project.Managers
{
    public class AddressableResourceManager
    {
        // 캐싱된 데이터
        private Dictionary<string, Object> _cache = new Dictionary<string, Object>();

        public async UniTask<T> LoadAsync<T>(string address) where T : Object
        {
            if (_cache.TryGetValue(address, out var cachedObj))
            {
                return cachedObj as T;
            }

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _cache[address] = handle.Result;
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load addressable asset: {address}");
                return null;
            }
        }

        // UIViewType 기반으로 팝업 열기 (새로운 메서드)
        public async UniTask<T> OpenPopupAsync<T>(UIViewType viewType) where T : PopupBase
        {
            // UIViewType 확장 메서드로 PrefabPath 조회
            var prefabPath = viewType.GetPrefabPath();
            if (string.IsNullOrEmpty(prefabPath))
            {
                Debug.LogError($"UIViewType {viewType}에 해당하는 PrefabPath를 찾을 수 없습니다.");
                return null;
            }

            return await OpenPopupByAddressAsync<T>(prefabPath);
        }

        // 주소로 직접 팝업 열기 (기존 로직을 메서드명 변경)
        public async UniTask<T> OpenPopupByAddressAsync<T>(string address) where T : PopupBase
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(address);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject go = Object.Instantiate(handle.Result, CanvasManager.Instance.GetPopupTransform());
                T popup = go.GetComponent<T>();

                if (popup == null)
                    Debug.LogError($"Popup is null {typeof(T).Name}");

                popup.SetHandle(handle);
                return popup;
            }
            else
            {
                Debug.LogError($"Failed to load popup: {address}");
                return null;
            }
        }

        public void Release(Object obj)
        {
            if (obj == null) return;

            Addressables.Release(obj);

            // 캐시에서 제거 (주소를 알 수 없으므로 객체 매칭)
            foreach (var pair in _cache)
            {
                if (pair.Value == obj)
                {
                    _cache.Remove(pair.Key);
                    break;
                }
            }
        }
    }
}


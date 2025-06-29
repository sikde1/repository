using Cysharp.Threading.Tasks;
using Project.UI.Popup;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Project.Managers
{
    public class AddressableResourceManager
    {
        // 캐시된 에셋
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
        public async UniTask<T> OpenPopupAsync<T>() where T : PopupBase
        {
            string name = typeof(T).Name;
            string prefabName = ConvertClassNameToPrefabName(name);
            string address = $"Assets/Addressables/Prefabs/UI/Popup/{prefabName}.prefab";

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
        private string ConvertClassNameToPrefabName(string className)
        {
            if (className.StartsWith("Popup"))
            {
                string remaining = className.Substring(5); // CamelCase를 Snake_Case로 변환
                string snakeCase = System.Text.RegularExpressions.Regex.Replace(
                    remaining, "([a-z])([A-Z])", "$1_$2");
                return $"Popup_{snakeCase}";
            }
            return className;
        }


        public void Release(Object obj)
        {
            if (obj == null) return;

            Addressables.Release(obj);

            // 캐시에서 제거 (주소를 모를 수 있으니 전체 스캔)
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


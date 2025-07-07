using UnityEngine;

namespace Project.MainScene
{
    public class MainScene : MonoBehaviour
    {
        private static MainScene _instance;
        public static MainScene Instance
        {
            get
            {
                // 씬에서 MainScene 컴포넌트 찾기
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<MainScene>();
                    
                    // 씬에 없으면 새로 생성
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("MainScene");
                        _instance = singletonObject.AddComponent<MainScene>();
                        
                        // 씬 전환 시에도 유지 (필요한 경우)
                        // DontDestroyOnLoad(singletonObject);
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            // 이미 인스턴스가 있으면 현재 오브젝트 제거
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            
            // 씬 전환 시에도 유지하려면 주석 해제
            // DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}


using Project.Data;
using Project.Managers;
using Project.UI.Manager;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    public static MainSystem Instance { get; private set; }

    public AddressableResourceManager AddressableResourceManager { get; private set; }
    public GameDataRepository GameDataRepository { get; private set; }
    public PresenterManager PresenterManager { get; private set; }

    private void Awake()
    {
        Instance = this;

        GameDataRepository = new GameDataRepository();
        GameDataRepository.Initialize();

        AddressableResourceManager = new AddressableResourceManager();
        
        // PresenterManager 초기화
        PresenterManager = new PresenterManager();
    }

    private void OnDestroy()
    {
        // 모든 리소스 정리 (순서 중요!)
        PresenterManager?.Dispose();
        GameDataRepository?.Dispose();
    }
}


using Project.FlatBuffer;
using Project.Managers;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    public static MainSystem Instance { get; private set; }

    public AddressableResourceManager AddressableResourceManager { get; private set; }
    public FlatBufferReader FlatBufferReader { get; private set; }

    private void Awake()
    {
        Instance = this;

        FlatBufferReader = new FlatBufferReader();
        FlatBufferReader.Initialize();

        AddressableResourceManager = new AddressableResourceManager();
    }
}

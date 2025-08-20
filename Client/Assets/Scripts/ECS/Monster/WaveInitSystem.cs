using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct WaveInitSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        if (!SystemAPI.HasSingleton<WaveData>())
        {
            var e = state.EntityManager.CreateEntity(typeof(WaveData));
            state.EntityManager.SetComponentData(e, new WaveData { CurrentMonsterIdx = 1 });
        }
    }

    public void OnUpdate(ref SystemState state) { }
    public void OnDestroy(ref SystemState state) { }
}



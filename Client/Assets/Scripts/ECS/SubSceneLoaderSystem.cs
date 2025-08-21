using Unity.Burst;
using Unity.Entities;
using Unity.Scenes;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SubSceneLoaderSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SubSceneLoadConfig>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<SubSceneLoadConfig>(out var cfgEntity))
            return;

        var cfg = SystemAPI.GetComponent<SubSceneLoadConfig>(cfgEntity);
        if (!cfg.AutoLoad || !cfg.SceneGUID.IsValid)
            return;


        var sceneEntity = SceneSystem.LoadSceneAsync(state.WorldUnmanaged, cfg.SceneGUID);

        var tagEntity = state.EntityManager.CreateEntity();
        
        state.Enabled = false;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
}

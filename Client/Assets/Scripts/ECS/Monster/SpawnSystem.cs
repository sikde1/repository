using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static UnityEngine.EventSystems.EventTrigger;

[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct SpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ConfigData>();
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var configEntity = SystemAPI.GetSingletonEntity<ConfigData>();
        var config = SystemAPI.GetComponentRW<ConfigData>(configEntity);

        var currentTime = (float)SystemAPI.Time.ElapsedTime;

        // 스폰 간격 체크
        if (currentTime - config.ValueRO.LastSpawnTime >= config.ValueRO.SpawnInterval)
        {
            var monsterCountData = SystemAPI.GetSingleton<MonsterCountData>();
            
            if (monsterCountData.CurrentCount < config.ValueRO.MaxMonsterCount)
                SpawnMonsters(ref state, config.ValueRO, currentTime);

            // 마지막 스폰 시간 업데이트
            config.ValueRW.LastSpawnTime = currentTime;
        }

    }
    [BurstCompile]
    private void SpawnMonsters(ref SystemState state, ConfigData config, float currentTime)
    {
        var entity = state.EntityManager.Instantiate(config.Prefab);
        var xform = SystemAPI.GetComponentRW<LocalTransform>(entity);
        xform.ValueRW = LocalTransform.FromPosition(config.SpawnPosition);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

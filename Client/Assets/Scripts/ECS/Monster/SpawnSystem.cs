using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateAfter(typeof(WaveInitSystem))]
[UpdateAfter(typeof(MonsterTableInitSystem))]
partial struct SpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ConfigData>();
        state.RequireForUpdate<WaveData>();
        state.RequireForUpdate<MonsterTable>();
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

        // 현재 웨이브 idx로 Blob에서 Speed 조회
        var wave = SystemAPI.GetSingleton<WaveData>();
        int idx = wave.CurrentMonsterIdx;

        var table = SystemAPI.GetSingleton<MonsterTable>();
        if (!table.Blob.IsCreated)
            return;

        ref var ids = ref table.Blob.Value.Ids;
        ref var speeds = ref table.Blob.Value.Speeds;

        float speed = 1f;
        for (int i = 0; i < ids.Length; i++)
        {
            if (ids[i] == idx)
            {
                speed = speeds[i];
                break;
            }
        }

        var md = state.EntityManager.GetComponentData<MonsterData>(entity);
        md.Speed = speed;
        state.EntityManager.SetComponentData(entity, md);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
partial struct MonsterCountSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        var entity = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponentData(entity, new MonsterCountData { CurrentCount = 0 });
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var query = SystemAPI.QueryBuilder()
        .WithAll<MonsterData>()
        .Build();

        var monsterCount = query.CalculateEntityCount();

        SystemAPI.SetSingleton(new MonsterCountData { CurrentCount = monsterCount });
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
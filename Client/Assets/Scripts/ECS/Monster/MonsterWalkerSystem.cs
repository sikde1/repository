using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct MonsterWalkerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<ConfigData>();

        var job = new MonsterWalkerJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            MovePoints = config.MovePoints
        };

        job.ScheduleParallel();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

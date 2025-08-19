using Unity.Entities;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[UpdateAfter(typeof(MonsterCountSystem))]
partial struct MonsterCountUISystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MonsterCountData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var monsterCountData = SystemAPI.GetSingleton<MonsterCountData>();

        if (HUDMonsterCount.Instance != null)
        {
            HUDMonsterCount.Instance.UpdateMonsterCount(monsterCountData.CurrentCount);
        }
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct ConfigData : IComponentData
{
    public Entity Prefab;
    public float3 SpawnPosition;
    public float SpawnInterval;
    public float LastSpawnTime;
    public int MaxMonsterCount;
    public FixedList128Bytes<float3> MovePoints;
}

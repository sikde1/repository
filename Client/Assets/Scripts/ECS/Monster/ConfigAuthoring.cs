using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class ConfigAuthoring : MonoBehaviour
{
    public GameObject Prefab = null;
    public Vector3 SpawnPosition = Vector3.zero;
    public float SpawnInterval = 5f;
    [Header("Spawn Limits")]
    public int MaxMonsterCount = 5000;
    public Vector3[] MovePoints = new Vector3[]
    {
        new Vector3(-7, 0, 7),
        new Vector3(-7, 0, -7),
        new Vector3(7, 0, -7),
        new Vector3(7, 0, 7)
    };
}

class ConfigAuthoringBaker : Baker<ConfigAuthoring>
{
    public override void Bake(ConfigAuthoring authoring)
    {
        var spawnPoints = new FixedList128Bytes<float3>();
        for (int i = 0; i < authoring.MovePoints.Length; i++)
        {
            spawnPoints.Add(authoring.MovePoints[i]);
        }

        var data = new ConfigData()
        {
            Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
            SpawnPosition = authoring.SpawnPosition,
            SpawnInterval = authoring.SpawnInterval,
            LastSpawnTime = 0f,
            MaxMonsterCount = authoring.MaxMonsterCount,
            MovePoints = spawnPoints
        };
        AddComponent(GetEntity(TransformUsageFlags.None), data);
    }
}

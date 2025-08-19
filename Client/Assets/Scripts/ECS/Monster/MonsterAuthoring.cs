using Unity.Entities;
using UnityEngine;

class MonsterAuthoring : MonoBehaviour
{
    public float Speed = 1f;
}

class MonsterAuthoringBaker : Baker<MonsterAuthoring>
{
    public override void Bake(MonsterAuthoring authoring)
    {
        var data = new MonsterData() { Speed = authoring.Speed };
        AddComponent(GetEntity(TransformUsageFlags.Dynamic), data);
    }
}

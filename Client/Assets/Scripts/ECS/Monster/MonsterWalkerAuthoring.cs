using Unity.Entities;
using UnityEngine;

class MonsterWalkerAuthoring : MonoBehaviour
{
    public float StopingDistance = 0.1f;
}

class MonsterWalkerBaker : Baker<MonsterWalkerAuthoring>
{
    public override void Bake(MonsterWalkerAuthoring authoring)
    {
        var data = new MonsterWalkerData()
        {
            CurrentTargetIndex = 0,
            StoppingDistance = authoring.StopingDistance
        };
        AddComponent(GetEntity(TransformUsageFlags.Dynamic), data);
    }
}

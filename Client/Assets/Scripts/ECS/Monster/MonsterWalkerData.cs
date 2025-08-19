using Unity.Entities;

public struct MonsterWalkerData : IComponentData
{
    public int CurrentTargetIndex;
    public float StoppingDistance;
}

using Unity.Entities;
using UnityEngine;

public class SubSceneLoadAuthoring : MonoBehaviour
{
    public bool AutoLoad = true;
    public string SceneGuidString;
}

public struct SubSceneLoadConfig : IComponentData
{
    public Unity.Entities.Hash128 SceneGUID;
    public bool AutoLoad;
}

public class SubSceneLoadBaker : Baker<SubSceneLoadAuthoring>
{
    public override void Bake(SubSceneLoadAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        Unity.Entities.Hash128 guid = default;

        if (!guid.IsValid && !string.IsNullOrEmpty(authoring.SceneGuidString))
        {
            guid = new Unity.Entities.Hash128(authoring.SceneGuidString);
        }

        if (!guid.IsValid)
            return;

        AddComponent(entity, new SubSceneLoadConfig
        {
            SceneGUID = guid,
            AutoLoad = authoring.AutoLoad
        });
    }
}
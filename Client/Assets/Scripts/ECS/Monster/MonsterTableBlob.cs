using Unity.Collections;
using Unity.Entities;

public struct MonsterTableBlob
{
    public BlobArray<int> Ids;
    public BlobArray<float> Speeds;
}

public struct MonsterTable : IComponentData
{
    public BlobAssetReference<MonsterTableBlob> Blob;
}



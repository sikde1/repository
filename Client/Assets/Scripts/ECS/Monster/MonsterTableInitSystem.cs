using Project.Data;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct MonsterTableInitSystem : ISystem
{
	public void OnCreate(ref SystemState state)
	{
		if (!SystemAPI.HasSingleton<MonsterTable>())
			state.EntityManager.CreateEntity(typeof(MonsterTable));
	}

	public void OnUpdate(ref SystemState state)
	{
		var repo = MainSystem.Instance.GameDataRepository;
		var data = repo.GetData<DataUnitMonster>(DataType.UnitMonster);
		if (data == null || data.Monsters.Count == 0)
			return;

		var ids = data.Monsters.Keys.ToArray();
		var speeds = ids.Select(id => data.Monsters[id].Speed).ToArray();

		var builder = new BlobBuilder(Allocator.Temp);
		ref var root = ref builder.ConstructRoot<MonsterTableBlob>();

		var idsArr = builder.Allocate(ref root.Ids, ids.Length);
		var spdArr = builder.Allocate(ref root.Speeds, speeds.Length);

		for (int i = 0; i < ids.Length; i++)
		{
			idsArr[i] = ids[i];
			spdArr[i] = speeds[i];
		}

		var blobRef = builder.CreateBlobAssetReference<MonsterTableBlob>(Allocator.Persistent);
		builder.Dispose();

		var e = SystemAPI.GetSingletonEntity<MonsterTable>();
		state.EntityManager.SetComponentData(e, new MonsterTable { Blob = blobRef });

		state.Enabled = false;
	}

	public void OnDestroy(ref SystemState state)
	{
		if (SystemAPI.HasSingleton<MonsterTable>())
		{
			var e = SystemAPI.GetSingletonEntity<MonsterTable>();
			var table = SystemAPI.GetComponent<MonsterTable>(e);
			if (table.Blob.IsCreated)
				table.Blob.Dispose();
		}
	}
}



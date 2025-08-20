using FlatBufferData;
using Google.FlatBuffers;
using Project.FlatBuffer;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Project.Data
{
    public class DataUnitMonster : IFlatBufferData
    {
        public readonly Dictionary<int, FBUnitMonster> Monsters = new();

        public void LoadData(string name)
        {
            string path = Path.Combine(Application.streamingAssetsPath, $"{name}.bin");
            if (!File.Exists(path))
            {
                Debug.LogError($"{name}.bin 파일을 찾을 수 없습니다.");
            }

            byte[] data = File.ReadAllBytes(path);
            ByteBuffer buffer = new ByteBuffer(data);

            var root = FBUnitMonsterRoot.GetRootAsFBUnitMonsterRoot(buffer);
            int count = root.EntriesLength;

            for (int i = 0; i < count; ++i)
            {
                var entry = root.Entries(i);
                Monsters.Add(entry.Value.Idx, entry.Value);
            }
        }
    }
}

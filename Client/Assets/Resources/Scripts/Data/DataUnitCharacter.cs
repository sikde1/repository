using FlatBufferData;
using Google.FlatBuffers;
using Project.FlatBuffer;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Project.Data
{
    public class DataUnitCharacter : IFlatBufferData
    {
        public readonly Dictionary<int, FBUnitCharacter> Characters = new();
        public readonly Dictionary<int, List<int>> GradeCharacters = new(); // ĳ���� Grade �� �з�

        public void LoadData(string name)
        {
            string path = Path.Combine(Application.streamingAssetsPath, $"{name}.bin");
            if (!File.Exists(path))
            {
                Debug.LogError($"{name}.bin ������ ã�� �� �����ϴ�.");
            }

            byte[] data = File.ReadAllBytes(path);
            ByteBuffer buffer = new ByteBuffer(data);

            var root = FBUnitCharacterRoot.GetRootAsFBUnitCharacterRoot(buffer);
            int count = root.EntriesLength;

            for (int i = 0; i < count; ++i)
            {
                var entry = root.Entries(i);
                Characters.Add(entry.Value.Idx, entry.Value);
                if (!GradeCharacters.ContainsKey(entry.Value.Grade))
                    GradeCharacters.Add(entry.Value.Grade, new List<int>());
                GradeCharacters[entry.Value.Grade].Add(entry.Value.Idx);
            }
        }
    }
}

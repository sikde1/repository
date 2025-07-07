using FlatBufferData;
using Google.FlatBuffers;
using Project.FlatBuffer;
using Project.UI.Type;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Project.Data
{
    public class DataUIPopup : IFlatBufferData
    {
        public readonly Dictionary<UIViewType, FBUIPopup> UIPopups = new();

        public void LoadData(string name)
        {
            string path = Path.Combine(Application.streamingAssetsPath, $"{name}.bin");
            if (!File.Exists(path))
            {
                Debug.LogError($"{name}.bin 파일을 찾을 수 없습니다.");
            }

            byte[] data = File.ReadAllBytes(path);
            ByteBuffer buffer = new ByteBuffer(data);

            var root = FBUIPopupRoot.GetRootAsFBUIPopupRoot(buffer);
            int count = root.EntriesLength;

            for (int i = 0; i < count; ++i)
            {
                var entry = root.Entries(i);
                if(Enum.TryParse<UIViewType>(entry.Value.Type, out var result))
                    UIPopups.Add(result, entry.Value);
            }
        }
    }
}

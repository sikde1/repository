using Project.Data;
using System.Collections.Generic;

namespace Project.FlatBuffer
{
    public class FlatBufferReader
    {
        private Dictionary<string, IFlatBufferData> _datas = new()
        {
            { "FBUnitCharacter", new DataUnitCharacter() }
        };
        public void Initialize()
        {
            foreach (var data in _datas)
            {
                data.Value.LoadData(data.Key);
            }
        }
    }
}


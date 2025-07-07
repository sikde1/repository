using Project.FlatBuffer;
using System;

namespace Project.Data
{
    public static class DataTypeExtensions
    {
        /// <summary>
        /// DataType에서 FlatBuffer 파일명 추출
        /// 
        /// 예: DataType.UnitCharacter → "FBUnitCharacter"
        /// 이 문자열은 StreamingAssets/{fileName}.bin 파일을 찾는데 사용됨
        /// </summary>
        public static string GetFileName(this DataType dataType)
        {
            return dataType switch
            {
                DataType.UnitCharacter => "FBUnitCharacter",
                DataType.UIPopup => "FBUIPopup",
                _ => throw new ArgumentException($"Unknown DataType: {dataType}")
            };
        }

        /// <summary>
        /// DataType에서 해당 Data 클래스 타입 가져오기
        /// 
        /// 용도: 타입 검증, 리플렉션 등에 활용
        /// </summary>
        public static Type GetDataClassType(this DataType dataType)
        {
            return dataType switch
            {
                DataType.UnitCharacter => typeof(DataUnitCharacter),
                DataType.UIPopup => typeof(DataUIPopup),
                _ => throw new ArgumentException($"Unknown DataType: {dataType}")
            };
        }

        /// <summary>
        /// DataType에서 해당 Data 클래스 인스턴스 생성
        /// 
        /// Factory Pattern 적용:
        /// - 객체 생성 로직을 중앙화
        /// - 새로운 Data 클래스 추가 시 이 메서드만 수정하면 됨
        /// </summary>
        public static IFlatBufferData CreateDataInstance(this DataType dataType)
        {
            return dataType switch
            {
                DataType.UnitCharacter => new DataUnitCharacter(),
                DataType.UIPopup => new DataUIPopup(),
                _ => throw new ArgumentException($"Unknown DataType: {dataType}")
            };
        }
    }
} 
namespace Project.Data
{
    public enum DataType
    {
        UnitCharacter = 0,
        UIPopup = 1,
        UnitMonster = 2,
        
        // 새로운 데이터 타입 추가 시:
        // 1. 여기에 열거형 값 추가
        // 2. DataTypeExtensions.cs에 매핑 로직 추가
        // 3. 해당 Data 클래스 생성
    }
} 
using Project.FlatBuffer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Data
{
    public class GameDataRepository : IDisposable
    {
        private readonly Dictionary<DataType, IFlatBufferData> _dataCache = new();
        private bool _disposed = false;

        /// <summary>
        /// 모든 테이블 데이터 초기화 및 로드
        /// 1. DataType 열거형의 모든 값에 대해 FlatBuffer 파일 로드
        /// 2. 메모리 캐시에 저장하여 빠른 접근 제공
        /// 3. 게임 시작 시 한 번만 실행됨 (정적 테이블 데이터)
        /// </summary>
        public void Initialize()
        {
            try
            {
                foreach (DataType dataType in Enum.GetValues(typeof(DataType)))
                {
                    LoadData(dataType);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"GameDataRepository 초기화 실패: {ex.Message}");
                throw; // 테이블 데이터 로드 실패는 치명적이므로 예외를 다시 던짐
            }
        }

        /// <summary>
        /// 특정 테이블 데이터 로드 (내부 메서드)
        /// 1. DataType에 따라 적절한 Data 클래스 인스턴스 생성
        /// 2. StreamingAssets에서 해당 FlatBuffer 파일 로드
        /// 3. 메모리 캐시에 저장
        /// </summary>
        private void LoadData(DataType dataType)
        {
            try
            {
                // DataType에 맞는 테이블 데이터 클래스 인스턴스 생성
                var dataInstance = dataType.CreateDataInstance();
                var fileName = dataType.GetFileName();
                
                // StreamingAssets에서 FlatBuffer 테이블 데이터 로드
                dataInstance.LoadData(fileName);
                
                // 메모리 캐시에 저장
                _dataCache[dataType] = dataInstance;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{dataType} 테이블 데이터 로드 실패: {ex.Message}");
                // 개별 테이블 로드 실패는 로그만 남기고 계속 진행
            }
        }

        /// <summary>
        /// 캐시된 테이블 데이터 조회 (핵심 메서드)
        /// var characterData = GameDataRepository.GetData&lt;DataUnitCharacter&gt;(DataType.UnitCharacter);
        /// var popupData = GameDataRepository.GetData&lt;DataUIPopup&gt;(DataType.UIPopup);
        /// - 메모리 캐시에서 즉시 반환
        /// - 컴파일 타임 타입 검증
        /// - 전역 어디서든 안전하게 접근 가능
        /// </summary>
        public T GetData<T>(DataType dataType) where T : class, IFlatBufferData
        {
            // Repository 상태 확인
            if (_disposed)
            {
                Debug.LogWarning("GameDataRepository가 이미 Dispose되었습니다.");
                return null;
            }

            // 캐시에서 데이터 조회
            if (_dataCache.TryGetValue(dataType, out var data))
            {
                var result = data as T;
                if (result == null)
                {
                    Debug.LogError($"{dataType} 데이터의 타입이 {typeof(T).Name}과 일치하지 않습니다.");
                }
                return result;
            }

            Debug.LogError($"{dataType} 데이터를 찾을 수 없습니다. Initialize()가 호출되었는지 확인하세요.");
            return null;
        }

        /// <summary>
        /// 테이블 데이터 존재 여부 확인
        /// 안전한 데이터 접근을 위한 사전 검증
        /// </summary>
        public bool HasData(DataType dataType)
        {
            return !_disposed && _dataCache.ContainsKey(dataType);
        }
        public void Dispose()
        {
            if (_disposed) return;

            _dataCache.Clear();
            _disposed = true;
        }
    }
} 
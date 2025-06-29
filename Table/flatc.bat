@echo off
setlocal

:: 현재 배치 파일의 경로를 가져옵니다.
set BATCH_DIR=%~dp0

:: flatc.exe 경로 (배치 파일과 동일한 경로에 flatc.exe가 있다고 가정)
set FLATC=%BATCH_DIR%flatc.exe

:: JSON 파일과 FBS 파일 경로를 받아옵니다.
set FBS_FILE=%1
set JSON_FILE=%2

:: 현재 배치파일 기준으로 상위 디렉토리
set BASE_DIR=%~dp0..
set BIN_OUTPUT=%BASE_DIR%\Client\Assets\StreamingAssets
set CS_OUTPUT=%BASE_DIR%\Client\Assets\Resources

:: flatc 실행 - bin 생성
"%FLATC%" -b -o "%BIN_OUTPUT%" "%FBS_FILE%" "%JSON_FILE%"

:: flatc 실행 - C# 코드 생성
"%FLATC%" --csharp -o "%CS_OUTPUT%" "%FBS_FILE%"

del /q "%FBS_FILE%"
del /q "%JSON_FILE%"

:: 변환이 완료되면 완료 메시지를 출력
if %errorlevel% equ 0 (
    echo "변환 완료!"
) else (
    echo "변환 중 오류 발생"
)

endlocal

@echo off
echo ========================================
echo Caprim API Stellar - Despliegue Lambda
echo ========================================

echo.
echo Verificando AWS CLI...
aws --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: AWS CLI no está instalado o configurado.
    echo Instala AWS CLI desde: https://aws.amazon.com/cli/
    pause
    exit /b 1
)

echo.
echo Verificando AWS SAM CLI...
sam --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: AWS SAM CLI no está instalado.
    echo Instala AWS SAM CLI desde: https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/serverless-sam-cli-install.html
    pause
    exit /b 1
)

echo.
echo Verificando credenciales AWS...
aws sts get-caller-identity >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: Credenciales AWS no configuradas.
    echo Ejecuta: aws configure
    pause
    exit /b 1
)

echo.
echo Construyendo aplicación...
dotnet publish -c Release -o ./publish

if %errorlevel% neq 0 (
    echo ERROR: Falló la construcción de la aplicación.
    pause
    exit /b 1
)

echo.
echo Construyendo con SAM...
sam build

if %errorlevel% neq 0 (
    echo ERROR: Falló la construcción con SAM.
    pause
    exit /b 1
)

echo.
echo Validando template...
sam validate

if %errorlevel% neq 0 (
    echo ERROR: Template inválido.
    pause
    exit /b 1
)

echo.
echo ¿Deseas continuar con el despliegue? (y/n)
set /p respuesta=

if /i not "%respuesta%"=="y" (
    echo Despliegue cancelado.
    pause
    exit /b 0
)

echo.
echo Desplegando a AWS Lambda...
sam deploy --guided

if %errorlevel% neq 0 (
    echo ERROR: Falló el despliegue.
    pause
    exit /b 1
)

echo.
echo ========================================
echo ¡Despliegue completado exitosamente!
echo ========================================
echo.
echo Recuerda configurar las variables de entorno en la consola de AWS Lambda.
echo Revisa el README.md para instrucciones detalladas.
echo.
pause
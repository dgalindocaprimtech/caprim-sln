@echo off
echo Configurando variables de entorno para Caprim API Stellar...
echo.

if exist .env (
    echo El archivo .env ya existe. ¿Quieres sobrescribirlo? (s/n)
    set /p overwrite=
    if /i not "!overwrite!"=="s" goto :end
)

copy .env.example .env
echo.
echo Archivo .env creado. Edítalo con tus valores reales antes de ejecutar docker-compose.
echo.
echo Valores que necesitas configurar:
echo - POSTGRES_HOST: Host de tu base de datos PostgreSQL
echo - POSTGRES_USER: Usuario de la base de datos
echo - POSTGRES_PASSWORD: Password de la base de datos
echo - POSTGRES_DB: Nombre de la base de datos
echo - COGNITO_USER_POOL_ID: ID de tu User Pool de Cognito
echo - COGNITO_CLIENT_ID: Client ID de tu App Client
echo.
echo Una vez configurado, ejecuta: docker-compose up --build
echo.

:end
pause
# Caprim API Stellar

API REST para plataforma de custodia y gestión de activos digitales en la red Stellar. Construida con ASP.NET Core 8.0, PostgreSQL y AWS Cognito.

## � Tabla de Contenidos

- [Características](#-características)
- [Arquitectura](#-arquitectura)
- [Requisitos](#-requisitos)
- [Instalación y Configuración](#-instalación-y-configuración)
- [Ejecutar la Aplicación](#-ejecutar-la-aplicación)
- [API Endpoints](#-api-endpoints)
- [Autenticación](#-autenticación)
- [Base de Datos](#-base-de-datos)
- [Configuración](#-configuración)
- [Monitoreo](#-monitoreo)
- [Desarrollo](#-desarrollo)
- [Despliegue](#-despliegue)
- [Seguridad](#-seguridad)
- [Contribución](#-contribución)

## ✨ Características

- ✅ **Gestión de Usuarios**: CRUD completo con integración AWS Cognito
- ✅ **Cuentas Stellar**: Administración de cuentas y balances
- ✅ **Transacciones**: Envío y recepción de XLM y USDC
- ✅ **Trustlines**: Establecimiento de líneas de confianza para assets
- ✅ **Exchange Rates**: Tasas de cambio en tiempo real
- ✅ **Autenticación JWT**: Integración completa con AWS Cognito
- ✅ **API RESTful**: Endpoints bien documentados con Swagger
- ✅ **Docker**: Containerización completa para fácil despliegue
- ✅ **PostgreSQL**: Base de datos robusta con esquemas versionados
- ✅ **Health Checks**: Monitoreo de salud de la aplicación

## 🏗️ Arquitectura

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   React Native  │    │   API Gateway   │    │   AWS Cognito   │
│     (Mobile)    │◄──►│  (ASP.NET Core) │◄──►│  (Auth/JWT)    │
└─────────────────┘    └─────────────────┘    └─────────────────┘
                                │
                                ▼
                       ┌─────────────────┐
                       │  PostgreSQL     │
                       │  (Database)     │
                       └─────────────────┘
                                │
                                ▼
                       ┌─────────────────┐
                       │   Stellar       │
                       │   Network       │
                       └─────────────────┘
```

### Patrones Implementados

- **Repository Pattern**: Separación de lógica de datos
- **Service Layer**: Lógica de negocio centralizada
- **Dependency Injection**: Inyección de dependencias nativa de .NET
- **DTO Pattern**: Objetos de transferencia de datos
- **RESTful API**: Principios REST con HTTP verbs apropiados

## 📋 Requisitos

- **.NET 8.0 SDK**
- **Docker Desktop** (para desarrollo local)
- **PostgreSQL** (o usar contenedor Docker)
- **AWS Account** con Cognito configurado
- **Git**

### Requisitos del Sistema

- **RAM**: 4GB mínimo, 8GB recomendado
- **Disco**: 2GB para aplicación + base de datos
- **OS**: Windows 10/11, macOS, Linux

## 🚀 Instalación y Configuración

### 1. Clonar el Repositorio

```bash
git clone https://github.com/DiegoAGalindo/Caprim.sln.git
cd Caprim.sln/Caprim.API.Stellar
```

### 2. Configurar Variables de Entorno

**Opción A: Script automático (Windows)**
```bash
setup-env.bat
```

**Opción B: Configuración manual**
```bash
# Copiar archivo de ejemplo
cp .env.example .env

# Editar con tus valores reales
nano .env  # o notepad .env en Windows
```

**Variables requeridas:**
```bash
# Base de datos
POSTGRES_HOST=localhost
POSTGRES_DB=n8n
POSTGRES_USER=caprimtech
POSTGRES_PASSWORD=caprim2025.
POSTGRES_SCHEMA=CaprimApp

# AWS Cognito
COGNITO_USER_POOL_ID=us-east-1_lvRYtEIyu
COGNITO_CLIENT_ID=20hlo41tc8dgbfshpil3ps3lc2

# Stellar Network
STELLAR_NETWORK=MAINNET
STELLAR_HORIZON_URL=https://horizon.stellar.org
```

### 3. Ejecutar con Docker

```bash
# Construir y ejecutar
docker-compose up --build

# Ejecutar en background
docker-compose up -d --build
```

## 📖 API Endpoints

### 🔐 Autenticación Requerida

Todos los endpoints requieren autenticación JWT en el header:
```
Authorization: Bearer <tu_jwt_token>
```

### 👥 Usuarios (`/api/users`)

#### GET `/api/users`
Obtiene todos los usuarios del sistema.

**Respuesta (200):**
```json
[
  {
    "id": 1,
    "cognitoSub": "cognito-sub-uuid",
    "email": "usuario@example.com",
    "firstName": "Juan",
    "lastName": "Pérez",
    "phoneNumber": "+1234567890",
    "countryId": 1,
    "userStatusId": 1,
    "kycLevelId": 1,
    "createdAt": "2025-01-01T00:00:00Z",
    "updatedAt": "2025-01-01T00:00:00Z"
  }
]
```

#### GET `/api/users/{id}`
Obtiene un usuario específico por ID.

**Parámetros:**
- `id` (int): ID del usuario

**Respuesta (200):**
```json
{
  "id": 1,
  "cognitoSub": "cognito-sub-uuid",
  "email": "usuario@example.com",
  "firstName": "Juan",
  "lastName": "Pérez",
  "phoneNumber": "+1234567890",
  "countryId": 1,
  "userStatusId": 1,
  "kycLevelId": 1,
  "createdAt": "2025-01-01T00:00:00Z",
  "updatedAt": "2025-01-01T00:00:00Z"
}
```

#### GET `/api/users/cognito/{cognitoSub}`
Obtiene un usuario por su Cognito Subject ID.

**Parámetros:**
- `cognitoSub` (string): Subject ID de Cognito

#### POST `/api/users`
Crea un nuevo usuario.

**Body:**
```json
{
  "cognitoSub": "cognito-sub-uuid",
  "email": "usuario@example.com",
  "firstName": "Juan",
  "lastName": "Pérez",
  "phoneNumber": "+1234567890",
  "countryId": 1
}
```

**Respuesta (201):**
```json
{
  "id": 1,
  "message": "Usuario creado exitosamente"
}
```

#### PUT `/api/users/{id}`
Actualiza un usuario existente.

**Parámetros:**
- `id` (int): ID del usuario

**Body:**
```json
{
  "firstName": "Juan Carlos",
  "lastName": "Pérez García",
  "phoneNumber": "+0987654321"
}
```

#### DELETE `/api/users/{id}`
Elimina un usuario (soft delete).

**Parámetros:**
- `id` (int): ID del usuario

### 🌟 Cuentas Stellar (`/api/stellaraccounts`)

#### GET `/api/stellaraccounts`
Obtiene todas las cuentas Stellar.

#### GET `/api/stellaraccounts/user/{userId}`
Obtiene las cuentas Stellar de un usuario específico.

**Parámetros:**
- `userId` (int): ID del usuario

#### GET `/api/stellaraccounts/{id}`
Obtiene una cuenta Stellar específica.

#### GET `/api/stellaraccounts/publickey/{publicKey}`
Obtiene una cuenta por su public key.

#### POST `/api/stellaraccounts`
Crea una nueva cuenta Stellar.

**Body:**
```json
{
  "userId": 1,
  "accountName": "Cuenta Principal",
  "publicKey": "GABC...XYZ",
  "encryptedPrivateKey": "encrypted_key_data",
  "isActive": true
}
```

#### PUT `/api/stellaraccounts/{id}`
Actualiza una cuenta Stellar.

### 💸 Transacciones (`/api/transactions`)

#### GET `/api/transactions`
Obtiene todas las transacciones.

#### GET `/api/transactions/account/{stellarAccountId}`
Obtiene transacciones de una cuenta específica.

#### GET `/api/transactions/{id}`
Obtiene una transacción específica.

#### GET `/api/transactions/hash/{stellarTxHash}`
Obtiene una transacción por su hash en Stellar.

#### POST `/api/transactions`
Registra una nueva transacción.

**Body:**
```json
{
  "stellarAccountId": 1,
  "transactionTypeId": 1,
  "amount": 100.50,
  "assetCode": "XLM",
  "destinationAddress": "GABC...XYZ",
  "stellarTxHash": "hash_de_stellar",
  "status": "PENDING"
}
```

#### DELETE `/api/transactions/{id}`
Elimina una transacción.

### 💰 Operaciones de Transacción (`/api/transaction`)

#### POST `/api/transaction/send-xlm`
Envía XLM nativo.

**Body:**
```json
{
  "fromSecret": "SABC...XYZ",
  "toAddress": "GABC...XYZ",
  "amount": "100.0",
  "memo": "Pago de servicios"
}
```

#### POST `/api/transaction/send-usdc`
Envía USDC.

**Body:**
```json
{
  "fromSecret": "SABC...XYZ",
  "toAddress": "GABC...XYZ",
  "amount": "50.0",
  "issuerAddress": "GUSDC_ISSUER_ADDRESS"
}
```

### 🔗 Trustlines (`/api/trustline`)

#### POST `/api/trustline/establish`
Establece una trustline para un asset.

**Body:**
```json
{
  "secretKey": "SABC...XYZ",
  "assetCode": "USDC",
  "issuerAddress": "GUSDC_ISSUER_ADDRESS",
  "limit": "10000"
}
```

### 📊 Exchange Rates (`/api/exchangerates`)

#### GET `/api/exchangerates`
Obtiene las tasas de cambio actuales.

#### GET `/api/exchangerates/{id}`
Obtiene una tasa específica.

#### POST `/api/exchangerates`
Crea una nueva tasa de cambio.

#### PUT `/api/exchangerates/{id}`
Actualiza una tasa.

#### DELETE `/api/exchangerates/{id}`
Elimina una tasa.

### 🏥 Health Check (`/health`)

#### GET `/health`
Verifica el estado de la aplicación.

**Respuesta (200):**
```json
{
  "status": "healthy",
  "timestamp": "2025-01-01T12:00:00Z"
}
```

## 🔐 Autenticación

### AWS Cognito JWT

La API utiliza JSON Web Tokens (JWT) emitidos por AWS Cognito para autenticación.

#### Obtener un Token

1. **Configura tu App Client** en Cognito con:
   - Authorization Code Grant activado
   - PKCE activado (para apps móviles)

2. **Para pruebas**, puedes usar Postman o curl:

```bash
curl -X POST "https://us-east-1qvnqk2unb.auth.us-east-1.amazoncognito.com/oauth2/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -H "Authorization: Basic <base64(client_id:client_secret)>" \
  -d "grant_type=password&username=tu_usuario&password=tu_password"
```

#### Usar el Token

```bash
curl -X GET "http://localhost:8080/api/users" \
  -H "Authorization: Bearer TU_JWT_TOKEN"
```

### Swagger UI

1. Abre `http://localhost:8080/swagger`
2. Haz clic en **"Authorize"** (candado)
3. Ingresa: `Bearer TU_JWT_TOKEN`
4. Haz clic en **"Authorize"**

## 🗄️ Base de Datos

### PostgreSQL con Esquemas

- **Host**: Configurable via `POSTGRES_HOST`
- **Base de datos**: `n8n`
- **Esquema**: `CaprimApp`
- **Usuario**: `caprimtech`

### Migraciones

Los scripts SQL están versionados en `Database/Scripts/`:
- `V1.0/create_schema.sql` - Esquema inicial completo

### Tablas Principales

- `users` - Información de usuarios
- `stellar_accounts` - Cuentas Stellar
- `transactions` - Historial de transacciones
- `countries` - Catálogo de países
- `user_statuses` - Estados de usuario
- `kyc_levels` - Niveles KYC
- `transaction_types` - Tipos de transacción
- `exchange_rates` - Tasas de cambio

## ⚙️ Configuración

### Variables de Entorno

| Variable | Descripción | Valor por defecto |
|----------|-------------|-------------------|
| `POSTGRES_HOST` | Host de PostgreSQL | `localhost` |
| `POSTGRES_DB` | Nombre de la BD | `n8n` |
| `POSTGRES_USER` | Usuario de BD | `caprimtech` |
| `POSTGRES_PASSWORD` | Password de BD | Requerido |
| `POSTGRES_SCHEMA` | Esquema de BD | `CaprimApp` |
| `COGNITO_USER_POOL_ID` | User Pool ID | Requerido |
| `COGNITO_CLIENT_ID` | Client ID | Requerido |
| `STELLAR_NETWORK` | Red Stellar | `MAINNET` |
| `STELLAR_HORIZON_URL` | URL de Horizon | `https://horizon.stellar.org` |

### Configuración de Stellar

- **Mainnet**: Para producción real
- **Testnet**: Para desarrollo y pruebas

## 📊 Monitoreo

### Health Checks

- **Endpoint**: `GET /health`
- **Docker**: Automático en docker-compose
- **Intervalo**: 30 segundos

### Logs

```bash
# Ver logs de Docker
docker-compose logs -f caprim-api-stellar

# Ver logs específicos
docker-compose logs caprim-api-stellar
```

### Métricas

- Application Insights (configurable)
- Logs estructurados con Serilog
- Health checks automáticos

## 💻 Desarrollo

### Estructura del Proyecto

```
Caprim.API.Stellar/
├── Controllers/          # Controladores REST API
├── Models/              # Entidades de EF Core
├── Dtos/                # Objetos de transferencia
├── Services/            # Lógica de negocio
│   ├── Interfaces/      # Contratos de servicios
│   └── Implementations/ # Implementaciones
├── Database/
│   └── Scripts/         # Scripts SQL versionados
├── Properties/
│   └── launchSettings.json
├── appsettings.json     # Configuración local
├── Program.cs           # Punto de entrada
├── Dockerfile           # Containerización
└── docker-compose.yml   # Orquestación
```

### Ejecutar en Desarrollo

```bash
# Restaurar dependencias
dotnet restore

# Ejecutar
dotnet run

# La aplicación estará en:
# - API: http://localhost:5237
# - Swagger: http://localhost:5237/swagger
```

### Ejecutar Tests

```bash
# Ejecutar todos los tests
dotnet test

# Con cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## 🚢 Despliegue

### AWS Lambda

La API está completamente compatible con AWS Lambda para despliegues serverless.

#### Prerrequisitos para Lambda

- **AWS CLI** configurado con credenciales
- **AWS SAM CLI** instalado
- **Cuenta AWS** con permisos para Lambda, API Gateway y RDS

#### Despliegue en AWS Lambda

**1. Instalar AWS SAM CLI**
```bash
# Windows (Chocolatey)
choco install aws-sam-cli

# macOS (Homebrew)
brew install aws-sam-cli

# Linux
wget https://github.com/aws/aws-sam-cli/releases/latest/download/aws-sam-cli-linux-x86_64.zip
unzip aws-sam-cli-linux-x86_64.zip -d sam-installation
sudo ./sam-installation/install
```

**2. Configurar AWS**
```bash
# Configurar credenciales AWS
aws configure

# Verificar configuración
aws sts get-caller-identity
```

**3. Preparar el despliegue**
```bash
# Construir la aplicación
sam build

# Validar el template
sam validate
```

**4. Desplegar a AWS**
```bash
# Desplegar (primera vez)
sam deploy --guided

# Desplegar actualizaciones
sam deploy
```

**Parámetros durante el despliegue:**
- **Stack Name**: `caprim-api-stellar-stack`
- **AWS Region**: `us-east-1` (o tu región preferida)
- **Confirm changes**: `y`
- **Allow SAM CLI IAM role**: `y`

#### Configuración de Variables de Entorno en Lambda

Después del despliegue, configura estas variables en la consola de AWS Lambda:

```bash
# Base de datos
ConnectionStrings__DefaultConnection=Host=tu-rds-endpoint.rds.amazonaws.com;Database=tu_db;Username=tu_user;Password=tu_password;SearchPath=CaprimApp

# Cognito
Cognito__Region=us-east-1
Cognito__UserPoolId=tu_user_pool_id
Cognito__ClientId=tu_client_id

# Stellar
Stellar__Environment=PROD
Stellar__HorizonUrl=https://horizon.stellar.org
Stellar__HorizonUrlDev=https://horizon-testnet.stellar.org
```

#### Probar Lambda Localmente

**Opción 1: Usando Docker Compose**
```bash
# Ejecutar solo el servicio Lambda local
docker-compose --profile lambda-local up caprim-api-lambda-local

# La API estará disponible en:
# http://localhost:9000/2015-03-31/functions/function/invocations
```

**Opción 2: Usando AWS SAM CLI**
```bash
# Iniciar API local
sam local start-api --port 3000

# La API estará disponible en:
# http://localhost:3000
```

**Probar un endpoint:**
```bash
# Health check
curl http://localhost:3000/health

# Con SAM local
curl http://localhost:3000/health
```

#### URLs después del despliegue

Después del despliegue exitoso, SAM mostrará:
- **API Gateway URL**: `https://xxxxx.execute-api.us-east-1.amazonaws.com/Prod/`
- **Lambda Function ARN**: `arn:aws:lambda:us-east-1:123456789012:function:caprim-api-stellar`

#### Monitoreo en AWS

**CloudWatch Logs:**
```bash
# Ver logs de Lambda
aws logs tail /aws/lambda/caprim-api-stellar --follow

# Ver métricas
aws cloudwatch get-metric-statistics \
  --namespace AWS/Lambda \
  --metric-name Duration \
  --start-time 2024-01-01T00:00:00Z \
  --end-time 2024-01-02T00:00:00Z \
  --period 3600 \
  --statistics Average \
  --function-name caprim-api-stellar
```

**API Gateway Logs:**
- Ve a la consola de API Gateway
- Selecciona tu API
- Ve a "Logs" en el menú lateral

#### Costos Estimados

- **Lambda**: ~$0.20 por 1M requests + costo por duración
- **API Gateway**: ~$3.50 por millón de requests
- **RDS PostgreSQL**: Depende del tamaño de instancia

#### Troubleshooting Lambda

**Error: Function timeout**
```bash
# Aumentar timeout en serverless.template
"Timeout": 60  # segundos
```

**Error: Memory limit exceeded**
```bash
# Aumentar memoria en serverless.template
"MemorySize": 1024  # MB
```

**Error: Database connection timeout**
- Verificar que la Lambda esté en la misma VPC que RDS
- Configurar security groups correctamente
- Usar connection pooling

**Logs de debugging:**
```bash
# Ver logs detallados
sam logs -n CaprimApiStellarFunction --stack-name caprim-api-stellar-stack --tail
```

### Docker Production

```bash
# Construir imagen
docker build -t caprim-api-stellar:latest .

# Ejecutar en producción
docker run -d \
  --name caprim-api-prod \
  -p 8080:8080 \
  --env-file .env \
  caprim-api-stellar:latest
```

### AWS ECS/Fargate

```yaml
# task-definition.json
{
  "family": "caprim-api-stellar",
  "containerDefinitions": [
    {
      "name": "caprim-api",
      "image": "tu-registry/caprim-api-stellar:latest",
      "essential": true,
      "portMappings": [
        {
          "containerPort": 8080,
          "hostPort": 8080
        }
      ],
      "environment": [
        {"name": "ASPNETCORE_ENVIRONMENT", "value": "Production"}
      ],
      "secrets": [
        {"name": "POSTGRES_PASSWORD", "valueFrom": "arn:aws:secretsmanager:..."}
      ]
    }
  ]
}
```

### CI/CD con GitHub Actions

```yaml
# .github/workflows/deploy.yml
name: Deploy to AWS
on:
  push:
    branches: [ main ]
jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Build and push Docker image
      run: |
        docker build -t caprim-api-stellar .
        docker tag caprim-api-stellar:latest your-registry/caprim-api-stellar:latest
        docker push your-registry/caprim-api-stellar:latest
```

## � Seguridad

### Autenticación y Autorización

- **JWT Bearer Tokens** de AWS Cognito
- **RBAC** (Role-Based Access Control) implementado
- **API Keys** para integraciones de terceros (futuro)

### Protección de Datos

- **Encriptación** de claves privadas Stellar
- **Hashing** de datos sensibles
- **Auditoría** completa de transacciones

### Mejores Prácticas

- ✅ **HTTPS obligatorio** en producción
- ✅ **Rate limiting** implementado
- ✅ **Input validation** en todos los endpoints
- ✅ **SQL injection prevention** con EF Core
- ✅ **CORS** configurado apropiadamente
- ✅ **Security headers** (HSTS, CSP, etc.)

### Secrets Management

- **AWS Secrets Manager** para producción
- **Azure Key Vault** como alternativa
- **Archivo .env** solo para desarrollo local

## 🤝 Contribución

### Guías de Desarrollo

1. **Fork** el repositorio
2. **Crea** una rama feature: `git checkout -b feature/nueva-funcionalidad`
3. **Commit** tus cambios: `git commit -m 'Agrega nueva funcionalidad'`
4. **Push** a la rama: `git push origin feature/nueva-funcionalidad`
5. **Crea** un Pull Request

### Estándares de Código

- **C# 11** con nullable reference types
- **RESTful** API design
- **SOLID** principles
- **Clean Architecture**
- **Documentación XML** en métodos públicos

### Testing

```bash
# Ejecutar tests unitarios
dotnet test

# Ejecutar tests de integración
dotnet test --filter Category=Integration

# Generar reporte de cobertura
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage
```

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

---

## 📖 Guía Completa de Configuración AWS

Para instrucciones detalladas paso a paso sobre cómo configurar toda la infraestructura AWS (RDS, Cognito, Lambda, API Gateway), consulta el archivo:

**[AWS_LAMBDA_SETUP_GUIDE.md](AWS_LAMBDA_SETUP_GUIDE.md)**

Esta guía incluye:
- ✅ Configuración completa de AWS CLI y SAM CLI
- ✅ Creación y configuración de RDS PostgreSQL
- ✅ Setup de AWS Cognito User Pool
- ✅ Despliegue paso a paso de Lambda
- ✅ Configuración de API Gateway
- ✅ Pruebas y troubleshooting
- ✅ Estimación de costos

---

## 📞 Soporte
```

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 📞 Soporte

- **Issues**: [GitHub Issues](https://github.com/DiegoAGalindo/Caprim.sln/issues)
- **Discussions**: [GitHub Discussions](https://github.com/DiegoAGalindo/Caprim.sln/discussions)
- **Email**: soporte@caprimtech.com

## 🙏 Agradecimientos

- **Stellar Development Foundation** por la red Stellar
- **AWS** por los servicios Cognito y RDS
- **Microsoft** por ASP.NET Core
- **PostgreSQL** por la base de datos robusta

---

**Caprim Tech** - Innovando en el futuro de las finanzas digitales 🚀
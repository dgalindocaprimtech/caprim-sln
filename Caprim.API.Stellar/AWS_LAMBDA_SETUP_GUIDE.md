# 🚀 Guía Paso a Paso: Configuración de AWS Lambda para Caprim API Stellar

## 📋 Requisitos Previos

### 1. Cuenta AWS
- ✅ Cuenta AWS activa con permisos administrativos
- ✅ Billing habilitado (para evitar sorpresas)

### 2. Herramientas Locales
```bash
# Instalar AWS CLI
# Windows (PowerShell como admin):
msiexec.exe /i https://awscli.amazonaws.com/AWSCLIV2.msi

# macOS:
curl "https://awscli.amazonaws.com/AWSCLIV2.pkg" -o "AWSCLIV2.pkg"
sudo installer -pkg AWSCLIV2.pkg -target /

# Linux:
curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip"
unzip awscliv2.zip
sudo ./aws/install

# Verificar instalación
aws --version
```

### 3. AWS SAM CLI
```bash
# Windows (Chocolatey):
choco install aws-sam-cli

# macOS (Homebrew):
brew tap aws/tap
brew install aws-sam-cli

# Linux:
wget https://github.com/aws/aws-sam-cli/releases/latest/download/aws-sam-cli-linux-x86_64.zip
unzip aws-sam-cli-linux-x86_64.zip -d sam-installation
sudo ./sam-installation/install

# Verificar instalación
sam --version
```

### 4. .NET 8 SDK
```bash
# Verificar instalación
dotnet --version
# Debe ser 8.x.x
```

---

## 🔐 Paso 1: Configurar Credenciales AWS

### Opción A: AWS CLI (Recomendado)
```bash
# Configurar credenciales
aws configure

# Ingresar:
# AWS Access Key ID: [tu access key]
# AWS Secret Access Key: [tu secret key]
# Default region name: us-east-1
# Default output format: json
```

### Opción B: Variables de Entorno
```bash
# Windows (PowerShell):
$env:AWS_ACCESS_KEY_ID="tu-access-key"
$env:AWS_SECRET_ACCESS_KEY="tu-secret-key"
$env:AWS_DEFAULT_REGION="us-east-1"

# Linux/macOS:
export AWS_ACCESS_KEY_ID="tu-access-key"
export AWS_SECRET_ACCESS_KEY="tu-secret-key"
export AWS_DEFAULT_REGION="us-east-1"
```

### Verificar Configuración
```bash
# Probar conexión
aws sts get-caller-identity

# Debe mostrar algo como:
# {
#     "UserId": "AIDACKCEVSQ6C2EXAMPLE",
#     "Account": "123456789012",
#     "Arn": "arn:aws:iam::123456789012:user/usuario"
# }
```

---

## 🗄️ Paso 2: Configurar Base de Datos (RDS PostgreSQL)

### 2.1 Crear Instancia RDS

1. **Ir a AWS Console** → RDS
2. **Crear base de datos**:
   - **Método de creación**: Estándar
   - **Engine**: PostgreSQL
   - **Versión**: 15.x (o la más reciente)
   - **Plantillas**: Dev/Test (para empezar)
   - **Identificador de instancia**: `caprim-stellar-db`
   - **Credenciales**:
     - Usuario maestro: `caprimadmin`
     - Contraseña: `[tu_password_seguro]`
   - **Configuración de instancia**:
     - Clase de instancia: `db.t3.micro` (gratuito por 12 meses)
     - Almacenamiento: 20 GB
   - **Conectividad**:
     - VPC: Default
     - Subnet group: Default
     - Acceso público: Sí (para desarrollo)
     - Grupo de seguridad: Crear nuevo
       - Nombre: `caprim-db-sg`
       - Reglas de entrada: PostgreSQL (5432) desde Anywhere (0.0.0.0/0)

3. **Esperar** a que se cree la instancia (15-20 minutos)

### 2.2 Obtener Endpoint de RDS
1. Ir a **RDS** → **Bases de datos**
2. Seleccionar `caprim-stellar-db`
3. Copiar el **Endpoint** (ej: `caprim-stellar-db.xxxxxx.us-east-1.rds.amazonaws.com`)

### 2.3 Crear Base de Datos y Esquema
```bash
# Conectar a PostgreSQL
psql -h [tu-endpoint-rds] -U caprimadmin -d postgres

# Crear base de datos
CREATE DATABASE caprim_stellar;

# Salir
\q

# Conectar a la nueva base de datos
psql -h [tu-endpoint-rds] -U caprimadmin -d caprim_stellar

# Crear esquema
CREATE SCHEMA CaprimApp;

# Crear tablas (copia las sentencias SQL de Database/Scripts/)
\i /ruta/a/tu/archivo/create_schema.sql

# Verificar
\dn  # Listar esquemas
\dt CaprimApp.*  # Listar tablas
```

---

## 🔐 Paso 3: Configurar AWS Cognito

### 3.1 Crear User Pool

1. **Ir a AWS Console** → Cognito
2. **Crear user pool**:
   - **Nombre**: `CaprimStellarUsers`
   - **Autenticación**:
     - Proveedores: Cognito user pool
     - Sign-in options: Email
   - **Password policy**: Default
   - **MFA**: No (para desarrollo)
   - **Required attributes**: email
   - **Message delivery**: Send email with Cognito

3. **Crear** el User Pool

### 3.2 Crear App Client

1. En el User Pool creado, ir a **App clients**
2. **Crear app client**:
   - **App client name**: `CaprimStellarAPI`
   - **Client secret**: No generar (para API)
   - **Authentication flows**:
     - ✅ ALLOW_USER_PASSWORD_AUTH
     - ✅ ALLOW_REFRESH_TOKEN_AUTH
   - **Authentication flow session duration**: 3 minutos

3. Copiar el **Client ID** (ej: `20hlo41tc8dgbfshpil3ps3lc2`)

### 3.3 Obtener User Pool ID

1. En el User Pool, ir a **General settings**
2. Copiar el **User pool ID** (ej: `us-east-1_lvRYtEIyu`)

---

## 🚀 Paso 4: Desplegar Lambda Function

### 4.1 Preparar el Código

```bash
# Navegar al directorio del proyecto
cd Caprim.API.Stellar

# Restaurar dependencias
dotnet restore

# Verificar que compile
dotnet build
```

### 4.2 Configurar Variables de Entorno

Editar `serverless.template` y reemplazar los placeholders:

```json
{
  "Environment": {
    "Variables": {
      "ASPNETCORE_ENVIRONMENT": "Production",
      "ConnectionStrings__DefaultConnection": "Host=caprim-stellar-db.xxxxxx.us-east-1.rds.amazonaws.com;Database=caprim_stellar;Username=caprimadmin;Password=tu_password_seguro;SearchPath=CaprimApp",
      "Cognito__Region": "us-east-1",
      "Cognito__UserPoolId": "us-east-1_lvRYtEIyu",
      "Cognito__ClientId": "20hlo41tc8dgbfshpil3ps3lc2",
      "Stellar__Environment": "PROD",
      "Stellar__HorizonUrl": "https://horizon.stellar.org",
      "Stellar__HorizonUrlDev": "https://horizon-testnet.stellar.org"
    }
  }
}
```

### 4.3 Desplegar con SAM

```bash
# Construir la aplicación
sam build

# Validar el template
sam validate

# Desplegar (primera vez - guided)
sam deploy --guided

# Respuestas durante el despliegue:
# Stack name: caprim-api-stellar-stack
# AWS Region: us-east-1
# Confirm changes before deploy: y
# Allow SAM CLI IAM role creation: y
# Save arguments to configuration file: y
# SAM configuration file: samconfig.toml
# SAM configuration environment: default
```

### 4.4 Verificar Despliegue

```bash
# Ver logs del despliegue
sam logs -n CaprimApiStellarFunction --stack-name caprim-api-stellar-stack --tail
```

---

## 🌐 Paso 5: Configurar API Gateway

### 5.1 Verificar API Gateway

Después del despliegue, SAM crea automáticamente un API Gateway. Para verificar:

1. **Ir a AWS Console** → API Gateway
2. Buscar `caprim-api-stellar-stack`
3. Verificar que tenga:
   - Método: ANY
   - Ruta: /{proxy+}
   - Integración: Lambda

### 5.2 Obtener URL de la API

```bash
# Ver outputs del stack
aws cloudformation describe-stacks --stack-name caprim-api-stellar-stack --query 'Stacks[0].Outputs'

# O desde AWS Console:
# CloudFormation → Stacks → caprim-api-stellar-stack → Outputs
```

La URL será algo como:
`https://xxxxx.execute-api.us-east-1.amazonaws.com/Prod/`

---

## 🧪 Paso 6: Probar la API

### 6.1 Health Check

```bash
# Probar endpoint de health
curl https://xxxxx.execute-api.us-east-1.amazonaws.com/Prod/health

# Respuesta esperada:
# {"status":"healthy","timestamp":"2025-01-01T12:00:00Z"}
```

### 6.2 Probar con Autenticación

Primero necesitas obtener un token JWT de Cognito:

```bash
# Instalar AWS CLI Cognito helper (opcional)
pip install boto3

# O usar Postman/Insomnia para autenticación
```

### 6.3 Probar Endpoints Protegidos

```bash
# Ejemplo: Obtener usuarios
curl -X GET "https://xxxxx.execute-api.us-east-1.amazonaws.com/Prod/api/users" \
  -H "Authorization: Bearer TU_JWT_TOKEN"
```

---

## 🔧 Paso 7: Configuración Adicional (Opcional)

### 7.1 Configurar VPC (Recomendado para Producción)

Para conectar Lambda con RDS de manera segura:

1. **Crear VPC** con subnets privadas
2. **Configurar RDS** en VPC privada
3. **Configurar Lambda** para usar VPC:
   - En `serverless.template`, agregar:
   ```json
   "VpcConfig": {
     "SecurityGroupIds": ["sg-xxxxx"],
     "SubnetIds": ["subnet-xxxxx", "subnet-yyyyy"]
   }
   ```

### 7.2 Configurar Custom Domain

1. **Ir a API Gateway** → Custom domain names
2. **Crear dominio** con certificado ACM
3. **Configurar Route 53** para apuntar al API Gateway

### 7.3 Configurar Environment Variables Seguras

Usar AWS Systems Manager Parameter Store o Secrets Manager:

```json
"Environment": {
  "Variables": {
    "DB_PASSWORD": "{{resolve:ssm:caprim-db-password}}"
  }
}
```

---

## 📊 Paso 8: Monitoreo y Logs

### 8.1 CloudWatch Logs

```bash
# Ver logs en tiempo real
aws logs tail /aws/lambda/caprim-api-stellar --follow --region us-east-1

# Ver logs específicos
sam logs -n CaprimApiStellarFunction --stack-name caprim-api-stellar-stack
```

### 8.2 CloudWatch Metrics

- **Ir a AWS Console** → CloudWatch → Metrics
- Buscar namespace: `AWS/Lambda`
- Métricas disponibles:
  - Invocations
  - Duration
  - Errors
  - Throttles

### 8.3 X-Ray (Opcional)

Para tracing distribuido, agregar a `serverless.template`:

```json
"Tracing": "Active",
"Policies": [
  "AWSLambdaBasicExecutionRole",
  "AWSXRayDaemonWriteAccess"
]
```

---

## 🆘 Troubleshooting

### Error: "The security group 'sg-xxxxx' does not exist"
- Verificar que el security group existe en la VPC correcta
- Asegurarse de que Lambda esté en la misma VPC que RDS

### Error: "Database connection timeout"
- Verificar que RDS permite conexiones desde Lambda
- Si RDS está en VPC privada, configurar Lambda para usar la misma VPC

### Error: "Function timeout"
- Aumentar timeout en `serverless.template`:
```json
"Timeout": 60
```

### Error: "Memory limit exceeded"
- Aumentar memoria en `serverless.template`:
```json
"MemorySize": 1024
```

---

## 💰 Costos Estimados

### Gratuito / Bajo Costo:
- **Lambda**: Primer millón de requests gratis
- **API Gateway**: Primer millón de requests gratis
- **RDS t3.micro**: ~$15/mes (gratuito por 12 meses con free tier)

### Costos Típicos:
- **100k requests/mes**: ~$1-2
- **1M requests/mes**: ~$10-15
- **RDS t3.small**: ~$30/mes

---

## 🎯 Checklist Final

- ✅ AWS CLI configurado
- ✅ SAM CLI instalado
- ✅ RDS PostgreSQL creado y configurado
- ✅ Cognito User Pool y App Client creados
- ✅ Lambda function desplegada
- ✅ API Gateway configurado
- ✅ Variables de entorno configuradas
- ✅ Health check funcionando
- ✅ Endpoints protegidos accesibles

¡Tu API Stellar está lista para producción en AWS Lambda! 🚀
# 🚀 Instrucciones de Despliegue - Caprim API Stellar en AWS Lambda

## ✅ Configuración Completada

El archivo `serverless.template` ha sido configurado con:

### 🔧 **Mejoras Implementadas:**
- ✅ **Variables de Cognito reales** configuradas
- ✅ **Timeout aumentado** a 60 segundos (recomendado para operaciones Stellar)
- ✅ **Memoria aumentada** a 1024MB (recomendado para Entity Framework + Stellar SDK)
- ✅ **Políticas IAM** configuradas para RDS, SSM y Secrets Manager
- ✅ **Parámetros CloudFormation** para configuración flexible
- ✅ **SSL habilitado** para conexión PostgreSQL

### 🔐 **Seguridad Implementada:**
- ✅ **Password como parámetro seguro** (NoEcho: true)
- ✅ **Permisos IAM específicos** solo para recursos necesarios
- ✅ **SSL/TLS habilitado** para base de datos

---

## 🎯 **Cómo Desplegar:**

### **Paso 1: Configurar Variables**
Antes del despliegue, necesitas configurar estos valores:

```bash
# Variables requeridas:
RDS_HOST="tu-instancia-rds.cluster-xxxxx.us-east-1.rds.amazonaws.com"
RDS_DATABASE="tu_base_de_datos"  
RDS_USERNAME="tu_usuario_db"
RDS_PASSWORD="tu_password_seguro"
ENVIRONMENT="prod"  # o "dev", "staging"
```

### **Paso 2: Ejecutar Despliegue**
```bash
# Ejecutar el script de despliegue
.\deploy-lambda.bat

# O manualmente con SAM:
sam build
sam deploy --guided --parameter-overrides \
  RdsHost=tu-rds-host \
  RdsDatabase=tu-db \
  RdsUsername=tu-usuario \
  RdsPassword=tu-password \
  Environment=prod
```

### **Paso 3: Verificar Despliegue**
Después del despliegue, verifica:

1. **Function creada:** `caprim-api-stellar-prod`
2. **API Gateway:** URL en los outputs de CloudFormation
3. **Health Check:** `GET /health` debe responder `200 OK`
4. **Autenticación:** Endpoints protegidos requieren JWT

---

## 🛡️ **Configuración de Base de Datos (Recomendado):**

### **Opción 1: RDS PostgreSQL (Recomendado para Producción)**
```bash
# Crear RDS PostgreSQL instance
aws rds create-db-instance \
  --db-instance-identifier caprim-postgres-prod \
  --db-instance-class db.t3.micro \
  --engine postgres \
  --master-username caprimuser \
  --master-user-password [PASSWORD_SEGURO] \
  --allocated-storage 20 \
  --vpc-security-group-ids sg-xxxxx \
  --db-subnet-group-name caprim-db-subnet-group
```

### **Opción 2: Variables de Entorno Locales (Para Testing)**
Si tienes una base de datos PostgreSQL existente, solo actualiza los parámetros en el despliegue.

---

## 🔍 **Troubleshooting:**

### **Error Común: Connection String**
Si la Lambda no puede conectar a la base de datos:

1. **Verificar VPC:** La Lambda debe estar en la misma VPC que RDS
2. **Security Groups:** Permitir conexiones en puerto 5432
3. **SSL:** Verificar que RDS tenga SSL habilitado

### **Error Común: JWT Validation**
Si la autenticación falla:

1. **Cognito Config:** Verificar UserPoolId y ClientId
2. **Token Format:** Usar formato "Bearer {token}"
3. **Token Validity:** Verificar que el token no haya expirado

---

## 📋 **Checklist Post-Despliegue:**

- [ ] **Lambda Function** creada exitosamente
- [ ] **API Gateway** endpoint funcionando
- [ ] **Health check** respondiendo `/health`
- [ ] **Base de datos** conectando correctamente
- [ ] **Autenticación JWT** validando tokens
- [ ] **CORS** configurado para tu frontend
- [ ] **Logs** apareciendo en CloudWatch

---

## 🎉 **¡Listo para Producción!**

Una vez completados todos los pasos, tu API estará disponible en:
```
https://{api-id}.execute-api.us-east-1.amazonaws.com/Prod/
```

**Endpoints principales:**
- `GET /health` - Health check
- `POST /api/users` - Gestión de usuarios  
- `POST /api/account` - Cuentas Stellar
- `POST /api/transaction/send-xlm` - Envío de XLM
- `POST /api/transaction/send-usdc` - Envío de USDC
- `POST /api/trustline` - Gestión de trustlines

¡Tu aplicación Caprim API Stellar está lista para AWS Lambda! 🚀
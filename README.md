ReservasTucson API

Proyecto de gestión de reservas de mesas para eventos, desarrollado en .NET 8, con arquitectura DDD, y soporte para reservas estándar, VIP y de cumpleaños.

Requisitos Previos

Antes de ejecutar el proyecto, asegúrate de tener instalados:

.NET 8 SDK

Visual Studio 2022 o VSCode con extensión de C# (yo uitilicé Visual Studio 2022)

SQL Server Local 

Postman/Swagger  o similar para probar la API

Para pruebas unitarias e integración:

MSTest

Configuración del Proyecto

Clonar el repositorio:

[git clone https://github.com/tu-usuario/ReservasTucson.git
cd ReservasTucson]

(https://github.com/AntonellaCatanzano/ChallengeHipodromoPalermo/)

ReservasTucson/
│
├─ ReservasTucson.API/        # Proyecto WebApi
├─ ReservasTucson.Authentication/     # Implementación JWT y Usuarios
├─ ReservasTucson.DataAccess/ # Contexto de Base datos
├─ ReservasTucson.Domain/     # Entidades, DTOs, enums, profiles, builderTypesConfigurations
├─ ReservasTucson.Repositories/     # Repositorios
├─ ReservasTucson.Services/   # Lógica de negocio
├─ ReservasTucson.UnitnTests/ # Pruebas Unitarias
├─ ReservasTucson.IntegrationTests/ # Pruebas de integración

Configurar la cadena de conexión:

Editar appsettings.json:

"ConnectionStrings": {
  "ReservasTucsonDB": "Server=TU_SERVIDOR;Database=ReservasTucsonDB;Encrypt=false;",  
}

TU_SERVIDOR: Nombre de tu instancia de SQL Server Local

Encrypt=false: Solo para desarrollo local

Restaurar paquetes NuGet:

dotnet restore

Migraciones y base de datos:

dotnet ef database update --project ReservasTucson.DataAccess

Esto creará la base de datos y las tablas necesarias.

Ejecutar la API

Desde la carpeta del proyecto API:

dotnet run --project ReservasTucson.API

La API estará disponible por defecto en:

https://localhost:7156

Endpoints principales
Método	Ruta	Descripción
POST	/api/Reserva/CrearEstandar	Crea una reserva estándar
POST	/api/Reserva/CrearVip	Crea una reserva VIP
POST	/api/Reserva/CrearCumple	Crea una reserva de cumpleaños
POST	/api/Reserva/{reservaId}/Confirmar	Confirma una reserva
POST	/api/Reserva/Cancelar/{reservaId}	Cancela una reserva
POST	/api/Reserva/NoAsistio/{reservaId}	Marca como “No asistió”
GET	/api/Reserva/GetDetalle/{id}	Obtiene detalle de una reserva
GET	/api/Reserva/ListadoConFiltros	Obtiene reservas con filtros y paginación
GET	/api/Reserva/GetAll	Obtiene todas las reservas

(Nota: Se requiere autenticación JWT para todos los endpoints. Para endpoints de gestión, el rol debe ser Recepcionista.

Email: recepcion@tucson.com
Contraseña: recepcionTucson@2025) --> Se obtiene el accessToken y se copia y pega en el botón Authorize que aparece en Swagger y luego, recién ahí ejecutar los otros métodos (con login exitoso)

Ejecutar Pruebas de Integración

Navegar a la carpeta de tests:

cd ReservasTucson.IntegrationTests

Ejecutar pruebas:

dotnet test

Las pruebas usan una base de datos InMemory para no afectar la base de datos real.

Consideraciones

Formato de fecha/hora: "yyyy-MM-dd HH:mm"

Horarios permitidos:

Estándar: 19:00 a 23:30

VIP: 12:00 a 01:00

Cumpleaños: hasta las 23:00

Cantidad de personas:

Estándar: 1-4

VIP: 1-4

Cumpleaños: 5-12

Las reservas pueden ser confirmadas, canceladas o marcadas como “No asistió” según reglas de negocio.

Se incluye una carpeta con el .bak de la Base de Datos

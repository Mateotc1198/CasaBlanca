# Base de Datos

## Motor y conexion

La aplicacion usa SQL Server a traves de la cadena `CasaBlanca` definida en [Web.config](../Web.config).

Configuracion actual:

- Servidor: `.\SQLEXPRESS`
- Base de datos: `CasaBlancaHotelDB`
- Autenticacion: integrada de Windows

## Tablas principales

### `Usuarios`

Guarda la informacion de acceso y perfil del usuario.

Campos clave:

- `IdUsuario`
- `Nombre`
- `Apellido`
- `Correo`
- `Telefono`
- `Usuario`
- `Clave`
- `EsAdmin`
- `FechaRegistro`

### `Hoteles`

Representa el catalogo disponible para reservar.

Campos clave:

- `IdHotel`
- `Nombre`
- `Ciudad`
- `PrecioPorNoche`
- `CapacidadMaxima`
- `Imagen`
- `Activo`

### `Reservas`

Relaciona usuarios y hoteles con fechas, estado y total.

Campos clave:

- `IdReserva`
- `IdUsuario`
- `IdHotel`
- `Destino`
- `FechaEntrada`
- `FechaSalida`
- `Huespedes`
- `Estado`
- `PrecioPorNoche`
- `Noches`
- `Total`

## Relaciones

- Un usuario puede tener muchas reservas.
- Un hotel puede estar asociado a muchas reservas.
- `Reservas` referencia a `Usuarios` y `Hoteles` mediante claves foraneas.

## Script recomendado para el repositorio

Se agrego una version mas limpia del esquema y datos semilla para fines de portafolio:

- [CasaBlanca.portfolio.sql](database/CasaBlanca.portfolio.sql)

Esta version:

- Crea la base de datos.
- Crea las tablas en orden.
- Define las relaciones.
- Inserta hoteles de ejemplo.
- Inserta un usuario administrador con contrasena hasheada.

## Credenciales de ejemplo

Administrador sembrado en el script del repositorio:

- Usuario: `admin`
- Clave: `admin123`

## Nota importante

La aplicacion hashea contrasenas con SHA-256 antes de validarlas. Por eso, si se insertan usuarios manualmente desde SQL, la columna `Clave` debe guardar el hash y no el texto plano.

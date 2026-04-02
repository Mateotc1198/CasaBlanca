# Arquitectura de Casa Blanca

## Resumen

Casa Blanca es una aplicacion monolitica en ASP.NET MVC 5 sobre .NET Framework 4.7.2. La solucion usa una arquitectura por capas ligeras:

- Capa de presentacion con Razor Views.
- Capa de control con Controllers.
- Capa de dominio con Models.
- Capa de persistencia con clases DAO y ADO.NET.

## Patron de trabajo

### Controllers

Los controladores gestionan navegacion, validaciones adicionales y sesion de usuario.

- [Controllers/HomeControllers.cs](../Controllers/HomeControllers.cs): pagina principal y vista institucional.
- [Controllers/UsuarioControllers.cs](../Controllers/UsuarioControllers.cs): registro, login, logout y vista de confirmacion.
- [Controllers/HotelController.cs](../Controllers/HotelController.cs): listado publico de hoteles.
- [Controllers/ReservaController.cs](../Controllers/ReservaController.cs): creacion, consulta y cancelacion de reservas.
- [Controllers/AdminController.cs](../Controllers/AdminController.cs): dashboard y mantenimiento de hoteles.

### Models

Las clases del modelo mezclan entidades de dominio con acceso a datos.

- [Models/Usuario.cs](../Models/Usuario.cs)
- [Models/Hotel.cs](../Models/Hotel.cs)
- [Models/Reserva.cs](../Models/Reserva.cs)
- [Models/AdminDashboard.cs](../Models/AdminDashboard.cs)

### DAO

La persistencia se implementa con consultas SQL manuales y parametros para reducir riesgos de inyeccion.

- [Models/UsuarioDAO.cs](../Models/UsuarioDAO.cs)
- [Models/HotelDAO.cs](../Models/HotelDAO.cs)
- [Models/ReservaDAO.cs](../Models/ReservaDAO.cs)

## Flujo de autenticacion

1. El usuario se registra desde la vista `Registrar`.
2. La contrasena se transforma con SHA-256 en [Models/SeguridadHelper.cs](../Models/SeguridadHelper.cs).
3. En el login se compara el hash generado con el valor almacenado.
4. Si las credenciales son correctas, se guardan datos en `Session`.
5. Los controladores usan `Session["IdUsuario"]` y `Session["EsAdmin"]` para autorizar acciones.

## Flujo de reservas

1. El usuario autenticado entra a `Reserva/Nueva`.
2. El sistema puede precargar hotel, fechas y huespedes desde query string.
3. Se consulta el hotel seleccionado para completar nombre, ciudad, imagen y precio.
4. El total se calcula a partir de noches por precio por noche.
5. La reserva se registra con estado inicial `Pendiente`.
6. El usuario consulta sus reservas en `MisReservas` y puede cancelar si siguen pendientes.

## Decisiones tecnicas relevantes

- Arquitectura sencilla y facil de explicar para entornos academicos.
- Validaciones declarativas mediante Data Annotations.
- Validaciones de negocio adicionales en controladores y modelo `Reserva`.
- Acceso directo a SQL Server sin ORM, util para mostrar dominio de ADO.NET.

## Observaciones reales del estado del proyecto

- El dashboard administrativo si consume datos reales desde los DAO.
- La gestion administrativa de hoteles esta conectada de extremo a extremo.
- Existen vistas y metodos DAO para ampliar administracion de usuarios y reservas, pero hoy no todas esas acciones estan expuestas desde el controlador admin.
- La solucion esta orientada a un entorno local con `.\SQLEXPRESS`.

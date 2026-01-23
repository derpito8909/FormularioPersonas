<h1 align="center"> Prueba Técnica - Formulario Usuario y personas </h1>

## BackEnd RegistroApi (.NET 8 + SqlServer + JWT+ Fluent Validation)
### API REST: login con **JWT**, creacion de tablas Usuarios y personas, creacion de procedimiento almacenado para listar las personas, Conecta a **SQLServer**.
---
<details open>
<summary>
## ✅ Pre-requisitos
</summary>
Para ejecutar la aplicacion necesita tener instalado:

- Instacia de SQLServer
- Crear BD + tablas del programa con este script: 
[Create_DB.sql](./Create_DB.sql)
</details>

<details open>
<summary>
## 🚀 Ejecutar la aplicacion
</summary> <br>
para ejecutar la aplicacion:

1. Clone el repositorio:

```shell
https://github.com/derpito8909/FormularioPersonas.git
```
2. Ingrese a la carpeta /FormularioPersonas/API/ e ingrese estos comandos para inicar la aplicacion

```shell
 cd FormularioPersonas/API//
 dotnet restore
 dotnet build
 dotnet run --project Registro.Api/Registro.Api.csproj
```
</details>

<details open>
<summary>
Descripción de los Endpoint
</summary> <br />

## Endpoint: `POST /api/auth/login`

- **Método:** `POST`
- **Descripción:** autentica y devuelve un accessToken (JWT). credenciales fijas
en memoria

### Parámetros de Solicitud

- `usuario` (requerido): Usuario por defecto
- `pass` (requerido): Contraseña por defecto.

```json
{ "usuario": "admin", "pass": "Admin123!" }
```

### Respuesta

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwidW5pcXVlX25hbWUiOiJkYXZpZCIsImp0aSI6ImYyYzQxMjAxLTBiZWYtNGE0YS1iNmI1LWNkNjFkODk4MzE3ZCIsIm5iZiI6MTc2OTEzOTc2MCwiZXhwIjoxNzY5MTQwMzYwLCJpc3MiOiJSZWdpc3Ryb1BlcnNvbmEuQXBpIiwiYXVkIjoiUmVnaXN0cm9QZXJzb25hLkNsaWVudCJ9.0TGxwNwgcglqqgDvcgD0JVRuYO6hFwilxf7cHu-nZsA"
}
```
## Endpoint: `POST /api/personas` (Protegido) solo para usuarios autenticados

- **Método:** `POST`
- **Descripción:** Crear una persona nueva

### Parámetros de Solicitud

- `nombres` (requerido): cadena de caracteres con el nombre de la persona.
- `apellidos` (requerido): cadena de caracteres con el apellido de la persona.
- `tipoIdentificacion` (requerido): listado de tipos de identificacion Ej. "CC, TI, NIP"
-  `numeroIdentificacion` (requerido): codigo numerico de el numero de identificacion de la persona
-  `email` (requerido): cadena de caracteres que suministran el email de la persona"

```json
{
  "nombres": "yesinia",
  "apellidos": "martinez",
  "tipoIdentificacion": "CC",
  "numeroIdentificacion": "87895225",
  "email": "pedro@yahoo.com"
}
```

### Respuesta

```json
{
  "id": 4
}
```
## Endpoint GET /api/personas (Protegido) (solo parar autenticados)

- **Método:** `GET`
- **Descripción:** Lista personas usando el stored procedure 

### Parámetros de Solicitud

- `tipoId` (opcional): valor de cadena de caracters para representar el codigo de los tipos de identificacion (ej: CC, TI)
- `numeroId` (opcional): valor numerico del numero de identificacion de la persona
- `email` (opcional): valor  de cadena de caracteres para representar el correo electronica de la persona

### Respuesta

```json
[
    {
        "personaId": 1,
        "nombres": "pepito",
        "apellidos": "perez",
        "tipoIdentificacion": "CC",
        "numeroIdentificacion": "1019037537",
        "email": "petiyo@gmail.com",
        "identificacionCompleta": "CC-1019037537",
        "nombreCompleto": "pepito perez",
        "fechaCreacion": "2026-01-23T01:51:47"
    }
]
```
## Endpoint GET /api/personas/{id} (Protegido)(solo usuarios autenticados)

- **Método:** `GET`
- **Descripción:** obtiene una persona por el id

### Respuesta

```json
{
        "personaId": 4,
        "nombres": "yesinia",
        "apellidos": "martinez",
        "tipoIdentificacion": "CC",
        "numeroIdentificacion": "87895225",
        "email": "pedro@yahoo.com",
        "identificacionCompleta": "CC-87895225",
        "nombreCompleto": "yesinia martinez",
        "fechaCreacion": "2026-01-23T03:55:51"
    }
```
## Endpoint: `POST /api/usuarios`

- **Método:** `POST`
- **Descripción:** Registra un usuario nuevo

### Parámetros de Solicitud

- `usuario` (requerido): Usuario por defecto
- `pass` (requerido): Contraseña por defecto.

```json
{ "usuario": "admin", "pass": "Admin123!" }
```

### Respuesta

```json
{
  "id": 3
}
```
</details>

# FrontEnd Registro Usuarios y personas (. NET Razor 8)
Pagina web para visualizar la validacion del login y creacion de formularios para insertar Usuarios y personas
Incluye: **Login JWT**, **Menu con opciones**, **seccion de formularios para crear usuarios y peronas**
---
<details open>
<summary>
## ✅ Pre-requisitos
</summary>
Para ejecutar la aplicacion necesita tener instalado:  

- Acceso al **API** corriendo en local
</details>

<details open>
<summary>
## 🚀 Ejecutar la aplicacion
</summary> <br>
para ejecutar la aplicacion:

1. Clone el repositorio:

```shell
https://github.com/derpito8909/FormularioPersonas.git
```
2. Ingrese a la carpeta /FormularioPersonas/WEB/ e ingrese estos comandos para inicar la aplicacion

```shell
 cd FormularioPersonas/WEB/
 dotnet restore
 dotnet build
 dotnet run --project Registro.Web/Registro.Web.csproj
```
3. se visualiza pagina inicial de sitio web
   ![imagen](./index.png)

4. se ingresa con un usuario y contraseña, para la validacion de los datos de usuario
   ![imagen](./login.png)

5. pagina al ingresar cuando se valido el usuario y la contraseña
   ![imagen](./autenticado.png)

6. formulario para la creacion de usuario con los nombres de la tabla usuario creado en la BD
  ![imagen](./usuario.png)
  

7. formulario para la creacion de la persona con los nombres de la tabla persona creado en la BD
   ![imagen](./persona.png)
   
8. listado y busqueda de las personas creadas
     ![imagen](./verPersona.png)
   
8. ver infomacion especifica de una persona
     ![imagen](./detallePersona.png)
</details>

## Autor

<p>desarrollador por David Esteban Rodriguez Pineda 2026&copy;</p>

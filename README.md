# 🎓 Sistema de Gestión de Estudiantes (C# + Windows Forms)

Aplicación de escritorio desarrollada con **C# (.NET Framework 4.7.2)** y **Windows Forms** para administrar un padrón de estudiantes almacenado en una base de datos **MySQL**.  
El proyecto implementa una arquitectura **en tres capas** que separa la interfaz, la lógica de negocio y el acceso a datos.

---

## 🖼️ Vista general

> Interfaz principal del sistema y modo de edición y visualizacion con imagen de estudiante.

<img width="1598" height="857" alt="Sistema Estudiantes" src="https://github.com/user-attachments/assets/c18914b5-69b9-48f1-b4a7-0fd87e63cb0e" />

<img width="1599" height="851" alt="Sistema Estudiantes Editar" src="https://github.com/user-attachments/assets/f767f4ef-d50a-47e9-af9d-d7bb7148d27b" />

---

## ✨ Características principales

- 🧾 **Altas, bajas y modificaciones** de estudiantes.  
- 🔍 **Búsqueda incremental** por DNI, nombre, apellido o email.  
- 🧠 **Validaciones en capa lógica y visual**, evitando duplicados y formatos inválidos.  
- 📸 **Carga de fotografía opcional** por estudiante.  
- 📄 **Paginación configurable** en el `DataGridView`.  
- ⚙️ Arquitectura modular: *Data*, *Logica* y *Estudiantes (UI)*.

---

## 🧱 Estructura del proyecto

```text
Sistema_Estudiantes-CSharp-y-WindowsForms/
├── Data/
│   ├── Conexion.cs                   # Conexión LinqToDB con MySQL
│   └── Estudiante.cs                 # Modelo de datos
├── Logica/
│   ├── LEstudiantes.cs               # Lógica de negocio (validaciones, CRUD, paginación)
│   └── Library/
│       ├── Librarys.cs
│       ├── Paginador.cs
│       ├── TextBoxEvent.cs
│       └── Uploadimage.cs
└── Estudiantes/
    ├── Form1.cs                      # Lógica de la interfaz
    └── Form1.Designer.cs             # Diseño visual del formulario
```

## 🧰 Requisitos previos

Windows 10 o superior
Visual Studio 2022 con soporte para .NET Framework 4.7.2
MySQL Server 8 o compatible
Paquetes NuGet restaurados: linq2db, MySql.Data, etc.


## ⚙️ Configuración de la base de datos

El connectionString con nombre LM1 se encuentra en Estudiantes/App.config:

<connectionStrings>
  <add name="LM1" 
       connectionString="Server=localhost;Port=3306;Database=estudiantes_cdb;Uid=root;Pwd=password;charset=utf8;SslMode=Preferred" 
       providerName="MySql.Data.MySqlClient"/>
</connectionStrings>

Ajustá los valores de usuario (Uid) y contraseña (Pwd) según tu entorno local.


## 🚀 Ejecución del proyecto

1. Abrí el archivo Estudiantes.sln en Visual Studio.
2. Restaurá los paquetes NuGet (Restore NuGet Packages).
3. Actualizá la conexión en App.config si es necesario.
4. Compilá la solución (Build → Build Solution).
5. Ejecutá el proyecto Estudiantes (F5 o Start).

## 💡 Flujo de uso

1. Agregar estudiante → completa los campos y presioná Agregar.
2. Seleccionar y editar → elegí un registro del DataGridView, modificá los datos y presioná Actualizar.
3. Eliminar → seleccioná un registro y presioná Borrar.
4. Cancelar → limpia el formulario y vuelve al modo de inserción.
5. Buscar / paginar → usá el cuadro de búsqueda o los botones inferiores para navegar entre páginas.

## 🧩 Validaciones y usabilidad

- Las validaciones se ejecutan tanto en la UI como en la lógica de negocio (LEstudiantes).
- Se controlan duplicados de DNI y email, campos vacíos y formato de correo.
- Los mensajes se muestran directamente debajo de los campos, con colores según el tipo de error.

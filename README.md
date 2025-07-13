# 📦 Sistema de Gestión de Envíos

## 📌 Introducción

Una empresa dedicada a la **logística de envíos de paquetes** comenzará a operar en todo el territorio nacional y nos ha encargado el desarrollo de un **sistema de gestión interna y comunicación con clientes**. Se busca hacer especial énfasis en la **trazabilidad de envíos** y una **comunicación fluida y eficiente** con los clientes.

---

## 🏆 Obligatorio 1: Gestión Interna del Sistema

### 🏢 Información de la Empresa

La empresa cuenta con múltiples agencias, cada una identificada por:
- 🏷️ **Nombre**
- 📍 **Dirección postal**
- 🌍 **Ubicación** (coordenadas de latitud y longitud)

### 🔑 Gestión de Usuarios

El sistema manejará distintos **roles de usuario**, diferenciando empleados de clientes:
- 🛠️ **Administrador:** Gestiona empleados y operaciones generales del sistema.
- 📦 **Funcionario:** Maneja envíos y el seguimiento de paquetes.
- 👤 **Cliente:** Realiza envíos y consulta su estado.

Los usuarios se registran con sus datos personales, **email y contraseña**.

### 📦 Gestión de Envíos

Cada envío posee:
- 🆔 **ID único**
- 🔎 **Número de tracking**
- 🏢 **Agencia de destino o dirección postal**
- 👨‍💼 **Empleado que toma el pedido**
- 👤 **Cliente que lo realiza**
- ⚖️ **Peso del paquete**
- 🚦 **Estado:** `EN_PROCESO` | `FINALIZADO`

#### 🚛 Tipos de Envíos

- **Común:** Se entrega en una **agencia** para retiro en persona.
- **Urgente:** Se entrega a una **dirección postal específica**, y su eficiencia se mide por si la entrega ocurre en **menos de 24 horas desde su salida**.

### 🔎 Seguimiento de Envíos

Cada envío contará con **etapas de seguimiento**, indicando su recorrido desde la agencia de origen hasta el cliente. Los empleados pueden ingresar comentarios con:
- 📅 **Fecha de registro**
- 👨‍💼 **Empleado que realizó el comentario**
- 📝 **Estados posibles:** `"Ingresado en agencia de origen"`, `"En camino"`, `"Listo para retirar"` y otros definidos por los empleados.

---

### ⚙️ Requerimientos Funcionales

#### RF1 – 🔑 Login de Usuario
- Solo los **empleados** podrán acceder con sus credenciales.
- Se habilitarán funcionalidades según el rol (`Administrador`, `Funcionario`).

#### RF2 – 🛠️ Gestión de Empleados (**Solo Administrador**)
- **CRUD de empleados** (alta, edición y baja).
- Todas las acciones deberán ser **auditables**, registrando fecha, acción y usuario.

#### RF3 – 🚚 Alta de Envío (**Para cualquier rol**)
- Registro de tipo de envío, email del cliente, agencia de destino o dirección postal, y peso del paquete.
- Se guardan valores predeterminados.

#### RF4 – ✅ Finalizar un Envío (**Para cualquier rol**)
- Listado de envíos **en proceso**.
- Posibilidad de **finalizar** agregando fecha de entrega.

#### RF5 – ✍️ Ingreso de Comentario de Estado (**Para cualquier rol**)
- Listado de envíos en proceso.
- Posibilidad de agregar **estado de seguimiento**.

#### RF6 – 🔍 Obtener Detalles de un Envío (**Sin autenticación**)
- **API pública** que permite consultar los detalles de un envío mediante su **número de tracking**.
- Se probará con **Postman o Swagger**.

---

### 📌 Estado de Desarrollo:  
![Finalizado](https://img.shields.io/badge/Obligatorio%201-Finalizado-brightgreen?style=for-the-badge&logo=checkmark)  

---

## 🚀 Obligatorio 2: API para Clientes y Consultas Externas

### 🌐 Desarrollo de Web API y Aplicación Cliente

En esta fase, se implementarán funcionalidades dirigidas a **clientes de la agencia**, exponiéndolas mediante **endpoints en una Web API Restful** y consumiéndolas desde una **aplicación MVC con HttpClient**.

### 🔧 Requerimientos Funcionales

#### RF1 – 📦 Consultar detalles de un envío (**Sin autenticación**)
- Permite consultar el detalle completo de un envío **por su número de tracking**, incluyendo seguimientos registrados.

#### RF2 – 🔐 Login / Logout (**Sin autenticación inicial, luego con JWT**)
- Se implementará **autenticación con JWT**. Inicialmente los endpoints estarán abiertos y luego se agregarán restricciones.
- El cliente podrá iniciar sesión desde la **aplicación Cliente HTTP**.

#### RF3 – 🔄 Cambiar contraseña (**Autenticado con rol cliente**)
- Los clientes podrán modificar su contraseña ingresando la **actual** y la **nueva** (solo si la actual es válida).

#### RF4 – 📜 Listar todos los envíos del cliente (**Autenticado con rol cliente**)
- Muestra un **historial de envíos** ordenado por **fecha de creación**, con acceso a los seguimientos en una vista separada.

#### RF5 – 📅 Búsqueda de envíos por fecha (**Autenticado con rol cliente**)
- Permite filtrar envíos por **fecha de inicio**, **fecha de fin** y **estado**.
- Resultados ordenados por **número de tracking**.

#### RF6 – 🔍 Búsqueda de envíos por comentario (**Autenticado con rol cliente**)
- Se podrá buscar envíos **que contengan una palabra clave** en sus comentarios de seguimiento.
- Ejemplo: `"Demorado"` o `"En camino"`.

---

## 🏗️ Implementación Técnica

### 🚀 Desarrollo de la Web API
✔ Se expondrán los **endpoints RESTful** adecuados.
✔ La API estará **documentada en Swagger**.

### 🎛️ Cliente MVC para consumir la API
✔ Se desarrollará una **aplicación MVC** usando **HttpClient**.
✔ La solución del cliente estará **separada** de la API.

### 🛠️ Precarga de Datos y Documentación
✔ Se generarán **datos de prueba con ChatGPT**.
✔ Se documentará el proceso con:
  - 📷 Capturas de **prompts utilizados**.
  - 📜 **Scripts de inserts** con datos coherentes.

---

### 📌 Estado de Desarrollo:  
![Finalizado](https://img.shields.io/badge/Obligatorio%202-Finalizado-brightgreen?style=for-the-badge&logo=checkmark)

---

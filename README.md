# BlogEngine
.NET Blog Engine App 

Descripción: 
Web API: Asp.Net Core 3.0 
Frontend: Proyecto Web ASP.NET MVC Core app con React usando la opción Individual User Accounts para el manejo de la autenticación en la aplicación

El proyecto web es el encargado del manejo de la autenticación, por esto al ejecutar la aplicación se debe iniciar la DB para el manejo de usuarios, se incluyeron un par de datos semilla para acceder al sitio con los perfiles de editor y escritor Estos son los datos de acceso:

Editor user: celesteq@gmail.com psw: Oscar123.

Writer user: oscarse@gmail.com psw: Oscar123.

En el archivo appsettings.json del proyecto BlogEngine se encuentra la url para acceder a la API "https://localhost:44355/api/blog/". Para evitar errores de "crossdomain access denied" se creo esta variable en "AllowedDomains": "https://localhost:44336" en el archivo appsettings.json del proyecto BlogEngine.Api.

Se creo una DB llamada BlogDB en el localhost (utilizar el script BlogEngine.Data/Data/BlogDB.sql para crear la DB localmente) Para actualizar el nombre del servidor donde se despliegue la DB por favor modificar el valor de la variable DBServer en el archivo appsettings.json del proyecto BlogEngine.Api

Para la ejecución en un ambiente local, se debe crear la DB como se describe anteriormente, luego se debe ejecutar el API (proyecto BlogEngine.Api) y luego la aplicación (Proyecto BlogEngine)

El tiempo empleado en el desarrollo de la prueba fueron alrededor de 15 horas

Cualquier duda o problema con la ejecución del proyecto por favor comunicarse a oscarse@gmail.com

Gracias,

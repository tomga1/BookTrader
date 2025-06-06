# BookTrader
COMANDO PARA AGREGAR MIGRATION
dotnet ef migrations add "Rutade imagenes agregado"

ACTUALIZAR BASE
dotnet ef database update




////////////////////////////////////////////

Instalacion de paquetes ENTITY FRAMEWORK

**Microsoft.EntityFramworkCore.SqlServer
**Microsoft.EntityFramworkCore.Tools

///////////////////////////////////////////


 CONECTION STRING SERVER SQL DER 

  //"DefaultConnection": "Server=SIS-05\\SQLEXPRESS;Database=BookTrader;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"

  
/////////////////////////////////////////

 CONECTION STRING SERVER SQL CASA 

   "DefaultConnection": "Server=DESKTOP-RDT17TE\\SQLEXPRESS;Database=BookTrader;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"



   ////////////////////////
   ✅ 5. Prueba Ejecutar la Migración con la Cadena de Conexión Explícita
En la terminal, intenta ejecutar este comando con la cadena de conexión directamente:

powershell
Copiar 
Editar
dotnet ef database update --connection "Server=DESKTOP-RDT17TE\SQLEXPRESS;Database=BookTrader;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
